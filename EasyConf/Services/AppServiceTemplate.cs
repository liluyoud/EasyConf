using EasyConf.Extensions;
using System.Text.Json;

namespace EasyConf.Services;

public partial class AppService
{
    public async Task<string> GetEasyTemplate()
    {
        var easy = "";
        var globals = await GetGlobalsAsync();
        var templates = await GetTemplatesAsync();
        if (globals != null && templates != null)
        {
            foreach (var global in globals)
            {
                templates = templates.Replace("{{" + global.Key + "}}", global.Value);
            }
            easy = templates;
        }
        return easy;
    }

    private string? ReplaceBaseKey(string env)
    {
        if (!string.IsNullOrEmpty(Key))
            env = env.Replace("{{EASY_KEY}}", Key);
        return env;
    }

    private async Task<Dictionary<string,string>> GetGlobalsAsync()
    {
        var envs = new Dictionary<string, string>();
        var strEnv = await GetModuleAsync(EASY_ENVIRONMENT);
        if (strEnv != null)
        {
            var lines = strEnv.Split("\n");
            foreach (var line in lines)
            {
                var cleanLine = line.Replace("\r", "");
                cleanLine = cleanLine.Trim();
                if (cleanLine.Length > 0 && cleanLine[0] != '#')
                {
                    var fields = cleanLine.Split('=');
                    envs.Add(fields[0], fields[1]);
                }
            }
        }
        return envs;
    }

    private async Task<string> GetTemplatesAsync()
    {
        var jsonEasy = "";
        if (Modules != null)
        {
            var selectedModules = Modules.Where(m => m.Selected);
            List<string> jsonTemplate = new();
            foreach (var module in selectedModules)
            {
                if (!string.IsNullOrEmpty(module.Name))
                {
                    var envModule = await GetModuleAsync(module.Name.ToEnv());
                    var jsonModule = await GetModuleAsync(module.Name.ToJson());
                    if (jsonModule != null)
                    {
                        jsonModule = jsonModule.RemoveArray();
                        envModule = envModule?.Replace("\n", "\\n");
                        envModule = envModule?.Replace("\r", "");
                        jsonModule = jsonModule.Replace("{{EASY_ENVS}}", envModule);
                        // verify if env is diferent for each image
                        if (envModule != null && envModule.Contains("###ENV_PART###"))
                        {
                            var envModules = envModule.Split("###ENV_PART###");
                            for (var i = 1; i < envModules.Length; i++)
                            {
                                jsonModule = jsonModule.Replace("{{EASY_ENVS_" + i + "}}", envModules[i]);
                            }
                        }
                        jsonTemplate.Add(jsonModule);
                    }
                }
            }
            if (jsonTemplate.Count > 0)
                jsonEasy = string.Join(",\n", jsonTemplate.ToArray());
            jsonEasy = "{\n\"services\": [\n\n" + jsonEasy + "\n\n]\n}";
        }
        return jsonEasy;
    }
}

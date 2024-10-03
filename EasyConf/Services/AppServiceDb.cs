namespace EasyConf.Services;

public partial class AppService
{
    public async Task<bool> ExistsModule(string? module)
    {
        if (module != null)
        {
            var exists = await _storage.ContainKeyAsync(module);
            return exists;
        }
        return false;
    }

    public async Task<string?> GetModuleAsync(string module)
    {
        if (await ExistsModule(module))
        {
            var strModule = await _storage.GetItemAsync<string>(module);
            if (module == EASY_ENVIRONMENT && strModule != null)
            {
                strModule = ReplaceBaseKey(strModule);
            }
            return strModule;
        }
        return null;
    }

    public async Task SaveModuleAsync(string? module, string? data)
    {
        if (module != null && data != null)
        {
            await _storage.SetItemAsync(module, data);
        }
    }

    public async Task<string?> ResetModuleAsync(string module)
    {
        try
        {
            var url = $"data/{module}";
            var strModule = await _http.GetStringAsync(url);
            if (strModule != null && !strModule.Contains("DOCTYPE html"))
            {
                await _storage.SetItemAsync(module, strModule);
                if (module == EASY_ENVIRONMENT)
                    strModule = ReplaceBaseKey(strModule);
                return strModule;
            }
        }
        catch { }
        return null;
    }

    public async Task SaveModules()
    {
        await _storage.SetItemAsync(EASY_MODULES, Modules);
    }

}

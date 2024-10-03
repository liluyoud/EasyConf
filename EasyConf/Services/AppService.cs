using Blazored.LocalStorage;
using EasyConf.Extensions;
using EasyConf.Helpers;
using EasyConf.Models;
using System.Net.Http.Json;
using System.Reflection;

namespace EasyConf.Services;

public partial class AppService
{
    private readonly ILogger<AppService> _logger;
    private readonly HttpClient _http;
    private readonly ILocalStorageService _storage;

    public readonly string EASY_KEY = "easy_key";
    public readonly string EASY_GROUPS = "easy_groups";
    public readonly string EASY_MODULES = "easy_modules";

    public readonly string EASY_ENVIRONMENT = "_base.env";

    public string? Version { get; set; }
    public string? Key { get; set; }
    public List<string>? Groups { get; set; }
    public List<ModuleModel>? Modules { get; set; }

    public AppService(ILogger<AppService> logger, HttpClient http, ILocalStorageService storage)
    {
        _logger = logger;
        _http = http;
        _storage = storage;
        Version = Assembly.GetExecutingAssembly()
                          .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                          .InformationalVersion ?? "1.0.0";
    }

    public async Task InitDb()
    {
        Key = await GenerateKeyAsync();
        Groups = await GetGroupsAsync();
        Modules = await GetModulesAsync();
    }

    private async Task<string> GenerateKeyAsync()
    {
        // generate key
        var key = await _storage.GetItemAsync<string>("easy_key");
        if (string.IsNullOrEmpty(key))
        {
            key = AppHelper.GenerateKey(32);
            await _storage.SetItemAsync("easy_key", key);
        }
        return key;
    }

    private async Task<List<string>?> GetGroupsAsync()
    {
        // generate key
        var groups = await _storage.GetItemAsync<List<string>>("easy_groups");
        if (groups == null)
        {
            groups = await _http.GetFromJsonAsync<List<string>>($"data/_groups.json");
            await _storage.SetItemAsync("easy_groups", groups);
        }
        return groups;
    }

    private async Task<List<ModuleModel>?> GetModulesAsync()
    {
        var modules = await _storage.GetItemAsync<List<ModuleModel>>("easy_modules");
        if (modules == null)
        {
            modules = await _http.GetFromJsonAsync<List<ModuleModel>>($"data/_modules.json");
            if (modules != null)
            {
                await ResetModuleAsync(EASY_ENVIRONMENT);
                foreach (var module in modules)
                {
                    var jsonModule = await ResetModuleAsync(module.Name!.ToJson());
                    module.Enabled = (jsonModule != null);
                    var envModule = await ResetModuleAsync(module.Name!.ToEnv());
                }
            }
            await _storage.SetItemAsync("easy_modules", modules);
        }
        return modules;
    }
}

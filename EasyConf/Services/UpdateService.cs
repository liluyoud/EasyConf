using Microsoft.JSInterop;

namespace EasyConf.Services;

public class UpdateService
{
    private readonly IJSRuntime _jsRuntime;

    public UpdateService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task CheckForUpdatesAsync()
    {
        await _jsRuntime.InvokeVoidAsync("checkForServiceWorkerUpdate");
    }
}


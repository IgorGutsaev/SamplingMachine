using Microsoft.JSInterop;

public class BrowserService
{
    private readonly IJSRuntime _js;

    public BrowserService(IJSRuntime js) {
        _js = js;
    }

    public async Task<BrowserDimension> GetDimensionsAsync() 
        => await _js.InvokeAsync<BrowserDimension>("ui.getDimensions");

    public async Task<int> GetReplenishmentScreenWidthAsync() {
        try {
            return await _js.InvokeAsync<int>("ui.getReplenishmentScreenWidth");
        }
        catch {
            return 1280;
        }
    }
}

public class BrowserDimension
{
    public int Width { get; set; }
    public int Height { get; set; }
}
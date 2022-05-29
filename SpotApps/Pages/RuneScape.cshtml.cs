using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;

namespace SpotApps.Pages;

public class RuneScapeModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public RuneScapeModel(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public string PlayerName { get; } = "wh1teberry";
    public int Rank { get; private set; }
    public int Xp { get; private set; }

    private Task<string> GetDataAsync()
    {
        var key = $"RuneScape:${PlayerName}";

        return _cache.GetOrCreateAsync(
            key,
            cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6);

                var url = $"https://secure.runescape.com/m=hiscore/index_lite.ws?player={PlayerName}";
                return _httpClient.GetStringAsync(url);
            }
        );
    }

    public async Task OnGetAsync()
    {
        var text = await GetDataAsync();

        var match = Regex.Match(text, @"(\d+),\d+,(\d+)");

        if (match == null)
        {
            throw new Exception("Failed to parse result.");
        }

        Rank = int.Parse(match.Groups[1].Value);
        Xp = int.Parse(match.Groups[2].Value);
    }
}

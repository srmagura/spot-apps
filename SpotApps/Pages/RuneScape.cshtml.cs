using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace SpotApps.Pages;

public class RuneScapeModel : PageModel
{
    private static readonly HttpClient _httpClient = new();

    public string PlayerName { get; } = "wh1teberry";
    public int Rank { get; private set; }
    public int Xp { get; private set; }

    public async Task OnGetAsync()
    {
        var url = $"https://secure.runescape.com/m=hiscore/index_lite.ws?player={PlayerName}";
        var text = await _httpClient.GetStringAsync(url);

        var match = Regex.Match(text, @"(\d+),\d+,(\d+)");

        if (match == null)
        {
            throw new Exception("Failed to parse result.");
        }

        Rank = int.Parse(match.Groups[1].Value);
        Xp = int.Parse(match.Groups[2].Value);
    }
}

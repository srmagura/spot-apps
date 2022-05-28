using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SpotApps.Pages;

public class RuneScapeModel : PageModel
{
    public string PlayerName { get; } = "wh1teberry";
    public int Rank { get; private set; }
    public int Xp { get; private set; }

    public async Task OnGetAsync()
    {
        Rank = 117029;
        Xp = 580123123;
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace SpotApps.Pages;

public class TweetResponse
{
    public TweetResponse(Tweet[] data)
    {
        Data = data;
    }

    public Tweet[] Data { get; set; }
}

public class Tweet
{
    public Tweet(string id, string text)
    {
        Id = id;
        Text = text;
    }

    public string Id { get; set; }
    public string Text { get; set; }
}

public class TwitterModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly string _bearerToken;

    public TwitterModel(HttpClient httpClient, IMemoryCache cache, ApiKeySettings apiKeySettings)
    {
        _httpClient = httpClient;
        _cache = cache;
        _bearerToken = apiKeySettings.TwitterBearerToken;
    }

    public string Username { get; } = "elonmusk";
    public string? Tweet { get; private set; }

    private Task<string> GetDataAsync()
    {
        var key = $"MostRecentTweet:${Username}";

        return _cache.GetOrCreateAsync(
            key,
            async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6);

                // Lookup user ID via https://oauth-playground.glitch.me/?id=findUserByUsername

                var url = $"https://api.twitter.com/2/users/44196397/tweets?max_results=5&exclude=replies%2Cretweets";
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

                var response = await _httpClient.SendAsync(request);
                var tweetResponse = await response.Content.ReadFromJsonAsync<TweetResponse>();

                if (tweetResponse == null)
                {
                    throw new Exception("Failed to parse response.");
                }

                return tweetResponse.Data[0].Text;
            }
        );
    }

    public async Task OnGetAsync()
    {
        Tweet = await GetDataAsync();
    }
}

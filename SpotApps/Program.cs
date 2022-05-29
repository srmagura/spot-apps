using SpotApps;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

builder.Configuration.AddJsonFile("appsettings.DevelopmentSecrets.json");

var apiKeySettings = builder.Configuration.GetSection(nameof(ApiKeySettings)).Get<ApiKeySettings>();
builder.Services.AddSingleton(apiKeySettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

using UNIR.TFE.Monorepo.WebApp.Infrastructure.External.GitHub;
using UNIR.TFE.Monorepo.WebApp.Infrastructure.External.GitHub.Impl;
using UNIR.TFE.Monorepo.WebApp.Services;
using UNIR.TFE.Monorepo.WebApp.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICalculatorService, CalculatorService>();
builder.Services.AddScoped<IGitRepositoryAnalyzerService, GitRepositoryAnalyzerService>();
builder.Services.AddScoped<IGitHubRepositoryService, GitHubRepositoryService>();
builder.Services.AddScoped<IGitHubUrlParser, GitHubUrlParser>();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

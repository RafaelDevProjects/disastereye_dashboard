using DisasterEye.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// ── OpenTelemetry (Aspire Dashboard) ─────────────────────────────
const string serviceName = "DisasterEye.Web";

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService(serviceName))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation());

var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
if (!string.IsNullOrWhiteSpace(otlpEndpoint))
{
    builder.Services.ConfigureOpenTelemetryTracerProvider(t => t.AddOtlpExporter());
    builder.Services.ConfigureOpenTelemetryMeterProvider(m => m.AddOtlpExporter());
    builder.Logging.AddOpenTelemetry(l => l.AddOtlpExporter());
}

// ── HttpClient → API (Web NUNCA acessa o banco diretamente) ───────
var apiUrl = builder.Configuration["services:apiservice:https:0"]
    ?? builder.Configuration["services:apiservice:http:0"]
    ?? builder.Configuration["ApiBaseUrl"]
    ?? "http://localhost:5001";

builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

// ── Autenticação por Cookie com Claims ────────────────────────────
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AcessoNegado";
        options.Cookie.Name = "DisasterEye.Auth";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

// ── Autorização por perfil via Claims ─────────────────────────────
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApenasAdmin", policy =>
        policy.RequireClaim("Perfil", "Administrador"));
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
using DisasterEye.ApiService.Data;
using DisasterEye.ApiService.Repositories;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// ── OpenTelemetry (Aspire Dashboard) ─────────────────────────────
const string serviceName = "DisasterEye.ApiService";

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

// Exporta para o Aspire Dashboard via OTLP se a variável estiver definida
var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
if (!string.IsNullOrWhiteSpace(otlpEndpoint))
{
    builder.Services.ConfigureOpenTelemetryTracerProvider(t => t.AddOtlpExporter());
    builder.Services.ConfigureOpenTelemetryMeterProvider(m => m.AddOtlpExporter());
    builder.Logging.AddOpenTelemetry(l => l.AddOtlpExporter());
}

// ── Banco de Dados Oracle via EF Core ─────────────────────────────
var connStr = builder.Configuration.GetConnectionString("OracleConnection")
    ?? "User Id=rm554019;Password=060304;Data Source=oracle.fiap.com.br:1521/ORCL;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connStr));

// ── Repositories (padrão Repository com Interfaces) ───────────────
builder.Services.AddScoped<ITecnologiaRepository, TecnologiaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// ── Swagger ───────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "DisasterEye API",
        Version = "v1",
        Description = "API REST para gerenciamento de tecnologias espaciais de monitoramento de desastres naturais."
    });
});

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(p =>
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// ── Migrations automáticas ao iniciar ────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();
app.Run();
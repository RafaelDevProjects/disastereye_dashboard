// Permite HTTP em desenvolvimento
Environment.SetEnvironmentVariable("ASPIRE_ALLOW_UNSECURED_TRANSPORT", "true");

// Portas altas fora de qualquer faixa reservada
Environment.SetEnvironmentVariable("ASPNETCORE_URLS", "http://localhost:47000");
Environment.SetEnvironmentVariable("DOTNET_DASHBOARD_OTLP_ENDPOINT_URL", "http://localhost:47001");
Environment.SetEnvironmentVariable("DOTNET_DASHBOARD_OTLP_HTTP_ENDPOINT_URL", "http://localhost:47001");
Environment.SetEnvironmentVariable("DOTNET_RESOURCE_SERVICE_ENDPOINT_URL", "http://localhost:47002");

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.DisasterEye_ApiService>("apiservice");

builder.AddProject<Projects.DisasterEye_Web>("webfrontend")
    .WithReference(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
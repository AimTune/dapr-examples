using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// builder.Services.AddDaprClient();

builder.Services.AddSingleton<DaprClient>(instance =>
{
    return new DaprClientBuilder()
        // .UseHttpEndpoint("http://localhost:3500/")
        .UseGrpcEndpoint("http://localhost:50001/")
        .UseJsonSerializationOptions(default)
        .Build();
});

var app = builder.Build();

app.UseRouting();
app.UseCloudEvents();
app.MapControllers();

app.MapGet("/get-secret", async ([FromServices] DaprClient daprClient) =>
{
    var secret = await daprClient.GetSecretAsync("vault", "mysecret");
    return Results.Ok(secret);
});

app.MapGet("/healthz", () => Results.Ok("OK"));

app.Run();

using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Dapr.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// builder.Services.AddDaprClient();

DaprClient daprClient = new DaprClientBuilder()
        // .UseHttpEndpoint("http://localhost:3500/")
        .UseGrpcEndpoint("http://localhost:50001/")
        .UseJsonSerializationOptions(default)
        .Build();

builder.Services.AddSingleton<DaprClient>(instance =>
{
    return daprClient;
});


builder.Configuration.AddDaprSecretStore("vault", daprClient, TimeSpan.FromSeconds(20));
// builder.Configuration.AddDaprConfigurationStore("configstore", [], daprClient, TimeSpan.FromSeconds(60));

var app = builder.Build();

app.UseRouting();
app.UseCloudEvents();
app.MapControllers();

app.MapGet("/get-secret", async ([FromServices] DaprClient daprClient) =>
{
    var secret = await daprClient.GetSecretAsync("vault", "mysecret");
    return Results.Ok(secret);
});

app.MapGet("/get-configuration", async ([FromServices] IConfiguration configuration) =>
{
    var newConfiguration = await daprClient.GetConfiguration("configstore", new List<string>() { "orderId1", "orderId2" });

    return Results.Ok(new
    {
        FirstKey = configuration["firstKey"],
        SecondKey = configuration["secondKey"],
        ThirdKey = configuration["thirdKey"],
        OrderId1 = newConfiguration.Items["orderId1"].Value,
        OrderId2 = newConfiguration.Items["orderId2"].Value
    });
});

app.MapGet("/healthz", () => Results.Ok("OK"));

app.Run();

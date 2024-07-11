using Dapr.Client;

const string DAPR_CONFIGURATION_STORE = "configstore";
var CONFIGURATION_ITEMS = new List<string> { "orderId1", "orderId2" };
string subscriptionId = string.Empty;

var client = new DaprClientBuilder().Build();

// Get config from configuration store
GetConfigurationResponse config = await client.GetConfiguration(DAPR_CONFIGURATION_STORE, CONFIGURATION_ITEMS);
foreach (var item in config.Items)
{
    var cfg = System.Text.Json.JsonSerializer.Serialize(item.Value);
    Console.WriteLine("Configuration for " + item.Key + ": " + cfg);
}

// Subscribe for configuration changes
SubscribeConfigurationResponse subscribe = await client.SubscribeConfiguration(DAPR_CONFIGURATION_STORE, CONFIGURATION_ITEMS);

// Print configuration changes
await foreach (var configItem in subscribe.Source)
{
    // First invocation when app subscribes to config changes only returns subscription id
    if (configItem.Keys.Count == 0)
    {
        Console.WriteLine("App subscribed to config changes with subscription id: " + subscribe.Id);
        subscriptionId = subscribe.Id;
        continue;
    }
    var cfg = System.Text.Json.JsonSerializer.Serialize(configItem);
    Console.WriteLine("Configuration update " + cfg);
}

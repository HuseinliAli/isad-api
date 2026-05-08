using System.Text.Json;

namespace EventBus.Base;

public class EventBusConfig
{
    public int ConnectionRetryCount { get; set; } = 5;
    public string DefaultTopicName { get; set; } = "LMSEventBus";
    public string ConnectionString { get; set; } = string.Empty;
    public string SubscriberAppName { get; set; } = string.Empty;
    public string EventNamePrefix { get; set; } = string.Empty;
    public string EventNameSuffix { get; set; } = "IntegrationEvent";
    public JsonElement? Connection { get; set; }
    public bool RemoveEventSuffix => !string.IsNullOrEmpty(EventNameSuffix);
    public bool RemoveEventPrefix => !string.IsNullOrEmpty(EventNamePrefix);
}
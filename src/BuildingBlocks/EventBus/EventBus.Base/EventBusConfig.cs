namespace EventBus.Base;

public class EventBusConfig
{
    public int ConnectionRetryCount { get; set; } = 5;
    public string DefaultTopicName { get; set; } = "LMSEventBus";
    public string ConnectionString { get; set; } = String.Empty;
    public string SubscriberAppName { get; set; } = String.Empty;
    public string EventNamePrefix { get; set; } = String.Empty;
    public string EventNameSuffix {  get; set; } = "IntegrationEvent";
    public object Connection { get; set; }

    public bool RemoveEventSuffix => !String.IsNullOrEmpty(EventNameSuffix);
    public bool RemoveEventPrefix => !String.IsNullOrEmpty(EventNamePrefix);
}

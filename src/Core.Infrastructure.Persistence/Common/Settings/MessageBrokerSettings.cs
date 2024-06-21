namespace Core.Infrastructure.Persistence.Common.Settings;

public class MessageBrokerSettings
{
    public const string Section = "MessageBrokerSettings";

    public string HostName { get; init; }
    
    public int Port { get; init; }
    
    public string UserName { get; init; }
    
    public string Password { get; init; }
    
    public string QueueName { get; init; }
    
    public string ExchangeName { get; init; }
}
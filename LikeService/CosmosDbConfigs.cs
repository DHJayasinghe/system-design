namespace LikeService;

public static class CosmosDbConfigs
{
    public const string DatabaseName = "like-service";
    public const string ContainerName = "reaction";
    public const string ConnectionName = "CosmosDB";
}

public static class ServiceBusConfigs
{
    public const string TopicName = "reaction";
    public const string ConnectionName = "ServiceBus";
    public const string SubscriptionName = "like-service";
}
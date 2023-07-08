using Newtonsoft.Json;

namespace SharedKernal;

public record EventBusMessageWrapper
{
    public string Assembly { get; init; }
    public string TypeName { get; init; }
    public object Payload { get; init; }

    public EventBusMessageWrapper() { }
    public EventBusMessageWrapper(object payload) : this()
    {
        Payload = payload;
        Assembly = payload.GetType().AssemblyQualifiedName;
        TypeName = payload.GetType().Name;
    }

    public Type GetType(Type callerContext = null) =>
        callerContext == null
           ? Type.GetType(Assembly)
           : callerContext.Assembly.GetType($"{callerContext.Namespace}.{TypeName}");

    public T Convert<T>() where T : class => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(Payload));
}
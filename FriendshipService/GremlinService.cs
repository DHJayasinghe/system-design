***REMOVED***
using System.Net.WebSockets;
using Gremlin.Net.Driver;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;
***REMOVED***

namespace FriendshipService;

public class GremlinService
***REMOVED***
    private static GraphTraversalSource g = AnonymousTraversalSource.Traversal();
    private readonly GremlinServer gremlinServer;
    private readonly ConnectionPoolSettings connectionPoolSettings = new()
    ***REMOVED***
        MaxInProcessPerConnection = 10,
        PoolSize = 30,
        ReconnectionAttempts = 3,
        ReconnectionBaseDelay = TimeSpan.FromMilliseconds(500)
***REMOVED***;
    public GremlinService(IConfiguration configs)
    ***REMOVED***
        var Primarykey = configs["CosmosDB:PrimaryKey"];
        var ContainerLink = configs["CosmosDB:ContainerLink"];
        var Host = configs["CosmosDB:Host"];
        var Port = int.Parse(configs["CosmosDB:Port"]);
        var EnableSSL = bool.Parse(configs["CosmosDB:EnableSSL"]);
        gremlinServer = new GremlinServer(Host, Port, enableSsl: EnableSSL, username: ContainerLink, password: Primarykey);
***REMOVED***

    public GremlinClient CreateClient()
    ***REMOVED***
        var webSocketConfiguration = new Action<ClientWebSocketOptions>(options =>
        ***REMOVED***
            options.KeepAliveInterval = TimeSpan.FromSeconds(10);
    ***REMOVED***);

        return new GremlinClient(gremlinServer, new GraphSON2MessageSerializer(), connectionPoolSettings, webSocketConfiguration);
***REMOVED***

    public GraphTraversalSource G => g;

    public string UpsertPersonQuery => "g.V().hasLabel('person').has('userId','***REMOVED***0***REMOVED***').fold().coalesce(unfold().property('name','***REMOVED***1***REMOVED***').property('email','***REMOVED***2***REMOVED***'), addV('person').property('userId','***REMOVED***0***REMOVED***').property('name','***REMOVED***1***REMOVED***').property('email','***REMOVED***2***REMOVED***').property('id','" + Guid.NewGuid() + "'))";
    public string AddFriendshipQuery => "g.V().hasLabel('person').has('userId','***REMOVED***0***REMOVED***').addE('friends').to(g.V().hasLabel('person').has('userId','***REMOVED***1***REMOVED***'))";
***REMOVED***

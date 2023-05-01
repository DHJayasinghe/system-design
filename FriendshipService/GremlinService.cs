using System;
using System.Net.WebSockets;
using Gremlin.Net.Driver;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Configuration;

namespace FriendshipService;

public class GremlinService
{
    private static GraphTraversalSource g = AnonymousTraversalSource.Traversal();
    private readonly GremlinServer gremlinServer;
    private readonly ConnectionPoolSettings connectionPoolSettings = new()
    {
        MaxInProcessPerConnection = 10,
        PoolSize = 30,
        ReconnectionAttempts = 3,
        ReconnectionBaseDelay = TimeSpan.FromMilliseconds(500)
    };
    public GremlinService(IConfiguration configs)
    {
        var Primarykey = configs["CosmosDB:PrimaryKey"];
        var ContainerLink = configs["CosmosDB:ContainerLink"];
        var Host = configs["CosmosDB:Host"];
        var Port = int.Parse(configs["CosmosDB:Port"]);
        var EnableSSL = bool.Parse(configs["CosmosDB:EnableSSL"]);
        gremlinServer = new GremlinServer(Host, Port, enableSsl: EnableSSL, username: ContainerLink, password: Primarykey);
    }

    public GremlinClient CreateClient()
    {
        var webSocketConfiguration = new Action<ClientWebSocketOptions>(options =>
        {
            options.KeepAliveInterval = TimeSpan.FromSeconds(10);
        });

        return new GremlinClient(gremlinServer, new GraphSON2MessageSerializer(), connectionPoolSettings, webSocketConfiguration);
    }

    public GraphTraversalSource G => g;
}

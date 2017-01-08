# Really simple competing consumers-style pub/sub messaging using RabbitMQ for .NET Core apps
![AppVeyor build](https://ci.appveyor.com/api/projects/status/github/neuralaxis/cocotte?branch=master&svg=true)


```csharp

var consumer = "sample"; // used as prefix for subscription queues. name of the application/daemon/microservice/thing is probably good.
var exchange = "events"; // the rabbitmq exchange

using (var coco = new Client(new Uri("amqp://localhost:5672/"), consumer, exchange))
{ 
    coco.Subscribe("client.created", (json) =>
    {
        Console.WriteLine($"received created: json {json}"); // { "ClientId": "created client" }
    });

    coco.Subscribe<ClientEvent>("client.updated", (msg) =>
    {
        Console.WriteLine($"received updated: object {msg.ClientId}");
    });

    coco.Subscribe<ClientEvent>("client.*", (msg) =>
    {
        Console.WriteLine($"received all: object {msg.ClientId}");
    });

    coco.Publish("client.created", new ClientEvent { ClientId = "created client" });
    coco.Publish("client.updated", new ClientEvent { ClientId = "updated client" });

    coco.Wait();
}
class ClientEvent
{
    public string ClientId;
}
```

# Really simple pub/sub messaging using RabbitMQ for .NET Core apps


```csharp

using (var coco = new Client(new Uri("amqp://localhost:5672/"), "pubsubsample", "events"))
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

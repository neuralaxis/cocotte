# Really simple pub/sub messaging using RabbitMQ for .NET Core apps


```csharp

        using (var coco = new Client(new Uri("amqp://localhost:5672/"), "pubsubsample", "events"))
        { 
            coco.Subscribe("client.created", (json) =>
            {
                Console.WriteLine($"received created: json {json}");
            });

            coco.Subscribe<Msg>("client.updated", (msg) =>
            {
                Console.WriteLine($"received updated: object {msg.ClientId}");
            });

            coco.Subscribe<Msg>("client.*", (msg) =>
            {
                Console.WriteLine($"received all: object {msg.ClientId}");
            });

            coco.Publish("client.created", new Msg { ClientId = "created client" });
            coco.Publish("client.updated", new Msg { ClientId = "updated client" });
            Console.WriteLine("messages sent");

            coco.Wait();
        }
```

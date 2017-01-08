using Cocotte;
using System;

class Program
{
    class Msg
    {
        public string ClientId;
    }
    static void Main(string[] args)
    {
        var mode = "both";
        if (args.Length > 0) mode = args[0];

        using (var coco = new Client(new Uri("amqp://localhost:5672/"), "events", "example"))
        {

            if (mode == "both" || mode == "sub")
            {
                coco.Subscribe("client.created", (json) =>
                {
                    Console.WriteLine($"delegate received created: json {json}");
                });
                coco.Subscribe<Msg>("client.updated", (msg) =>
                {
                    Console.WriteLine($"delegate received updated: object {msg.ClientId}");
                });
                coco.Subscribe<Msg>("client.*", (msg) =>
                {
                    Console.WriteLine($"delegate received all: object {msg.ClientId}");
                });
            }

            if (mode == "both" || mode == "pub")
            {
                coco.Publish("client.created", new Msg { ClientId = "created client" });
                coco.Publish("client.updated", new Msg { ClientId = "updated client" });
                Console.WriteLine("messages sent");
            }

        }
        Console.WriteLine("Any key to continue...");
        Console.ReadLine();
    }
    
}
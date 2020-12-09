using DataStuff;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Dynamic;
using System.Text;

namespace RabbitMQReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()  
            {
#if DEBUG
                HostName = "localhost",
#else
                HostName = "some-rabbit",
#endif
                UserName = ConnectionFactory.DefaultUser,  
                Password = ConnectionFactory.DefaultPass,  
                ClientProvidedName = "Seller",
                Port = AmqpTcpEndpoint.UseDefaultPort  
            };  
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "sell", type: ExchangeType.Fanout);

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "sell", routingKey: "");

            using var logChannel = connection.CreateModel();
            logChannel.ExchangeDeclare(exchange: "log", type: ExchangeType.Fanout);
            var logQueue = logChannel.QueueDeclare("SellerQ", true, false, false).QueueName;
            logChannel.QueueBind(logQueue, "log", "");

            Console.WriteLine($"Queue [{queueName}] is waiting for messages.");

            var repo = new Repository();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var split = message.Split(' ');
                if (split.Length != 3)
                {
                    Console.WriteLine($"\t[x] Message is in wrong format!");
                }
                else
                {
                    var ressource = new Ressource(split[1], int.Parse(split[2]));
                    repo.Insert(ressource);
                    logChannel.BasicPublish(exchange: "log", routingKey: "", basicProperties: null, body: Encoding.UTF8.GetBytes($"Handled {message}"));
                    Console.WriteLine($"\t[x] {message}");
                }
            };
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
            connection.Close();
        }
    }
}

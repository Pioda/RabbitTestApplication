using DataStuff;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQBuyer
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
                Port = AmqpTcpEndpoint.UseDefaultPort  
            };  
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "buy", type: ExchangeType.Fanout);

            using var logChannel = connection.CreateModel();
            logChannel.ExchangeDeclare(exchange: "log", type: ExchangeType.Fanout);
            var logQueue = logChannel.QueueDeclare().QueueName;
            logChannel.QueueBind(logQueue, "log", "");

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "buy", routingKey: "");

            Console.WriteLine($"Queue [{queueName}] is waiting for messages.");

            var repo = new Repository();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var split = message.Split(' ');
                if (split.Length != 2)
                {
                    Console.WriteLine($"\t[x] Message is in wrong format!");
                }
                else
                {
                    repo.RemoveRessource(split[1]);
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

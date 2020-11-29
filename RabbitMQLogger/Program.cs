using DataStuff;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQLogger
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
            using var logChannel = connection.CreateModel();
            logChannel.ExchangeDeclare(exchange: "log", type: ExchangeType.Fanout);

            var logQueue = logChannel.QueueDeclare().QueueName;
            logChannel.QueueBind(queue: logQueue, exchange: "log", routingKey: "");

            Console.WriteLine($"Queue [{logQueue}] is waiting for messages.");


            var consumerLog = new EventingBasicConsumer(logChannel);
            consumerLog.Received += HandleEvent;

            logChannel.BasicConsume(queue: logQueue, autoAck: true, consumer: consumerLog);

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
            connection.Close();
        }

        private static void HandleEvent(object sender, BasicDeliverEventArgs args)
        {
            var repo = new Repository();
            var message = Encoding.UTF8.GetString(args.Body.ToArray());
            repo.Insert(new Log(message));
            Console.WriteLine($"\t[x] {message}");
        }
    }
}

using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQSender
{
    class Program
    {
        private static Dictionary<string, IModel> commands = new Dictionary<string, IModel>();
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
            using var sellChannel = connection.CreateModel();
            using var buyChannel = connection.CreateModel();
            sellChannel.ExchangeDeclare(exchange: "sell", type: ExchangeType.Fanout);
            buyChannel.ExchangeDeclare(exchange: "buy", type: ExchangeType.Fanout);
            commands.Add("sell", sellChannel);
            commands.Add("buy", buyChannel);

            Console.WriteLine("Please type your message.");
            Console.WriteLine("Type 'exit' to exit or 'help' for the commands");

            while (true)
            {
                var message = Console.ReadLine().ToLower();
                if (message == "exit")
                {
                    connection.Close();
                    break;
                }
                var splittedMessage = message.Split(' ');
                if (splittedMessage.Length > 3)
                {
                    Console.WriteLine("Your message is to long, please apply to the message formats:\n");
                    DisplayHelp();
                    continue;
                }
                var channelName = splittedMessage[0];
                if (!commands.ContainsKey(channelName) || message == "help")
                {
                    DisplayHelp();
                    continue;
                }
                var body = Encoding.UTF8.GetBytes(message);
                var channelToSend = commands[channelName];
                channelToSend.BasicPublish(exchange: channelName,
                    routingKey: "",
                    basicProperties: null,
                    body: body);
                Console.WriteLine($"\t[x] Sent {message}");
            }
        }

        static void DisplayHelp()
        {
            Console.WriteLine();
            Console.WriteLine("------ Help ------");
            Console.WriteLine("possible command: 'sell' \nUsage: 'sell item_name price'");
            Console.WriteLine("possible command: 'buy' \nUsage: 'buy item_name'");
            Console.WriteLine("------ ---- ------");
            Console.WriteLine();
        }
    }
}

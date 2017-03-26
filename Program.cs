using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Tatweer.Traffic
{
    internal class Program
    {
        public static List<Message> Logs = new List<Message>();

        private static void Main(string[] args)
        {
            Console.WriteLine("Tatweer Smart Traffic System");
            Console.WriteLine("By Waleed Kasem");

            var countsThread = new Thread(LogCounts);
            countsThread.Start();
            Console.ReadLine();
        }

        private static void LogCounts()
        {
            while (true)
            {
                var connection = RabbitServer.GetConnection();
                var channel = RabbitServer.GetChannel(connection);

                ReceiveService.ReceiveMessages(channel, Enums.Exchange.Counts);

                var period = int.Parse(ConfigurationManager.AppSettings["period"]);
                Thread.Sleep(TimeSpan.FromMinutes(period));

                // for test
                //Thread.Sleep(TimeSpan.FromSeconds(5));

                channel.Close();
                connection.Close();

                var counts = ReceiveService.GetMessages();

                if (counts.Any())
                {
                    var sensorWithAccident = RouteAnalysis.GetAccidentProbability(counts);
                    if (sensorWithAccident != null)
                    {
                        var messageBody = MessageService.SerializeSensor(sensorWithAccident);
                        EmitService.EmitMessage(Enums.Exchange.Accidents, messageBody);
                        Console.WriteLine(string.Concat("Accident: ", messageBody));
                        Console.WriteLine("\n\n");
                    }
                }

                Debug.WriteLine("Logs Count is: " + counts.Count);
            }
        }
    }
}
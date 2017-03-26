using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Tatweer.Traffic
{
    public static class ReceiveService
    {
        private static List<Message> _logMessages = new List<Message>();
        private static readonly int DangerSpeed = int.Parse(ConfigurationManager.AppSettings["speed"]);

        public static void ReceiveMessages(IModel channel, Enums.Exchange exchange, bool isLogClear = true)
        {
            if (isLogClear)
                _logMessages = new List<Message>();

            channel.BasicQos(0, 1, false);

            var eventingBasicConsumer = new EventingBasicConsumer(channel);

            eventingBasicConsumer.Received += (sender, delivery) =>
            {
                var messages = MessageService.DeserializeMessages(delivery.Body);
                foreach (var message in messages)
                {
                    Debug.WriteLine(MessageService.SerializeMessages(message));

                    if (message.Speed > DangerSpeed)
                    {
                        EmitService.EmitMessage(Enums.Exchange.Alerts, MessageService.SerializeMessages(message));

                        Console.WriteLine(string.Concat("Alerts: ", MessageService.SerializeMessages(message)));
                        Console.WriteLine("\n\n");
                    }

                    _logMessages.Add(message);
                }

                channel.BasicAck(delivery.DeliveryTag, false);
            };

            channel.BasicConsume(exchange.ToString(), false, eventingBasicConsumer);
        }

        public static List<Message> GetMessages()
        {
            return _logMessages;
        }
    }
}
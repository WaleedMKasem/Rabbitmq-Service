using System.Text;
using RabbitMQ.Client;

namespace Tatweer.Traffic
{
    public static class EmitService
    {
        public static void EmitMessage(Enums.Exchange exchange, string message)
        {
            using (var connection = RabbitServer.GetConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange.ToString().ToLower(), ExchangeType.Fanout);

                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange.ToString(), "", null, body);
                    //Debug.WriteLine(" [x] Sent {0}", message);
                }
            }
        }
    }
}
using System.Configuration;
using RabbitMQ.Client;

namespace Tatweer.Traffic
{
    public static class RabbitServer
    {
        public static ConnectionFactory CreateFactory()
        {
            var settings = ConfigurationManager.AppSettings;
            var hostName = settings["amqpUri"];
            var port = int.Parse(settings["port"]);
            var user = settings["user"];
            var pass = settings["pass"];

            return new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = user,
                Password = pass
            };
        }

        public static IConnection GetConnection()
        {
            var factory = CreateFactory();
            return factory.CreateConnection();
        }

        public static IModel GetChannel(IConnection connection)
        {
            return connection.CreateModel();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Tatweer.Traffic
{
    public static class MessageService
    {
        public static IEnumerable<Message> DeserializeMessages(byte[] content)
        {
            var messages = Encoding.UTF8.GetString(content);

            try
            {
                return JsonConvert.DeserializeObject<IList<Message>>(messages);
            }
            catch (Exception)
            {
                return Enumerable.Empty<Message>();
            }
        }

        public static string SerializeMessages(Message message)
        {
            try
            {
                return JsonConvert.SerializeObject(message);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static string SerializeSensor(SensorStatistics Sensor)
        {
            try
            {
                return JsonConvert.SerializeObject(Sensor);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
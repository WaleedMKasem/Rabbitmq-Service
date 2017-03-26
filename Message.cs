using System;

namespace Tatweer.Traffic
{
    public class Message
    {
        public int Id { get; set; }
        public float Location_ID { get; set; }
        public string PS_ID { get; set; }
        public DateTime DateTime { get; set; }
        public int Log_ID { get; set; }
        public string VehicleType { get; set; }
        public int LaneNo { get; set; }
        public string Violation { get; set; }
        public float Speed { get; set; }
        public int Direction { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Tatweer.Traffic
{
    public static class RouteAnalysis
    {
        public static List<SensorStatistics> GetSensorStatistics(List<Message> counts)
        {
            var sensorStatistics = new List<SensorStatistics>();
            var sensors = counts.Select(c => c.PS_ID).Distinct();
            foreach (var sensor in sensors)
            {
                var sensorCounts = counts.Where(c => c.PS_ID == sensor);
                float density = sensorCounts.Count();
                var totalSpeed = sensorCounts.Sum(c => c.Speed);

                float avgSpeed = 0;
                if (density != 0 && totalSpeed != 0)
                    avgSpeed = totalSpeed / density;

                var sensorStatistic = new SensorStatistics
                {
                    SendorId = sensor,
                    Density = density,
                    AvgSpeed = avgSpeed
                };
                sensorStatistics.Add(sensorStatistic);
            }
            return sensorStatistics;
        }

        public static SensorStatistics GetAccidentProbability(List<Message> counts)
        {
            var sensorStatistics = GetSensorStatistics(counts);

            if (sensorStatistics.Any())
            {
                var normSpeed = sensorStatistics.Average(s => s.AvgSpeed);
                var normDensity = sensorStatistics.Average(s => s.Density);

                var minSpeedSensor = sensorStatistics.OrderBy(s => s.AvgSpeed).FirstOrDefault();
                var minDensitySensor = sensorStatistics.OrderBy(s => s.Density).FirstOrDefault();

                if (minSpeedSensor.SendorId == minDensitySensor.SendorId)
                    if (minSpeedSensor.AvgSpeed < normSpeed / 2 && minSpeedSensor.Density < normDensity / 2)
                        return minSpeedSensor;
            }
            return null;
        }
    }
}
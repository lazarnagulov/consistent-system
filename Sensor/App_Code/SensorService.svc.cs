using System;
using System.Collections.Generic;
using System.ServiceModel;
using ConsistentSystem.Common.Models;
using ConsistentSystem.Contracts;
using ConsistentSystem.Sensor.Core;

namespace ConsistentSystem.Sensor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SensorService : ISensorService
    {
        private readonly Dictionary<string, SensorWorker> _sensors = new Dictionary<string, SensorWorker>();
        private readonly object _alignLock = new object();


        public SensorService()
        {
            _sensors = new Dictionary<string, SensorWorker>();

            for (int i = 1; i <= 3; i++)
            {
                string sensorId = "Sensor" + i;
                var repository = new SensorRepository(sensorId + ".db");
                var worker = new SensorWorker(repository, sensorId);
                worker.Start();
                _sensors[sensorId] = worker;
            }
        }

        public void Align(double value)
        {
            lock (_alignLock)
            {
                foreach (var worker in _sensors.Values)
                {
                    worker.Align(value);
                }
                Console.WriteLine("All sensors aligned to: " + value);
            }
        }

        public Measurement GetLastMeasurement(string sensorId)
        {
            SensorWorker worker;
            if (_sensors.TryGetValue(sensorId, out worker))
            {
                return worker.GetLastMeasurement();
            }

            throw new ArgumentException("Sensor with id " + sensorId + " not found.");
        }

        public string GetSensorName(string sensorId)
        {
            if (_sensors.ContainsKey(sensorId))
                return sensorId;

            throw new ArgumentException("Sensor with id " + sensorId + " not found.");
        }
    }

}

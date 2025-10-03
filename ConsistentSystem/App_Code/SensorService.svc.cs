using System;
using System.Collections.Generic;
using System.ServiceModel;
using ConsistentSystem.Common.Models;
using ConsistentSystem.Contracts;
using ConsistentSystem.Sensor.Core;

namespace ConsistentSystem.Sensor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SensorService : ISensorService
    {
        private readonly Dictionary<string, SensorWorker> _sensors = new Dictionary<string, SensorWorker>();
        private readonly object _alignLock = new object();
        private volatile bool _isAligning = false;

        public SensorService()
        {
            _sensors = new Dictionary<string, SensorWorker>();

            for (int i = 1; i <= 3; i++)
            {
                string sensorId = "Sensor" + i;
                var repository = new SensorRepository(sensorId);
                var worker = new SensorWorker(repository);
                worker.Start();
                _sensors[sensorId] = worker;
            }
        }

        public void Align(double value)
        {
            lock (_alignLock)
            {
                _isAligning = true;
                var callback = OperationContext.Current.GetCallbackChannel<ISensorCallback>();
                callback.OnAlignmentStarted();

                foreach (var worker in _sensors.Values)
                {
                    worker.Align(value);
                }

                System.Threading.Thread.Sleep(3000);

                callback.OnAlignmentCompleted(value);
                _isAligning = false;
            }
        }

        public Measurement GetLastMeasurement(string sensorId)
        {
            if (_isAligning)
            {
                return new Measurement
                {
                    Id = -1,
                    Temperature = double.NaN, 
                    Timestamp = DateTime.UtcNow
                };
            }
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

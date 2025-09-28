using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ConsistentSystem.Common.Models;
using ConsistentSystem.Contracts;
using ConsistentSystem.Sensor.Core;

namespace ConsistentSystem.Sensor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SensorService : ISensorService
    {
        private readonly SensorWorker _worker;
        private readonly object _alignLock = new object();

        public SensorService(SensorWorker worker)
        {
            _worker = worker;
        }

        public void Align(double value)
        {
            lock (_alignLock)
            {
                _worker.Align(value);
                Console.WriteLine("Sensor aligned to: " + value);
            }
        }

        public Measurement GetLastMeasurement()
        {
            return _worker.GetLastMeasurement();
        }

        public string GetSensorName()
        {
            return _worker.Name;
        }
    }

}

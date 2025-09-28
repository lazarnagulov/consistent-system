using ConsistentSystem.Common.Models;
using ConsistentSystem.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sensor
{
    public class SensorService : ISensorService
    {
        private readonly SensorRepository _repository;
        private readonly string _name;

        public SensorService(SensorRepository repository, string name)
        {
            _repository = repository;
            _name = name;
        }

        public void Align(double value) => _repository.InsertMeasurement(value);
        
        public Measurement GetLastMeasurement() => _repository.GetLastMeasurement();

        public string GetSensorName() => _name;
    }
}

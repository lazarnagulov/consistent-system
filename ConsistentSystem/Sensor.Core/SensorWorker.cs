using ConsistentSystem.Common.Models;
using System;
using System.Timers;
using System.Xml.Linq;

namespace ConsistentSystem.Sensor.Core
{
    public class SensorWorker
    {
        private readonly SensorRepository _repository;
        public SensorRepository Repository => _repository; 
        private readonly Timer _timer;
        private readonly Random _rand = new Random();
        private readonly object _lock = new object(); 
        public String Name { get; }

        public SensorWorker(SensorRepository repository, string name)
        {
            _repository = repository;
            _timer = new Timer();
            _timer.Elapsed += GenerateMeasurement;
            Name = name;
        }

        public void Start() => ScheduleNext();

        private void ScheduleNext()
        {
            _timer.Interval = _rand.Next(1000, 10000);
            _timer.Start();
        }

        private void GenerateMeasurement(object sender, ElapsedEventArgs e)
        {
            lock (_lock)
            {
                _timer.Stop();
                double value = 15 + _rand.NextDouble() * 15;
                _repository.InsertMeasurement(value);
                Console.WriteLine($"{Name} measured {value}");
                ScheduleNext();
            }
        }

        public void Align(double value)
        {
            lock (_lock)
            {
                _repository.InsertMeasurement(value);
                Console.WriteLine($"{Name} aligned to: {value}");
            }
        }

        public Measurement GetLastMeasurement()
        {
            lock (_lock)
            {
                return _repository.GetLastMeasurement();
            }
        }
    }

}

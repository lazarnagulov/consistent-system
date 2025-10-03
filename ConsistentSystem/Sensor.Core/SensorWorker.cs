using ConsistentSystem.Common.Models;
using System;
using System.Timers;

namespace ConsistentSystem.Sensor.Core
{
    public class SensorWorker
    {
        private readonly SensorRepository _repository;
        private readonly Timer _timer;
        private readonly Random _rand = new Random();
        private static readonly Random _globalRand = new Random();
        private readonly object _lock = new object(); 

        public SensorWorker(SensorRepository repository)
        {
            _repository = repository;

            lock (_globalRand)
            {
                _rand = new Random(_globalRand.Next());
            }

            _timer = new Timer();
            _timer.Elapsed += GenerateMeasurement;
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
                double value = 10 + _rand.NextDouble() * 40;
                _repository.InsertMeasurement(value);
                ScheduleNext();
            }
        }

        public void Align(double value)
        {
            lock (_lock)
            {
                _timer.Stop();
                _repository.InsertMeasurement(value);
                ScheduleNext();     
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

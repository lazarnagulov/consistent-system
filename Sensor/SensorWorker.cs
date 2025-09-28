using System;
using System.Timers;

namespace Sensor
{
    public class SensorWorker
    {
        private readonly SensorRepository _repository;
        private readonly Timer _timer;
        private readonly Random _rand = new Random();

        public SensorWorker(SensorRepository repository)
        {
            _repository = repository;
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
            _timer.Stop();
            double value = 15 + _rand.NextDouble() * 15; 
            _repository.InsertMeasurement(value);
            Console.WriteLine($"Sensor measured: {value}");
            ScheduleNext();
        }

    }
}

using SensorAligner.ServiceReference;
using System;
using System.Linq;
using System.ServiceModel;
using System.Timers;

namespace SensorAligner
{
    internal class Program
    {

        static ISensorService proxy;
        static Timer alignmentTimer;
        static string[] sensorIds = { "Sensor1", "Sensor2", "Sensor3" };

        static void Main(string[] args)
        {

            var callback = new SensorCallbackHandler();
            var context = new InstanceContext(callback);

            var factory = new DuplexChannelFactory<ISensorService>(
                context, "WSDualHttpBinding_ISensorService");

            proxy = factory.CreateChannel();

            Console.WriteLine("SensorAligner started.");

            alignmentTimer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds); // one minut
            alignmentTimer.Elapsed += OnTimedAlignment;
            alignmentTimer.AutoReset = true;
            alignmentTimer.Enabled = true;

            Console.ReadLine();
        }

        private static void OnTimedAlignment(object sender, ElapsedEventArgs e)
        {
            try
            {
                var measurements = sensorIds
                    .Select(id => proxy.GetLastMeasurement(id))
                    .ToList();

                double avg = measurements.Average(m => m.Temperature);

                proxy.Align(avg);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[{DateTime.Now}] Error during alignment: " + ex.Message);
            }
        }

        public class SensorCallbackHandler : ISensorServiceCallback
        {
            public void OnAlignmentCompleted(double alignedValue)
            {
                Console.WriteLine($"[{DateTime.Now}] Alignment completed: {alignedValue:F2} °C");
            }

            public void OnAlignmentStarted()
            {
                Console.WriteLine($"[{DateTime.Now}] Alignment started...");
            }
        }
    }
}

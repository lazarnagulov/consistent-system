using Client.ServiceReference;
using ConsistentSystem.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{

    public class SensorClientCallback : ISensorServiceCallback
    {
        public void OnAlignmentStarted()
        {
            Console.WriteLine($"[{DateTime.Now}] Alignment started. Please wait...");
        }

        public void OnAlignmentCompleted(double alignedValue)
        {
            Console.WriteLine($"[{DateTime.Now}] Alignment completed. All sensors now at {alignedValue:F2} °C");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var callback = new SensorClientCallback();
            var context = new InstanceContext(callback);

            var factory = new DuplexChannelFactory<ISensorService>(
                context, "WSDualHttpBinding_ISensorService");

            var proxy = factory.CreateChannel();

            var sensorIds = new List<string> { "Sensor1", "Sensor2", "Sensor3" };

            Console.WriteLine("Client started. Press Ctrl+C to exit.");

            while (true)
            {
                try
                {
                    var measurements = sensorIds.Select(id => proxy.GetLastMeasurement(id)).ToList();

                    foreach (var m in measurements)
                        Console.WriteLine($"    Sensor: {m.Id}, Temp: {m.Temperature:F2} °C at {m.Timestamp}");

                    double avg = measurements.Average(m => m.Temperature);

                    var validSensors = measurements
                        .Where(m => Math.Abs(m.Temperature - avg) <= 5)
                        .ToList();

                    if (validSensors.Count >= 2)
                    {
                        Console.WriteLine($"[{DateTime.Now}] Valid measurements found (avg={avg:F2} °C):");
                    }
                    else
                    {
                        proxy.Align(avg);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"[{DateTime.Now}] Error: " + ex.Message);
                }

                Console.ReadKey();
            }
        }
    }
}

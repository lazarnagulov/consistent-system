using ConsistentSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: Sensor.exe <sensorId>");
                return;
            }
            string dbPath = $"sensor{args[0]}.db";
            var repo = new SensorRepository(dbPath);
            var worker = new SensorWorker(repo);
            worker.Start();

            Console.ReadKey();
        }
    }
}

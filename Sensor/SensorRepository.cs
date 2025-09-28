using ConsistentSystem.Common;
using ConsistentSystem.Common.Models;

namespace Sensor
{
    public class SensorRepository
    {
        private readonly string _dbPath;

        public SensorRepository(string dbPath)
        {
            _dbPath = dbPath;
            DataBaseHelper.CreateConnection(dbPath);
        }

        public void InsertMeasurement(double temperature)
        {
        }

        public Measurement GetLastMeasurement()
        {
            return null;
        }

    }
}

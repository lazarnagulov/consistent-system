using ConsistentSystem.Common;
using ConsistentSystem.Common.Models;
using System;
using System.Data.SQLite;
using System.IO;
using System.Xml.Linq;

namespace ConsistentSystem.Sensor.Core
{
    public class SensorRepository
    {
        private readonly string _dbPath;
        private string ConnectionString => $"Data Source={_dbPath};";
        private const string InsertMeasurementSql = "INSERT INTO Measurements (Temperature) VALUES (@temperature)";
        private const string SelectLastMeasurementSql = "SELECT Id, Temperature, Timestamp FROM Measurements ORDER BY Timestamp DESC LIMIT 1";


        public SensorRepository(string dbPath)
        {
            if (string.IsNullOrWhiteSpace(dbPath))
                throw new ArgumentException("Database path cannot be null or empty.", nameof(dbPath));

            string appDataPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            _dbPath = Path.Combine(appDataPath, dbPath + ".db");
            DataBaseHelper.CreateConnection(_dbPath);
        }

        public void InsertMeasurement(double temperature)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(InsertMeasurementSql, connection))
                {
                    command.Parameters.AddWithValue("@temperature", temperature);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Measurement GetLastMeasurement()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(SelectLastMeasurementSql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Measurement
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Temperature = Convert.ToDouble(reader["Temperature"]),
                                Timestamp = Convert.ToDateTime(reader["Timestamp"])
                            };
                        }
                    }
                }
            }

            return null;
        }

    }
}

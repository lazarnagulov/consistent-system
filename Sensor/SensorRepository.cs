using ConsistentSystem.Common;
using ConsistentSystem.Common.Models;
using Microsoft.Data.Sqlite;
using System;

namespace Sensor
{
    public class SensorRepository
    {
        private readonly string _dbPath;
        private string ConnectionString => $"Data Source={_dbPath};Version=3;";
        private const string InsertMeasurementSql = "INSERT INTO Measurements (Temperature) VALUES (@temperature)";
        private const string SelectLastMeasurementSql = "SELECT Id, Temperature, Timestamp FROM Measurements ORDER BY Timestamp DESC LIMIT 1";


        public SensorRepository(string dbPath)
        {
            if (string.IsNullOrWhiteSpace(dbPath))
                throw new ArgumentException("Database path cannot be null or empty.", nameof(dbPath));

            _dbPath = dbPath;
            DataBaseHelper.CreateConnection(dbPath);
        }

        public void InsertMeasurement(double temperature)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(InsertMeasurementSql, connection))
                {
                    command.Parameters.AddWithValue("@temperature", temperature);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Measurement GetLastMeasurement()
        {
            using (var connection = new SqliteConnection($"Data Source={_dbPath};Version=3;"))
            {
                connection.Open();
                using (var command = new SqliteCommand(SelectLastMeasurementSql, connection))
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

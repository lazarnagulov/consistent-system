using System;
using System.Data.SQLite;
using System.IO;

namespace ConsistentSystem.Common
{
    public static class DataBaseHelper
    {

        public static SQLiteConnection GetConnection(string dbPath)
        {
            return new SQLiteConnection($"Data Source={dbPath};Version=3;");
        }

        public static void CreateConnection(string dbPath)
        {
            string directory = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(dbPath))
                return;

            SQLiteConnection.CreateFile(dbPath);
            using (var connection = GetConnection(dbPath))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Measurements (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Temperature REAL NOT NULL,
                            Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
                        );";
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
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
            if (!File.Exists(dbPath))
                SQLiteConnection.CreateFile(dbPath);
        }

    }
}
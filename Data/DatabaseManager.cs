using Microsoft.Data.Sqlite;

using System;
using System.IO;

namespace Viadivy.Tools.VyCapture.Data
{
    public class DatabaseManager
    {
        public string GetDatabasePath()
        {
            string localAppDataPath =
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData);

            string applicationFolder =
                Path.Combine(
                    localAppDataPath,
                    "Viadivy",
                    "VyCapture");

            if (!Directory.Exists(applicationFolder))
            {
                Directory.CreateDirectory(
                    applicationFolder);
            }

            string databasePath =
                Path.Combine(
                    applicationFolder,
                    "VyCapture.db");

            return databasePath;
        }


        public string GetConnectionString()
        {
            string databasePath =
                GetDatabasePath();

            string connectionString =
                "Data Source=" +
                databasePath;

            return connectionString;
        }


        public void InitializeDatabase()
        {
            string connectionString =
                GetConnectionString();

            using (SqliteConnection connection =
                new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql =
       @"
CREATE TABLE IF NOT EXISTS Captures
(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NULL,
    Content TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS DeletedCaptures
(
    DeletedId INTEGER PRIMARY KEY AUTOINCREMENT,
    OriginalId INTEGER NOT NULL,
    Title TEXT NULL,
    Content TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NOT NULL,
    DeletedAt TEXT NOT NULL
);
";

                using (SqliteCommand command =
                    new SqliteCommand(
                        sql,
                        connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
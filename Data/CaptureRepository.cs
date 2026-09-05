using Microsoft.Data.Sqlite;

using Viadivy.Tools.VyCapture.Models;

using System;
using System.Collections.Generic;

namespace Viadivy.Tools.VyCapture.Data
{
    public class CaptureRepository
    {
        private readonly DatabaseManager _databaseManager;

        public CaptureRepository(
            DatabaseManager databaseManager)
        {
            _databaseManager =
                databaseManager;
        }


        public CaptureItem Insert(
           string content)
        {
            string connectionString =
                _databaseManager
                    .GetConnectionString();


            using (SqliteConnection connection =
                new SqliteConnection(
                    connectionString))
            {
                connection.Open();


                string sql =
                    @"
INSERT INTO Captures
(
    Title,
    Content,
    CreatedAt,
    UpdatedAt
)
VALUES
(
    @Title,
    @Content,
    @CreatedAt,
    @UpdatedAt
);

SELECT last_insert_rowid();
";


                using (SqliteCommand command =
                    new SqliteCommand(
                        sql,
                        connection))
                {
                    DateTime now =
                        DateTime.Now;


                    command.Parameters.AddWithValue(
                        "@Title",
                        DBNull.Value);

                    command.Parameters.AddWithValue(
                        "@Content",
                        content);

                    command.Parameters.AddWithValue(
                        "@CreatedAt",
                        now.ToString(
                            "yyyy-MM-dd HH:mm:ss"));

                    command.Parameters.AddWithValue(
                        "@UpdatedAt",
                        now.ToString(
                            "yyyy-MM-dd HH:mm:ss"));


                    object? result =
                        command.ExecuteScalar();


                    long id =
                        Convert.ToInt64(
                            result);


                    CaptureItem item =
                        new CaptureItem();

                    item.Id =
                        id;

                    item.Title =
                        null;

                    item.Content =
                        content;

                    item.CreatedAt =
                        now;

                    item.UpdatedAt =
                        now;


                    return item;
                }
            }
        }


        public List<CaptureItem> GetAll()
        {
            List<CaptureItem> items =
                new List<CaptureItem>();


            string connectionString =
                _databaseManager
                    .GetConnectionString();


            using (SqliteConnection connection =
                new SqliteConnection(
                    connectionString))
            {
                connection.Open();


                string sql =
                    @"
SELECT
    Id,
    Title,
    Content,
    CreatedAt,
    UpdatedAt
FROM Captures
ORDER BY Id DESC;
";


                using (SqliteCommand command =
                    new SqliteCommand(
                        sql,
                        connection))
                {
                    using (SqliteDataReader reader =
                        command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CaptureItem item =
                                new CaptureItem();


                            item.Id =
                                reader.GetInt64(0);


                            if (reader.IsDBNull(1))
                            {
                                item.Title =
                                    null;
                            }
                            else
                            {
                                item.Title =
                                    reader.GetString(1);
                            }


                            item.Content =
                                reader.GetString(2);


                            string createdAtText =
                                reader.GetString(3);

                            item.CreatedAt =
                                DateTime.Parse(
                                    createdAtText);


                            string updatedAtText =
                                reader.GetString(4);

                            item.UpdatedAt =
                                DateTime.Parse(
                                    updatedAtText);


                            items.Add(
                                item);
                        }
                    }
                }
            }


            return items;
        }

        public bool Delete(
      long id)
        {
            string connectionString =
                _databaseManager
                    .GetConnectionString();


            using (SqliteConnection connection =
                new SqliteConnection(
                    connectionString))
            {
                connection.Open();


                using (SqliteTransaction transaction =
                    connection.BeginTransaction())
                {
                    try
                    {
                        //
                        // Step 1
                        // 先將原資料備份到 DeletedCaptures。
                        //
                        string backupSql =
                            @"
INSERT INTO DeletedCaptures
(
    OriginalId,
    Title,
    Content,
    CreatedAt,
    UpdatedAt,
    DeletedAt
)
SELECT
    Id,
    Title,
    Content,
    CreatedAt,
    UpdatedAt,
    @DeletedAt
FROM Captures
WHERE Id = @Id;
";


                        using (SqliteCommand backupCommand =
                            new SqliteCommand(
                                backupSql,
                                connection,
                                transaction))
                        {
                            backupCommand.Parameters.AddWithValue(
                                "@Id",
                                id);

                            backupCommand.Parameters.AddWithValue(
                                "@DeletedAt",
                                DateTime.Now.ToString(
                                    "yyyy-MM-dd HH:mm:ss"));


                            int backupRows =
                                backupCommand.ExecuteNonQuery();


                            if (backupRows != 1)
                            {
                                transaction.Rollback();

                                return false;
                            }
                        }


                        //
                        // Step 2
                        // 備份成功後，才刪除 Captures 原資料。
                        //
                        string deleteSql =
                            @"
DELETE FROM Captures
WHERE Id = @Id;
";


                        using (SqliteCommand deleteCommand =
                            new SqliteCommand(
                                deleteSql,
                                connection,
                                transaction))
                        {
                            deleteCommand.Parameters.AddWithValue(
                                "@Id",
                                id);


                            int deletedRows =
                                deleteCommand.ExecuteNonQuery();


                            if (deletedRows != 1)
                            {
                                transaction.Rollback();

                                return false;
                            }
                        }


                        //
                        // Step 3
                        // 備份與刪除都成功，正式 Commit。
                        //
                        transaction.Commit();

                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }
    }
}
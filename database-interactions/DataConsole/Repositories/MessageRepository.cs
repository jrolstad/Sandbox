using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace DataConsole.Repositories
{
    public class MessageRepository
    {
        public void Configure()
        {
            using var connection = new SqliteConnection("Data Source=hello.db");
            connection.Open();

            var existingCommand = connection.CreateCommand();
            existingCommand.CommandText = "SELECT name FROM sqlite_master WHERE name='message'";
            var existing = existingCommand.ExecuteScalar();

            if (existing!=null) return;

            var createCommand = connection.CreateCommand();
            createCommand.CommandText = @"CREATE TABLE message (messagetext VARCHAR(100))";
            createCommand.ExecuteNonQuery();

            connection.Close();
        }

        public void Save(string message)
        {
            using var connection = new SqliteConnection("Data Source=hello.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "insert into message (messagetext) values ($newMessage)";
            command.Parameters.AddWithValue("$newMessage", message);
            command.ExecuteNonQuery();

            connection.Close();
        }

        public List<string> Get()
        {
            using var connection = new SqliteConnection("Data Source=hello.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "select messagetext from message";

            using var reader = command.ExecuteReader();

            var result = new List<string>();
            while (reader.Read())
            {
                var message = reader.GetString(0);
                result.Add(message);
            }

            connection.Close();

            return result;
        }
    }
}

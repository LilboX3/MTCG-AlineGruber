using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;

namespace MTCG.Data_Layer
{
    public class DatabaseConnection
    {
        private static string _connectionString = "Host=localhost;Database=postgresdb;Username=admin;Password=istrator;Persist Security Info=True;Include Error Detail=True";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
            //dann connection.Open()! und new NpgsqlCommand(Command, connection);
            // command.AddWithValue("column", object.property);
            // var affectedRows = command.ExecuteNonQuery();

        }
    }
}

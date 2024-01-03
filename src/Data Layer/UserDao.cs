using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Data_Layer
{
    public class UserDao
    {
        private const string InsertUserCommand = @"INSERT INTO ""User""(username, password, elo, wins, losses) VALUES (@username, @password, @elo, @wins, @losses)";
        private const string SelectUserByNameCommand = @"SELECT userid FROM ""User"" WHERE username = @username";
        private const string DeleteUserByNameCommand = @"DELETE FROM ""User"" WHERE username = @username";
        private const string SelectUserByCredentialsCommand = @"SELECT userid FROM ""User"" WHERE username = @username AND password = @password";
        private const string SelectUserByTokenCommand = @"SELECT userid FROM ""User"" WHERE token = @token";

        public bool InsertUser(Models.User user)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            string username = user.UserCredentials.Username;
            string password = user.UserCredentials.Password;

            int elo = user.EloValue;
            int wins = user.Wins;
            int losses = user.Losses;
            var affectedRows = 0;

            //Check if user already exists
            if (!GetUserByName(username))
            {
                using var cmd = new NpgsqlCommand(InsertUserCommand, connection);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                cmd.Parameters.AddWithValue("elo", elo);
                cmd.Parameters.AddWithValue("wins", wins);
                cmd.Parameters.AddWithValue("losses", losses);
                affectedRows = cmd.ExecuteNonQuery();
            }

            return affectedRows > 0;
            //False wenn User registrieren nicht funktioniert hat
        }

        public bool GetUserByName(string username)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectUserByNameCommand, connection);
            cmd.Parameters.AddWithValue("username", username);
            using var reader = cmd.ExecuteReader();

            return reader.Read();
            //if no rows: user doesnt exist yet!
        }

        public bool DeleteUser(string username)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
         
            using var cmd = new NpgsqlCommand(DeleteUserByNameCommand, connection);
            cmd.Parameters.AddWithValue("username", username);
            var affectedRows = cmd.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public bool LoginUser(Models.Credentials credentials)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectUserByCredentialsCommand, connection);
            string username = credentials.Username;
            string password = credentials.Password;

            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            using var reader = cmd.ExecuteReader();
            //TODO: Insert token to database, or return 
            return reader.Read();
        }

        public bool TokenExists(string token)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectUserByTokenCommand, connection);
            cmd.Parameters.AddWithValue("token", token);
            using var reader = cmd.ExecuteReader();

            return reader.Read();
        }

        private string GenerateToken(string username)
        {
            return $"{username}-mtcgToken";
        }
        
        private bool InsertToken(string username, string token)
        {
            return false;
        }
    }
}

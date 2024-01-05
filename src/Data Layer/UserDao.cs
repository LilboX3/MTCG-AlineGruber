using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.Data_Layer
{
    public class UserDao
    {
        private const string InsertUserCommand = @"INSERT INTO ""User""(username, password, elo, wins, losses) VALUES (@username, @password, @elo, @wins, @losses)";
        private const string SelectUserIdByNameCommand = @"SELECT userid FROM ""User"" WHERE username = @username";
        private const string DeleteUserByNameCommand = @"DELETE FROM ""User"" WHERE username = @username";
        private const string SelectUserByCredentialsCommand = @"SELECT userid FROM ""User"" WHERE username = @username AND password = @password";
        private const string SelectUserByTokenCommand = @"SELECT userid FROM ""User"" WHERE token = @token";
        private const string SelectUserDataByUserIdCommand = @"SELECT * FROM ""UserData"" WHERE userid = @userid";
        private const string InsertUserDataCommand = @"INSERT INTO ""UserData"" (userid, name, bio, image) VALUES (@userid, @name, @bio, @image)";
        private const string InsertTokenByNameCommand = @"UPDATE ""User"" SET token = @token WHERE username = @username";

        public bool InsertUser(User user)
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

            using var cmd = new NpgsqlCommand(SelectUserIdByNameCommand, connection);
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

        public string? LoginUser(Models.Credentials credentials)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectUserByCredentialsCommand, connection);
            string username = credentials.Username;
            string password = credentials.Password;

            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            using var reader = cmd.ExecuteReader();
            
            if (reader.Read())
            {
                string token = GenerateToken(username);
                if (!InsertToken(username, token))
                {
                    throw new ArgumentException("Cannot insert token to database"); 
                }
                return token;
            }
            return null;
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

        public UserData? GetUserData(string username)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            //get userid to get userdata
            int userId = GetUserId(username);
            if (userId == -1)
            {
                //User not found
                return null;
            }

            //Now get userData
            using var cmd2 = new NpgsqlCommand(SelectUserDataByUserIdCommand, connection);
            cmd2.Parameters.AddWithValue("userid", userId);
            using var reader2 = cmd2.ExecuteReader();

            if (reader2.Read())
            {
                string? name = reader2["name"].ToString();
                string? bio = reader2["bio"].ToString();
                string? image = reader2["image"].ToString();
                UserData userData = new UserData(name, bio, image);
                return userData;
            }
            else
            {
                return null;
            }
        }
        public bool UpdateUserData(string username, UserData userData)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            //get userid to get userdata
            int userId = GetUserId(username);
            if (userId == -1)
            {
                //User not found
                return false;
            }
            if () //already exists, only update values
            {
               
            }
            using var cmd = new NpgsqlCommand(InsertUserDataCommand, connection);
            cmd.Parameters.AddWithValue("userid", userId);
            cmd.Parameters.AddWithValue("name", userData.Name);
            cmd.Parameters.AddWithValue("bio", userData.Bio);
            cmd.Parameters.AddWithValue("image", userData.Image);
            var affectedRows = cmd.ExecuteNonQuery();

            return affectedRows > 0;

        }

        private int GetUserId(string username)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            using var cmd = new NpgsqlCommand(SelectUserIdByNameCommand, connection);
            cmd.Parameters.AddWithValue("username", username);
            using var reader = cmd.ExecuteReader();
            int userId;

            if (reader.Read())
            {
                userId = reader.GetInt32(reader.GetOrdinal("userid"));
                return userId;
            }
            //User not found
            return -1;

        }

        private string GenerateToken(string username)
        {
            return $"{username}-mtcgToken";
        }
        
        private bool InsertToken(string username, string token)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            using var cmd = new NpgsqlCommand(InsertTokenByNameCommand, connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue ("token", token);
            var affectedRows = cmd.ExecuteNonQuery();

            return affectedRows > 0;
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;
using Npgsql;

namespace MTCG.Data_Layer
{
    public class ScoreboardDao
    {
        private const string GetUsersOrderedByElo = @"SELECT * FROM ""User"" ORDER BY elo DESC";

        public UserStats[] GetScoreboard()
        {
            List<UserStats> scoreboard = new List<UserStats>();
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            using var cmd = new NpgsqlCommand(GetUsersOrderedByElo, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(reader.GetOrdinal("username"));
                int elo = reader.GetInt32(reader.GetOrdinal("elo"));
                int wins = reader.GetInt32(reader.GetOrdinal("wins"));
                int losses = reader.GetInt32(reader.GetOrdinal("losses"));
                UserStats newStats = new UserStats(name, elo, wins, losses);
                scoreboard.Add(newStats);
            }

            return scoreboard.ToArray();
        }
    }
}

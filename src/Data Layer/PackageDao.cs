using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;
using Npgsql;

namespace MTCG.Data_Layer
{
    public class PackageDao
    {
        private const string InsertPackageCommand = @"INSERT INTO ""Package""(card1id, card2id, card3id, card4id, card5id) VALUES (@card1id, @card2id, @card3id, @card4id, @card5id)";
        private const string GetAvailablePackageCommand = @"SELECT * FROM ""Package"" limit 1";
        private const string GetUserIdByNameCommand = @"SELECT userid FROM ""User"" WHERE username = @username";
        private const string UpdateCardUserIdCommand = @"UPDATE ""Card"" SET userid = @userid WHERE cardid = @cardid";
        private const string UpdateUserCoinsCommand = @"UPDATE ""User"" SET coins = coins-5 WHERE userid = @userid";
        private const string GenerateCardByIdCommand = @"SELECT * FROM ""Card"" WHERE cardid = @cardid";
        private const string DeletePackageByIdCommand = @"DELETE FROM ""Package"" WHERE packageid = @packageid";
        
        public bool InsertPackage(Package package)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            string card1id = package.Cards[0].Id;
            string card2id = package.Cards[1].Id;
            string card3id = package.Cards[2].Id;
            string card4id = package.Cards[3].Id;
            string card5id = package.Cards[4].Id;
            
            Console.Write("CARD IDs ARE:\n"+card1id+"\n"+card2id+"\n"+card3id+"\n"+card4id+"\n");

            using var cmd = new NpgsqlCommand(InsertPackageCommand, connection);
            cmd.Parameters.AddWithValue("card1id", card1id);
            cmd.Parameters.AddWithValue("card2id", card2id);
            cmd.Parameters.AddWithValue("card3id", card3id);
            cmd.Parameters.AddWithValue("card4id", card4id);
            cmd.Parameters.AddWithValue("card5id", card5id);
            var affectedRows = cmd.ExecuteNonQuery();
            
            return affectedRows > 0;
        }

        public Card[]? BuyAndDeletePackage(User user)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand(GetAvailablePackageCommand, connection);
            var reader = cmd.ExecuteReader();
            Card[] cards = new Card[5];
            if (reader.Read())
            {
                int userid = GetUserId(user.UserCredentials.Username);
                if (userid == -1)
                {
                    return null;
                }
                
                if (!SpendUserCoins(userid))
                {
                    return null;
                }

                for (int i = 0; i < 5; i++)
                {
                    int cardidIndex = i + 1;
                    string cardid = reader.GetString(reader.GetOrdinal("card" + cardidIndex + "id"));
                    cards[i] = GenerateCard(cardid);
                    if (!AssignAllCardsToUser(reader, userid))
                    {
                        Console.WriteLine("COULDNT ASSIGN ALL CARDS TO USER");
                        return null;
                    }
                }
                //now delete the bought package
                int packageid = reader.GetInt32(reader.GetOrdinal("packageid"));
                if (!DeletePackage(packageid))
                {
                    Console.WriteLine("COULDNT DELETE PACKAGE!");
                    return null;
                }

                return cards;
            }
            
            return null;
        }

        private bool AssignAllCardsToUser(NpgsqlDataReader reader, int userid)
        {
            string card1id = reader.GetString(reader.GetOrdinal("card1id"));
            string card2id = reader.GetString(reader.GetOrdinal("card2id"));
            string card3id = reader.GetString(reader.GetOrdinal("card3id"));
            string card4id = reader.GetString(reader.GetOrdinal("card4id"));
            string card5id = reader.GetString(reader.GetOrdinal("card5id"));
            if (!AssignCardToUser(card1id, userid))
            {
                return false;
            }
            if (!AssignCardToUser(card2id, userid))
            {
                return false;
            }
            if (!AssignCardToUser(card3id, userid))
            {
                return false;
            }
            if (!AssignCardToUser(card4id, userid))
            {
                return false;
            }
            if (!AssignCardToUser(card5id, userid))
            {
                return false;
            }

            return true;

        }

        private bool DeletePackage(int packageid)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            using var cmd = new NpgsqlCommand(DeletePackageByIdCommand, connection);
            cmd.Parameters.AddWithValue("packageid", packageid);
            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        private Card? GenerateCard(string cardid)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            
            using var cmd = new NpgsqlCommand(GenerateCardByIdCommand, connection);
            cmd.Parameters.AddWithValue("cardid", cardid);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                double damage = reader.GetDouble(reader.GetOrdinal("damage"));
                Element element;
                string elementString = reader.GetString(reader.GetOrdinal("element"));
                Enum.TryParse<Element>(elementString, out element);
                    
                int monsterTypeOrdinal = reader.GetOrdinal("monstertype");
                string? monsterType = reader.IsDBNull(monsterTypeOrdinal) ? null : reader.GetString(monsterTypeOrdinal);
                if (monsterType == null)
                {
                    Monster monster;
                    Enum.TryParse<Monster>(monsterType, out monster);
                    MonsterCard monsterCard = new MonsterCard(monster, (float)damage, cardid, element);
                    return monsterCard;
                }
                
                SpellCard spellCard = new SpellCard((float)damage, cardid, element);
                return spellCard;
                
            }

            return null;
        }

        private bool SpendUserCoins(int userid)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            
            using var cmd = new NpgsqlCommand(UpdateUserCoinsCommand, connection);
            cmd.Parameters.AddWithValue("userid", userid);
            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }
        private int GetUserId(string username)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand(GetUserIdByNameCommand, connection);
            cmd.Parameters.AddWithValue("username", username);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int userid = reader.GetInt32(reader.GetOrdinal("userid"));
                return userid;
            }

            return -1;
        }

        private bool AssignCardToUser(string cardid, int userid)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand(UpdateCardUserIdCommand, connection);

            cmd.Parameters.AddWithValue("userid", userid);
            cmd.Parameters.AddWithValue("cardid", cardid);  
            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}

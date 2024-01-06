using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;
using Npgsql;

namespace MTCG.Data_Layer
{
    public class DeckDao
    {
        private const string GetUserIdByTokenCommand = @"SELECT userid FROM ""User"" WHERE token = @token";
        private const string GetDeckByUserIdCommand = @"SELECT * FROM ""Deck"" WHERE ownerid = @ownerid";
        private const string GetCardByIdCommand = @"SELECT * FROM ""Card"" WHERE cardid = @cardid";

        private const string CreateNewDeckCommand =
            @"INSERT INTO ""Deck""(card1id, card2id, card3id, card4id, ownerid) VALUES (@card1id, @card2id, @card3id, @card4id, @ownerid)";
        public Card[]? GetUserCardsByToken(string token)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            int ownerid = GetUserIdByToken(token);
            if (ownerid == -1)
            {
                return null;
            }
            List<Card> cards = new List<Card>();
            using var cmd = new NpgsqlCommand(GetDeckByUserIdCommand, connection);
            cmd.Parameters.AddWithValue("ownerid", ownerid);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string card1id = reader.GetString(reader.GetOrdinal("card1id"));
                string card2id = reader.GetString(reader.GetOrdinal("card2id"));
                string card3id = reader.GetString(reader.GetOrdinal("card3id"));
                string card4id = reader.GetString(reader.GetOrdinal("card4id"));
                cards.Add(GenerateCard(card1id));
                cards.Add(GenerateCard(card2id));
                cards.Add(GenerateCard(card3id));
                cards.Add(GenerateCard(card4id));
                return cards.ToArray();
            }
            else
            {
                //no deck found
                return null;
            }
        }

        public bool CreateUserDeck(string token, string[] cardIds)
        {
            int userid = GetUserIdByToken(token);
            foreach (var cardId in cardIds)
            {
                if (!CardBelongsToUser(cardId, userid))
                {
                    Console.WriteLine("Card doesnt belong to user");
                    return false;
                }
            }
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            
            using var cmd = new NpgsqlCommand(CreateNewDeckCommand, connection);
            cmd.Parameters.AddWithValue("card1id", cardIds[0]);
            cmd.Parameters.AddWithValue("card2id", cardIds[1]);
            cmd.Parameters.AddWithValue("card3id", cardIds[2]);
            cmd.Parameters.AddWithValue("card4id", cardIds[3]);
            cmd.Parameters.AddWithValue("ownerid", userid);
            var affectedRows = cmd.ExecuteNonQuery();
            
            return affectedRows > 0;
        }

        private bool CardBelongsToUser(string cardid, int userid)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            using var cmd = new NpgsqlCommand(GetCardByIdCommand, connection);
            cmd.Parameters.AddWithValue("cardid", cardid);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int actualUserId = reader.GetInt32(reader.GetOrdinal("userid"));
                return actualUserId == userid;
            }

            return false;
        }
        
        private Card GenerateCard(string cardid)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            
            using var cmd = new NpgsqlCommand(GetCardByIdCommand, connection);
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
                if (monsterType != null)
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
        
        private int GetUserIdByToken(string token)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand(GetUserIdByTokenCommand, connection);
            cmd.Parameters.AddWithValue("token", token);
            using var reader = cmd.ExecuteReader();
            
            if (reader.Read())
            {
                int userid = reader.GetInt32(reader.GetOrdinal("userid"));
                return userid;
            }

            return -1;
        }
    }
}

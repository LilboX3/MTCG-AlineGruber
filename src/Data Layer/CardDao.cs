using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;
using Npgsql;

namespace MTCG.Data_Layer
{
    public class CardDao
    {
        private const string InsertMonsterCardCommand = @"INSERT INTO ""Card""(cardid, name, damage, element, monstertype) VALUES (@cardid, @name, @damage, @element, @monstertype)";
        private const string InsertCardCommand = @"INSERT INTO ""Card""(cardid, name, damage, element) VALUES (@cardid, @name, @damage, @element)";
        private const string SelectCardByIdCommand = @"SELECT * FROM ""Card"" WHERE cardid = @cardid";
        private const string GetUserIdByTokenCommand = @"SELECT userid FROM ""User"" WHERE token = @token";
        private const string GetCardsOfUserCommand = @"SELECT * FROM ""Card"" WHERE userid = @userid;";
        public bool InsertCard(Card card)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            if (CardAlreadyExists(card))
            {
                return false;
            }

            string cardid = card.Id;
            string name = card.Name;
            float damage = card.Damage;
            string element = card.ElementType.ToString();
            if (card is MonsterCard)
            {
                using var cmd = new NpgsqlCommand(InsertMonsterCardCommand, connection);
                var monsterCard = card as MonsterCard;
                string monstertype = monsterCard.MonsterType.ToString();
                cmd.Parameters.AddWithValue("cardid", cardid);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("damage", damage);
                cmd.Parameters.AddWithValue("element", element);
                cmd.Parameters.AddWithValue("monstertype", monstertype);
                var affectedRows = cmd.ExecuteNonQuery();
            
                return affectedRows > 0;
            }
            else
            {
                using var cmd = new NpgsqlCommand(InsertCardCommand, connection);
                cmd.Parameters.AddWithValue("cardid", cardid);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("damage", damage);
                cmd.Parameters.AddWithValue("element", element);
                var affectedRows = cmd.ExecuteNonQuery();
            
                return affectedRows > 0;
            }
            
        }

        public Card[]? GetUserCardsByToken(string token)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            int userid = GetUserIdByToken(token);
            if (userid == -1)
            {
                Console.WriteLine("Userid is null");
                return null;
            }
            
            List<Card> cards = new List<Card>();
            using var cmd = new NpgsqlCommand(GetCardsOfUserCommand, connection);
            cmd.Parameters.AddWithValue("userid", userid);

            var reader = cmd.ExecuteReader();
            int index = 0;

            while (reader.Read())
            {
                Console.WriteLine("reading user cards");
                string cardid = reader.GetString(reader.GetOrdinal("cardid"));
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
                    cards.Add(monsterCard);
                }
                else
                {
                    SpellCard spellCard = new SpellCard((float)damage, cardid, element);
                    cards.Add(spellCard);
                }

                index++;

            }
            if (index==0)
            {
                //no cards belong to user
                Console.WriteLine("user "+userid+" has no cards");
                return null;
            }
            
            return cards.ToArray();
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

        private bool CardAlreadyExists(Card card)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            string cardid = card.Id;
            
            using var cmd = new NpgsqlCommand(SelectCardByIdCommand, connection);
            cmd.Parameters.AddWithValue("cardid", cardid);
            using var reader = cmd.ExecuteReader();
            return reader.HasRows;
        }
    }
}

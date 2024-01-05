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

        public bool CardAlreadyExists(Card card)
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

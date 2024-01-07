using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;
using Npgsql;
using NpgsqlTypes;

namespace MTCG.Data_Layer
{
    public class TradeDao
    {
        private const string GetAllTradesCommand = @"SELECT * FROM ""Trade""";
        private const string GetUserIdByTokenCommand = @"SELECT userid FROM ""User"" WHERE token = @token";
        private const string GetCardUserIdCommand = @"SELECT userid FROM ""Card"" WHERE cardid = @cardId";
        private const string GetDeckCardCommand = @"SELECT * FROM ""Deck"" WHERE card1id = @cardId OR card2id = @cardId OR card3id = @cardId OR card4id = @cardId";
        private const string SelectTradeByIdCommand = @"SELECT * FROM ""Trade"" WHERE tradeid = @tradeid";
        private const string InsertTradeCommand =
            @"INSERT INTO ""Trade""(tradeid, type, minimumdamage, cardtotradeid, userid) VALUES (@tradeid, @type, @minimumdamage, @cardtotradeid, @userid)";
        private const string DeleteTradeByIdCommand = @"DELETE FROM ""Trade"" WHERE tradeid = @tradeid";
        private const string GetCardIdCommand = @"SELECT cardtotradeid FROM ""Trade"" WHERE tradeid = @tradeid";
        private const string GetCardByCardIdCommand = @"SELECT * FROM ""Card"" WHERE cardid = @cardid";
        private const string SwapCardUserIdToCommand = @"UPDATE ""Card"" SET userid = @userid WHERE cardid = @cardid";
        
        public TradingDeal[]? GetAllTrades()
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();

            using var cmd = new NpgsqlCommand(GetAllTradesCommand, connection);
            using var reader = cmd.ExecuteReader();
            int index = 0;
            List<TradingDeal> trades = new List<TradingDeal>();
            while (reader.Read())
            {
                string tradeId = reader.GetString(reader.GetOrdinal("tradeid"));
                string type = reader.GetString(reader.GetOrdinal("type"));
                double minDamage = reader.GetDouble(reader.GetOrdinal("minimumdamage"));
                string cardId = reader.GetString(reader.GetOrdinal("cardtotradeid"));
                int userId = reader.GetInt32(reader.GetOrdinal("userid"));
                TradingDeal newTrade = new TradingDeal(tradeId, type, minDamage, cardId, userId);
                trades.Add(newTrade);
                index++;
            }
            //confirm no trades available, didnt work with if !reader.Read()
            if (index == 0)
            {
                return null;
            }

            return trades.ToArray();
        }

        public bool InsertTrade(string token, TradingDealFactory trade)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            int userId = GetUserIdByToken(token);
            if (userId == -1)
            {
                return false;
            }

            if (TradeIdExists(trade.Id))
            {
                return false;
            }
            
            using var cmd = new NpgsqlCommand(InsertTradeCommand, connection);
            cmd.Parameters.AddWithValue("tradeid",trade.Id);
            cmd.Parameters.AddWithValue("type", trade.Type);
            cmd.Parameters.AddWithValue("minimumdamage", trade.MinimumDamage);
            cmd.Parameters.AddWithValue("cardtotradeid", trade.CardToTrade);
            cmd.Parameters.AddWithValue("userid", userId);
            var affectedRows = cmd.ExecuteNonQuery();
            
            return affectedRows > 0;
        }

        public bool TradeIdExists(string tradeId)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            
            using var cmd = new NpgsqlCommand(SelectTradeByIdCommand, connection);
            cmd.Parameters.AddWithValue("tradeid", tradeId);
            using var reader = cmd.ExecuteReader();

            return reader.Read();
        }

        public bool CardNotOwnedOrInDeck(string token, string cardId)
        {
            int userId = GetUserIdByToken(token);
            if (userId == -1)
            {
                return false;
            }
            if (CardOwnedByOther(userId, cardId))
            {
                return true;
            }

            if (CardInDeck(cardId))
            {
                return true;
            }

            return false;
        }

        public bool TradeCardBelongsToToken(string token, string tradeId)
        {
            //See if card belongs to user so they can their delete card
            int userid = GetUserIdByToken(token);
            string cardid = GetCardId(tradeId);
            if (cardid == "")
            {
                //No trade exists, so "null" card does belong to token
                return true;
            }
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand(GetCardUserIdCommand, connection);
            cmd.Parameters.AddWithValue("cardid", cardid);
            using var reader = cmd.ExecuteReader();
            int actualUserId = 0;
            if (reader.Read())
            {
                actualUserId = reader.GetInt32(reader.GetOrdinal("userid"));
            }
            return userid == actualUserId;
        }

        private String GetCardId(string tradeId)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand(GetCardIdCommand, connection);
            cmd.Parameters.AddWithValue("tradeid", tradeId);
            using var reader = cmd.ExecuteReader();
            string cardId ="";
            if (reader.Read())
            {
                cardId = reader.GetString(reader.GetOrdinal("cardtotradeid"));
            }
            
            return cardId;
        }

        public bool DeleteTrade(string tradeId)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            
            using var cmd = new NpgsqlCommand(DeleteTradeByIdCommand, connection);
            cmd.Parameters.AddWithValue("tradeid", tradeId);
            var affectedRows = cmd.ExecuteNonQuery();
            
            return affectedRows > 0;
        }

        public bool CardBelongsToOwner(string token, string offeredCardId)
        {
            int userId = GetUserIdByToken(token);
            return !CardOwnedByOther(userId, offeredCardId);
        }

        public bool RequirementsMet(string cardId, string tradeId)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            
            double cardDamage = 0;
            string cardType = "";
            using(var cmd = new NpgsqlCommand(GetCardByCardIdCommand, connection))
            {
                Console.WriteLine("CARDID: " + cardId);
                //cardid wurde mit " übernommen
                string cleanedCardId = cardId.Replace("\"", "");
                cmd.Parameters.AddWithValue("cardid", cleanedCardId);

                using var reader = cmd.ExecuteReader();
                
                if (reader.Read())
                {
                    Console.WriteLine("Getting card stats");
                    //TODO: fix getting card stats
                    cardDamage = reader.GetDouble(reader.GetOrdinal("damage"));
                    if (!reader.IsDBNull(reader.GetOrdinal("monstertype")))
                    {
                        cardType = "monster";
                    }
                    else
                    {
                        cardType = "spell";
                    }
                
                }
                else
                {
                    Console.WriteLine("No data found for cardID: " + cardId);
                }
            }
            double tradeDamage = 0;
            string tradeType = "";
            using (var cmd2 = new NpgsqlCommand(SelectTradeByIdCommand, connection))
            {
                cmd2.Parameters.AddWithValue("tradeid", tradeId);
                using var reader = cmd2.ExecuteReader();
                if (reader.Read())
                {
                    tradeDamage = reader.GetDouble(reader.GetOrdinal("minimumdamage"));
                    tradeType = reader.GetString(reader.GetOrdinal("type"));
                }
            }
            Console.WriteLine("Card: "+cardType+" "+cardDamage+" Trade:"+tradeType+" "+tradeDamage);
            return DecideRequirements(tradeDamage, cardDamage, tradeType, cardType);
        }

        private bool DecideRequirements(double minDamage, double cardDamage, string tradeType, string cardType)
        {
            tradeType = tradeType.Trim();
            return (cardDamage >= minDamage) && (tradeType.Equals(cardType));
        }

        public bool TradeCardWith(string tradeId, string cardId, string token)
        {
            cardId = cardId.Replace("\"", "");
            int cardUserId = GetUserIdByToken(token);
            int tradeUserId = GetTradeUserId(tradeId);
            string tradeCardId = GetCardId(tradeId);

            if (!SwapCardUserId(tradeUserId, cardId) ||
                !SwapCardUserId(cardUserId, tradeCardId))
            {
                return false;
            }

            return DeleteTrade(tradeId);
            //TODO: switch userids
            //TODO: delete trade
        }

        private bool SwapCardUserId(int newUserId, string cardId)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand(SwapCardUserIdToCommand, connection);
            cmd.Parameters.AddWithValue("cardid", cardId);
            cmd.Parameters.AddWithValue("userid", newUserId);
            var affectedRows = cmd.ExecuteNonQuery();
            
            return affectedRows > 0; 
        }

        private int GetTradeUserId(string tradeId)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand(SelectTradeByIdCommand, connection);
            cmd.Parameters.AddWithValue("tradeid", tradeId);
            using var reader = cmd.ExecuteReader();
            int userid = 0;
            if (reader.Read())
            {
                userid = reader.GetInt32(reader.GetOrdinal("userid"));
            }

            return userid;
        }

        public bool CardOwnedByOther(int userid, string cardId)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand(GetCardUserIdCommand, connection);
            cmd.Parameters.AddWithValue("cardId", cardId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int? ownerId = reader.IsDBNull(reader.GetOrdinal("userid")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("userid"));
                //return has other owner
                Console.WriteLine(cardId);
                Console.WriteLine("GIVEN ID:"+userid+" ACTUAL ID:"+ownerId);
                return ownerId != userid;
                //false if owner is user
            }  

            return false;
        }

        public bool CardInDeck(string cardId)
        {
            using NpgsqlConnection connection = DatabaseConnection.GetConnection();
            connection.Open();
            using var cmd = new NpgsqlCommand(GetDeckCardCommand, connection);

            cmd.Parameters.AddWithValue("cardId", cardId);
            using var reader = cmd.ExecuteReader();
            return reader.Read();
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

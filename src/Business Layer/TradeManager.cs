using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Data_Layer;
using MTCG.HttpServer;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.Business_Layer
{
    public class TradeManager
    {
        private readonly TradeDao _tradeDao;

        public TradeManager(TradeDao tradeDao)
        {
            _tradeDao = tradeDao;
        }

        public HttpResponse GetAvailableTrades(Dictionary<string, string> headers)
        {
            if (!headers.ContainsKey("Authorization:"))
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized error");
            }

            string token = GetToken(headers);
            TradingDeal[]? trades = _tradeDao.GetAllTrades();
            if (trades == null)
            {
                return new HttpResponse(StatusCode.NoContent, "No Trading deals available");
            }

            return new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(trades));
        }

        public HttpResponse CreateTrade(Dictionary<string, string> headers, TradingDealFactory trade)
        {
            if (!headers.ContainsKey("Authorization:"))
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized error");
            }
            string token = GetToken(headers);
            if (_tradeDao.CardNotOwnedOrInDeck(token, trade.CardToTrade))
            {
                return new HttpResponse(StatusCode.Forbidden,
                    "The deal contains a card that is not owned by the user or locked in the deck");
            }

            if (!_tradeDao.InsertTrade(token, trade))
            {
                return new HttpResponse(StatusCode.Conflict, "A deal with this deal ID already exists.");
            }

            return new HttpResponse(StatusCode.Created, "Trading deal successfully created");
        }

        public HttpResponse DeleteDeal(string tradeId, Dictionary<string, string> headers)
        {
            if (!headers.ContainsKey("Authorization:"))
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized error");
            }
            string token = GetToken(headers);
            if (!_tradeDao.TradeCardBelongsToToken(token, tradeId))
            {
                return new HttpResponse(StatusCode.Forbidden,
                    "The deal to be deleted contains a card that is not owned by the user.");
            }

            if (!_tradeDao.DeleteTrade(tradeId))
            {
                return new HttpResponse(StatusCode.NotFound, "The provided deal ID was not found.");
            }
            return new HttpResponse(StatusCode.Ok, "Trading deal successfully deleted");
        }
        
        public HttpResponse MakeDeal(string tradeId, Dictionary<string, string> headers, string offeredCardId)
        {
            if (!headers.ContainsKey("Authorization:"))
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized error");
            }
            string token = GetToken(headers);
            if (!_tradeDao.TradeIdExists(tradeId))
            {
                return new HttpResponse(StatusCode.NotFound, "The provided deal ID was not found.");
            }
            //TODO: check if requirements meet
            if (!_tradeDao.CardBelongsToOwner(token, offeredCardId) || _tradeDao.CardInDeck(offeredCardId)
                                                                    || _tradeDao.TradeCardBelongsToToken(token, tradeId)
                                                                    || !_tradeDao.RequirementsMet(offeredCardId, tradeId)) 
            {
                return new HttpResponse(StatusCode.Forbidden,
                    "The offered card is not owned by the user, or the requirements are not met (Type, MinimumDamage), " +
                    "or the offered card is locked in the deck, or the user tries to trade with self");
            }
            //TODO: make trade: assign new owner ids to cards, delete trade
            if (!_tradeDao.TradeCardWith(tradeId, offeredCardId, token))
            {
                return new HttpResponse(StatusCode.InternalServerError, "Error making trade");
            }

            return new HttpResponse(StatusCode.Ok, "Trading deal successfully executed.");
        }
        
        private string GetToken(Dictionary<string, string> headers)
        {
            string getToken = headers["Authorization:"];
            string[] tokens = getToken.Split(' ');
            string token = tokens[1].Trim();
            return token;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MTCG.Data_Layer;
using MTCG.HttpServer;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.Business_Layer
{
    public class DeckManager
    {
        private readonly DeckDao _deckDao;

        public DeckManager(DeckDao deckDao)
        {
            _deckDao = deckDao;
        }

        public HttpResponse GetUserDeck(Dictionary<string, string> headers)
        {
            if (!headers.ContainsKey("Authorization:"))
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized error");
            }

            string token = GetToken(headers);
            Card[]? cards = _deckDao.GetUserCardsByToken(token);
            if (cards == null)
            {
                return new HttpResponse(StatusCode.NoContent, "Deck does not contain any cards");
            }

            return new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(cards));
        }
        
        public HttpResponse GetUserDeckText(Dictionary<string, string> headers)
        {
            if (!headers.ContainsKey("Authorization:"))
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized error");
            }

            string token = GetToken(headers);
            Card[]? cards = _deckDao.GetUserCardsByToken(token);
            if (cards == null)
            {
                return new HttpResponse(StatusCode.NoContent, "Deck does not contain any cards");
            }

            return new HttpResponse(StatusCode.Ok, DeckDescription(cards));
        }

        public HttpResponse CreateDeck(Dictionary<string, string> headers, string[] cardIds)
        {
            if (!headers.ContainsKey("Authorization:"))
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized error");
            }
            if (cardIds.Length != 4)
            {
                return new HttpResponse(StatusCode.BadRequest,
                    "The provided deck did not include the required amount of cards");
            }
            string token = GetToken(headers);
            if (!_deckDao.CreateUserDeck(token, cardIds))
            {
                return new HttpResponse(StatusCode.Forbidden,
                    "At least one of the provided cards does not belong to the user or is not available.");
            }

            return new HttpResponse(StatusCode.Ok, "The deck has been successfully configured");

        }

        private string DeckDescription(Card[] cards)
        {
            string description = "Cards in deck: \n";
            foreach (var card in cards)
            {
                description += card.ToString() + "\n";
            }

            return description;
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

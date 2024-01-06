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
    public class CardManager
    {
        private readonly CardDao _cardDao;

        public CardManager(CardDao cardDao)
        {
            _cardDao = cardDao;
        }
        
        //GET http://localhost:10001/cards --header "Authorization: Bearer altenhof-mtcgToken"
        public HttpResponse GetUserCards(Dictionary<string, string> headers) //needs token of user
        {
            if (!headers.ContainsKey("Authorization:"))
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized error");
            }
            string token = GetToken(headers);
            Card[]? userCards = _cardDao.GetUserCardsByToken(token);
            
            if (userCards == null)
            {
                
                return new HttpResponse(StatusCode.NoContent, "User does not have any cards");
            }
            
            return new HttpResponse(StatusCode.Ok, "Cards:\n"+JsonConvert.SerializeObject(userCards));
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

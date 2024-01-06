using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MTCG.Data_Layer;
using MTCG.HttpServer;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.Business_Layer
{
    public class PackageManager
    {
        private readonly PackageDao _packageDao;
        private readonly CardDao _cardDao;
        private readonly UserDao _userDao;

        public PackageManager(PackageDao packageDao, CardDao cardDao, UserDao userDao)
        {
            _packageDao = packageDao;
            _cardDao = cardDao;
            _userDao = userDao;
        }

        public HttpResponse CreatePackage(CardFactory[] cardFactories, Dictionary<string, string> headers)
        {
            if (!headers.ContainsKey("Authorization:"))
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized error");
            }
            if (!IsAdminToken(headers))
            {
                return new HttpResponse(StatusCode.Forbidden, "Provided user is not 'admin'");
            }

            if (cardFactories.Length != 5)
            {
                return new HttpResponse(StatusCode.BadRequest, "Not enough or too many cards in package");
            }
            
            //for each card in cardfactory: transform to new card
            var cards = cardFactories.Select(f => f.Create()).ToArray();
            Package package = new Package();
            int i = 0;
            foreach(var card in cards)
            {
                if (!_cardDao.InsertCard(card))
                {
                    return new HttpResponse(StatusCode.Conflict, "At least one card in the packages already exists");
                }

                package.Cards[i] = card;
                Console.WriteLine(card.ToString()+"CARD ID IS: "+card.Id);
                i++;
            }

            if (!_packageDao.InsertPackage(package))
            {
                return new HttpResponse(StatusCode.InternalServerError, "Error inserting package");
            }
                
            return new HttpResponse(StatusCode.Created, "Package and cards successfully created");
        }

        public HttpResponse BuyPackage(Dictionary<string, string> headers)
        {
            if (!headers.ContainsKey("Authorization:"))
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized error");
            }
            string getToken = headers["Authorization:"];
            string[] tokens = getToken.Split(' ');
            string token = tokens[1].Trim();
            User? user = _userDao.GetUserObjectByToken(token);
            if (user == null)
            {
                return new HttpResponse(StatusCode.Unauthorized, "Could not find user to buy package");
            }
            if (user.Coins < 5)
            {
                return new HttpResponse(StatusCode.Forbidden, "Not enough money for buying a card package");
            }

            Card[]? cards = _packageDao.BuyAndDeletePackage(user);
            if (cards==null)
            {
                return new HttpResponse(StatusCode.NotFound, "No card package available for buying");
            }

            return new HttpResponse(StatusCode.Ok, "Cards:\n"+JsonConvert.SerializeObject(cards));

        }

        private bool IsAdminToken(Dictionary<string, string> headers)
        {
            string getToken = headers["Authorization:"];
            string[] tokens = getToken.Split(' ');
            string token = tokens[1];
            token = token.Trim();
            return token.Equals("admin-mtcgToken");
        }
    }
}

using MTCG.Business_Layer;
using MTCG.HttpServer;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MTCG.Business_Layer;
using MTCG.Routing;

namespace MTCG
{
    public class Router
    {
        /* ROUTES
        POST /users register a new user

        *TOKEN* GET /users/{username} retrieve user data for username, only ADMIN
        *TOKEN* PUT /users/{username} Updates user data for given username, only ADMIN or matching user, retrieves data

        POST /sessions login with existing user 

        *TOKEN* POST /packages create new card packages requires ADMIN array of cards

        *TOKEN* POST /transactions/packages Acquire a card package with user money, needs enough money & available package as card array

        *TOKEN* GET /cards show a users cards that he owns in response as array

        *TOKEN* GET /deck users current deck as array, string textual deck description
        *TOKEN* PUT /deck configure deck with 4 provided cards, failed request doesnt change deck

        *TOKEN* GET /stats retrieves stats for the requesting user (Name, elo, wins, losses)

        *TOKEN* GET /scoreboard retrieve user scoreboard ordered by the users elo as array CREATE SCOREBOARD TABLE ORDERED BY ELO
        UPDATE SCOREBOARD AFTER BATTLE

        *TOKEN* POST /battles enters lobby to start a battle, if another user in lobby, battle starts immediately, or it waits
        success respond with battle log

        *TOKEN* GET /tradings retrieves all available trading deaks
        *TOKEN* POST /tradings creates a new deal
        *TOKEN* DELETE /tradings/{tradingdealid} deletes existing trading deal, only by owner of card
        *TOKEN* POST /tradings/{tradingdealid} carry out deal with provided card, with self not allowed

        ----> Missing or invalid token: 401 Unauthorized Error
         */
        private BattleManager _battleManager;
        private CardManager _cardManager;
        private TradeManager _tradeManager;
        private UserManager _userManager;
        private PackageManager _packageManager;
        private ScoreboardManager _scoreboardManager;
        private DeckManager _deckManager;
        private IdRouteParser _routeParser;

        public Router(BattleManager battleManager, CardManager cardManager, TradeManager tradeManager, UserManager userManager, PackageManager packageManager, ScoreboardManager scoreboardManager, DeckManager deckManager, IdRouteParser idRouteParser)
        {
            _battleManager = battleManager;
            _cardManager = cardManager;
            _tradeManager = tradeManager;
            _userManager = userManager;
            _packageManager = packageManager;
            _scoreboardManager = scoreboardManager;
            _deckManager = deckManager;
            _routeParser = idRouteParser;
        }
        public HttpResponse? Resolve(HttpRequest request)
        {
            HttpResponse? response = null;
            var isUsernameMatch = (string path) => _routeParser.IsMatch(path, "/users/{username}");
            var isTradeIdMatch = (string path) => _routeParser.IsMatch(path, "/tradings/{tradingdealid}");
            var parseUser = (string path) => string.Parse(_routeParser.ParseParameters(path, "/users/{username}")["username"]);
            var parseTradeId = (string path) => string.Parse(_routeParser.ParseParameters(path, "/tradings/{tradingdealid}")["tradingdealid"]);

            response = request switch
            {
                { Method: "POST", Path: "/users" } => _userManager.RegisterUser(JsonConvert.DeserializeObject<Credentials>(request.Payload)),
                { Method: "GET", Path: } => _userManager.GetUserData(), //Show UserData Name, Bio, Image
                { Method: "PUT", Path: } => _userManager.UpdateUserData(), //Update Name, Bio, Image

                { Method: "POST", Path: "/sessions"} => _userManager.LoginUser(), //sends token back

                { Method: "POST", Path: "/packages"} => _packageManager.CreatePackage(), //requires admin, 5 cards, card must be unique!!!!!

                { Method: "POST", Path: "/transactions/packages"} => _packageManager.BuyPackage(), //user buys package (token), then remove from db? array response

                { Method: "GET", Path: "/cards"} => _cardManager.GetUserCards(), //show token users acquired cards, as array

                { Method: "GET", Path: "/deck"} => _deckManager.GetUserDeck(), //response array of card deck, textual deck description
                { Method: "PUT", Path: "/deck"} => _deckManager.CreateDeck(), //Four card ids to create new deck as array, response just status

                { Method: "GET", Path: "/stats"} => _userManager.GetUserStats(), //Get UserStats: Name, Elo, Wins, Losses

                { Method: "GET", Path: "/scoreboard"} => _scoreboardManager.GetScoreboard(), //als array, geordnet nach elo

                { Method: "POST", Path: "/battles"} => _battleManager.FindGame(), //enter lobby to start battle, wait for other user, return battle log

                { Method: "GET", Path: "/tradings"} => _tradeManager.GetAvailableTrades(), // retrieve all available deals, as array
                { Method: "POST", Path: "/tradings"} => _tradeManager.CreateTrade(),//create new deal, only for card you own, no response payload
                { Method: "DELTE", Path: } => _tradeManager.DeleteDeal(), //Delete an existing deal, only by owner (id in path)
                { Method: "POST", Path: } => _tradeManager.MakeDeal(), //carry out deal, request has tradeid and card id, no payload response, must be card owner, meet requirements
                _ => new HttpResponse(StatusCode.NotImplemented)

            };

            return response;
        }

    }
}

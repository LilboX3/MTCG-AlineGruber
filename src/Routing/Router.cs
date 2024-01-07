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
            Console.WriteLine("CURRENTLY IN ROUTER");
            HttpResponse? response = null;
            var isUsernameMatch = (string path) => _routeParser.IsMatch(path, "/users/{id}");
            var isTradeIdMatch = (string path) => _routeParser.IsMatch(path, "/tradings/{id}");
            var isFormatPlainMatch = (string path) => _routeParser.IsFormatPlainMatch(path, "/deck");
            
            var parseUser = (string path) => _routeParser.ParseParameters(path, "/users/{id}")["id"];
            var parseTradeId = (string path) => _routeParser.ParseParameters(path, "/tradings/{id}")["id"];

            response = request switch
            {
                { Method: "POST", Path: "/users" } => _userManager.RegisterUser(Deserialize<Credentials>(request.Payload)),
                { Method: "GET", Path: var path } when isUsernameMatch(path) => _userManager.GetUserData(parseUser(path), request.Headers), //Show UserData Name, Bio, Image
                { Method: "PUT", Path: var path } when isUsernameMatch(path) => _userManager.UpdateUserData(parseUser(path), request.Headers, Deserialize<UserData>(request.Payload)), //Update Name, Bio, Image
                { Method: "DELETE", Path: var path} when isUsernameMatch(path) => _userManager.DeleteUser(parseUser(path), request.Headers),
                
                { Method: "POST", Path: "/sessions"} => _userManager.LoginUser(Deserialize<Credentials>(request.Payload)), //sends token back
                
                { Method: "POST", Path: "/packages"} => _packageManager.CreatePackage(Deserialize<CardFactory[]>(request.Payload), request.Headers), //requires admin, 5 cards, card must be unique!!!!!

                { Method: "POST", Path: "/transactions/packages"} => _packageManager.BuyPackage(request.Headers), //user buys package (token), then remove from db? array response

                { Method: "GET", Path: "/cards"} => _cardManager.GetUserCards(request.Headers), //show token users acquired cards, as array
                
                { Method: "GET", Path: var path} when isFormatPlainMatch(path) => _deckManager.GetUserDeckText(request.Headers), //textual description
                { Method: "GET", Path: "/deck"} => _deckManager.GetUserDeck(request.Headers), //response array of card deck
                { Method: "PUT", Path: "/deck"} => _deckManager.CreateDeck(request.Headers, Deserialize<string[]>(request.Payload)), //Four card ids to create new deck as array, response just status

                { Method: "GET", Path: "/stats"} => _userManager.GetUserStats(request.Headers), //Get UserStats: Name, Elo, Wins, Losses

                { Method: "GET", Path: "/scoreboard"} => _scoreboardManager.GetScoreboard(request.Headers), //als array, geordnet nach elo

                { Method: "POST", Path: "/battles"} => _battleManager.JoinLobby(request.Headers), //enter lobby to start battle, wait for other user, return battle log

                { Method: "GET", Path: "/tradings"} => _tradeManager.GetAvailableTrades(request.Headers), // retrieve all available deals, as array
                { Method: "POST", Path: "/tradings"} => _tradeManager.CreateTrade(request.Headers, Deserialize<TradingDealFactory>(request.Payload)),//create new deal, only for card you own, no response payload
                { Method: "DELETE", Path: var path} when isTradeIdMatch(path) => _tradeManager.DeleteDeal(parseTradeId(path), request.Headers), //Delete an existing deal, only by owner (id in path)
                { Method: "POST", Path: var path} when isTradeIdMatch(path) => _tradeManager.MakeDeal(parseTradeId(path), request.Headers, request.Payload), //carry out deal, request has tradeid and card id, no payload response, must be card owner, meet requirements
                
                _ => new HttpResponse(StatusCode.NotImplemented, "Route Not Implemented")

            };

            return response;
        }

        private T Deserialize<T>(string? body) where T : class
        {
            var data = body is not null ? JsonConvert.DeserializeObject<T>(body) : null;
            return data ?? throw new InvalidDataException();
        }


    }
}

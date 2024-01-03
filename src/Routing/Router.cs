using MTCG.HttpServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Router()
        {

        }
        public void Resolve(HttpRequest request)
        {
            string path = request.Path;
            switch (path)
            {
                case "/users":
                    break;
            }
        }

        public void HandleUserRequests(HttpRequest request)
        {

        }
    }
}

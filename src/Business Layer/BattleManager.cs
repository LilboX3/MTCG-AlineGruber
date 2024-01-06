using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Data_Layer;
using MTCG.HttpServer;
using MTCG.Models;

namespace MTCG.Business_Layer
{
    public class BattleManager
    {
        private List<User> lobby = new List<User>();
        private readonly UserDao _userDao;
        private readonly DeckDao _deckDao;
        public BattleManager(UserDao userDao, DeckDao deckDao)
        {
            _userDao = userDao;
            _deckDao = deckDao;
        }
        public HttpResponse JoinLobby(Dictionary<string, string> headers)
        {
            if (!headers.ContainsKey("Authorization:"))
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized error");
            }
            string token = GetToken(headers);
            User? user = _userDao.GetUserObjectByToken(token);
            if (user == null)
            {
                return new HttpResponse(StatusCode.InternalServerError, "User not found");
            }
            //Fill this players deck
            Card[]? deck = _deckDao.GetUserCardsByToken(token);
            if (deck == null)
            {
                return new HttpResponse(StatusCode.InternalServerError, "Could not create user deck");
            }
            FillUserDeck(deck, user);
            //TODO: race condition, lock list
            //lock List to avoid both threads accessing at the same time
            lock (lobby)
            {
                lobby.Add(user);
            }
            
            // Start battle if enough user in lobby
            if (lobby.Count == 2)
            {
                //TODO: battle
                if (lobby.ElementAt(0).UserCredentials.Username == lobby.ElementAt(1).UserCredentials.Username)
                {
                    return new HttpResponse(StatusCode.Forbidden, "Error: cannot battle yourself");
                }
                Battle battle = new Battle(lobby.ElementAt(0), lobby.ElementAt(1));
                User? winningUser = battle.PlayBattle();
                string winner;
                if (winningUser != null)
                {
                    winner = winningUser.UserCredentials.Username;
                    if (!_userDao.AddWin(winningUser))
                    {
                        return new HttpResponse(StatusCode.InternalServerError, "Error: could not update wins");
                    }
                    if (!_userDao.AddLoss(battle.GetLoser()))
                    {
                        return new HttpResponse(StatusCode.InternalServerError, "Error: could not update losses");
                    }
                }
                else
                {
                    winner = "It's a draw!";
                }
                string log = battle.BattleLog;
                if (!UpdateEloOfUsers())
                {
                    return new HttpResponse(StatusCode.InternalServerError, "Error: could not update elo");
                }
                lobby.RemoveAt(0);
                lobby.RemoveAt(0);
                //TODO: Update Elo of users
                
                return new HttpResponse(StatusCode.Ok, log+"\nWinner is: "+winner);
            }
            return new HttpResponse(StatusCode.Ok, "Not enough players (yet!)\n");
        }

        private bool UpdateEloOfUsers()
        {
            foreach (var user in lobby)
            {
                if (!_userDao.UpdateElo(user))
                {
                    return false;
                }
            }

            return true;
        }

        private void FillUserDeck(Card[] cards, User user)
        {
            foreach (var card in cards)
            {
                user.AddToDeck(card);
            }
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

using MTCG.Data_Layer;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.HttpServer;
using System.Text.Json;

namespace MTCG.Business_Layer
{
    public class UserManager
    {
        private readonly UserDao _userDao;

        public UserManager(UserDao userDao)
        {
            _userDao = userDao;
        }

        public HttpResponse RegisterUser(Credentials credentials)
        {
            string password = credentials.Password;
            var user = new User(credentials.Username, password);
            if (_userDao.InsertUser(user) == false)
            {
                return new HttpResponse(StatusCode.Conflict, "User with same username already registered");
            }
            return new HttpResponse(StatusCode.Created, "User successfully created");
        }

        public HttpResponse GetUserData(string username, Dictionary<string, string> headers)
        {
            //must be admin, or matching user!
            if (IsAdminOrUserToken(username, headers))
            {
                UserData? userData = _userDao.GetUserData(username);
                if (userData != null)
                {
                    string jsonString = JsonSerializer.Serialize(userData);
                    return new HttpResponse(StatusCode.Ok, "UserData: " + jsonString);
                }
                return new HttpResponse(StatusCode.NotFound, "User not found.");
            }
            else
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized Error");
            }
        }

        public HttpResponse UpdateUserData(string username, Dictionary<string, string> headers, UserData userData)
        {
            //must be admin, or matching user!
            if (IsAdminOrUserToken(username, headers))
            {
                if (_userDao.UpdateUserData(username, userData))
                {
                    return new HttpResponse(StatusCode.Ok, "User successfully updated.");
                }
                return new HttpResponse(StatusCode.NotFound, "User not found");
            }
            else
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized Error");
            }
        }

        public HttpResponse DeleteUser(string username, Dictionary<string, string> headers)
        {
            //must be admin, or matching user!
            if(IsAdminOrUserToken(username, headers))
            {
                if (_userDao.DeleteUser(username))
                {
                    return new HttpResponse(StatusCode.Ok, "User " + username + " successfully deleted.");
                }
                return new HttpResponse(StatusCode.NotFound, "User to delete not found");

            } else
            {
                return new HttpResponse(StatusCode.Unauthorized, "Unauthorized Error");
            }

        }

        public HttpResponse LoginUser(Credentials credentials)
        {
            string username = credentials.Username;
            string password = credentials.Password;
            string? token = _userDao.LoginUser(username, password);
            if (token != null)
            {
                return new HttpResponse(StatusCode.Ok, "User login successful: "+token);
            }
            return new HttpResponse(StatusCode.Unauthorized, "Invalid username/password provided");
        }

        private bool IsAdminOrUserToken(string username, Dictionary<string, string> headers)
        {
            string getToken = headers["Authorization:"];
            string[] tokens = getToken.Split(' ');
            string token = tokens[1];
            token = token.Trim();
            return token.Equals("admin-mtcgToken") || token.Equals(username + "-mtcgToken");
        }

    }
}

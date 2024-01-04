using MTCG.Data_Layer;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Business_Layer
{
    public class UserManager
    {
        private readonly UserDao _userDao;

        public UserManager(UserDao userDao)
        {
            _userDao = userDao;
        }

        public HttpServer.HttpResponse RegisterUser(Credentials credentials)
        {
            var user = new User(credentials.Username, credentials.Password);
            if (_userDao.InsertUser(user) == false)
            {
                return new HttpServer.HttpResponse(HttpServer.StatusCode.Conflict, "User with same username already registered");
            }
            return new HttpServer.HttpResponse(HttpServer.StatusCode.Created, "User successfully created");
        }


    }
}

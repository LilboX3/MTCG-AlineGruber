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
         /users
        /sessions
        /packages
        /transactions/packages
        /cards
        /deck
        /stats
        /scoreboard
        /battles
        /tradings

         */
        public Router()
        {

        }
        public void Resolve(HttpRequest request)
        {
            switch (request.Path)
            {

            }
        }
    }
}

﻿using MTCG.HttpServer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer.Routing
{
    internal interface IRouteCommand
    {
        HttpResponse Execute();
    }

}

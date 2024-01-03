﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer
{
    internal class HttpResponse
    {
        public StatusCode StatusCode { get; init; }
        public string? Payload { get; init; }

        public HttpResponse(StatusCode statusCode, string? payload = null)
        {
            StatusCode = statusCode;
            Payload = payload;
        }
    }

}

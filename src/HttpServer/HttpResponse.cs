using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer
{
    public class HttpResponse
    {
        public StatusCode StatusCode { get; set; }
        public string? Payload { get; set; }

        public HttpResponse(StatusCode statusCode, string? payload = null)
        {
            StatusCode = statusCode;
            Payload = payload;
        }
    }

}

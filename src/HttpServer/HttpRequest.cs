using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer
{
    public record HttpRequest
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string? Payload { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public HttpRequest(string method, string path, string? payload, Dictionary<string, string> headers)
        {
            if(method == null) throw new ArgumentNullException("method");
            if(path == null) throw new ArgumentNullException("path");

            Method = method;
            Path = path;
            Payload = payload;
            Headers = headers;
        }

    }
}

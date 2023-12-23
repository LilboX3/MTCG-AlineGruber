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
        public string Host { get; set; }
        public int ContentLength { get; set; }
        public string Payload { get; set; }
       
    }
}

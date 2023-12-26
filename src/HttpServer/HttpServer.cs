using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer
{
    internal class HttpServer
    {
        //private readonly Router _router;
        private readonly TcpListener _listener;
        private bool _listening;
        private int _serverPort;

        public HttpServer(/*Router router, */IPAddress address, int port)
        {
            //_router = router;
            _listener = new TcpListener(address, port);
            _listening = false;
            _serverPort = port;
        }

        public void Start()
        {
            _listener.Start();
            Console.WriteLine($"Server started on port {_serverPort}, looking for connections . . .");
            _listening = true;

            while (_listening)
            {
                var client = _listener.AcceptTcpClient();
                Console.WriteLine($"New connection from {client.Client.RemoteEndPoint}");
                HandleClient(client);
            }
        }

        public void Stop()
        {
            _listening = false;
            _listener.Stop();
        }

        /*
        POST /sessions HTTP/1.1
        Host: localhost:10001
        Content-Type: application/json
        Content-Length: ...
        ----------------------TRENNZEILE----------------------
        {"Username":"kienboec", "Password":"daniel"}*/

        //Erste line holen, splittenab
        //letzte line deserializen

        //Get requests via stream from client
        public void HandleClient(TcpClient client)
        {
            //Reader, cliebt variables
            StreamReader streamReader = new StreamReader(client.GetStream());
            StreamWriter streamWriter = new StreamWriter(client.GetStream());
            string? line;
            string[]? subs;
            int lineIndex = 0;

            //Request variables
            string method = "";

            while (true)
            {
                Console.WriteLine("Trying to read stream . . .");
             
                line = streamReader.ReadLine();
                
                if(line == null)
                {
                    Console.WriteLine("Leaving stream . . .");
                    break;
                }
                method += line;
                Console.WriteLine(method);
                /*subs = line.Split(' ');
                if (lineIndex == 0)
                {
                   method = subs[0];
                }
                lineIndex++;
                Console.WriteLine($"Sent request with {method}!");
                streamWriter.WriteLine($"Sent request with {method}!");*/
            }
            Console.WriteLine(method);


        }

       

    }
}

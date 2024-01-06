using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer
{
    public class HttpServer
    {
        private readonly TcpListener _listener;
        private bool _listening;
        private int _serverPort;
        private Router _router;

        public HttpServer(IPAddress address, int port, Router router)
        {
            _listener = new TcpListener(address, port);
            _listening = false;
            _serverPort = port;
            _router = router;
        }

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine($"Server started on port {_serverPort}, looking for connections . . .");
            _listening = true;

            while (_listening)
            {
                var client = _listener.AcceptTcpClient();
                Console.WriteLine($"New connection from {client.Client.RemoteEndPoint}");
                await Task.Run(async () => await  HandleClientAsync(client));
            }
        }

        public void Stop()
        {
            _listening = false;
            _listener.Stop();
        }

        //Erste line holen, splittenab
        //letzte line deserializen

        //Get requests via stream from client
        public async Task HandleClientAsync(TcpClient client)
        {
            StreamReader streamReader = new StreamReader(client.GetStream());
            HttpResponse? response;
            HttpRequest request = ReceiveRequest(streamReader);

            if(request is null)
            {
                response = new HttpResponse(StatusCode.BadRequest);
            } else
            {
                try
                {
                    response = _router.Resolve(request);
                }
                catch (InvalidDataException)
                {
                    response = new HttpResponse(StatusCode.InternalServerError, "Could not deserialize");
                }
            }

            if(response == null)
            {
                response = new HttpResponse(StatusCode.InternalServerError);
            }

            SendResponse(response, client);
  
        }

        private HttpRequest ReceiveRequest(StreamReader clientStream)
        {
            string data = ReadToEnd(clientStream);

            string[] getData = data.Split(' ');
            string method = getData[0];
            string path = getData[1];

            Console.WriteLine("METHOD: " + getData[0]);
            Console.WriteLine("PATH: " + getData[1]);

            Dictionary<string, string> headers = new Dictionary<string, string>();

            string[] lines = data.Split('\n');
            int lineIndex = 1;
            //key, value von header einlesen
            while (true)
            {
                //Bei Leerzeile stoppen (string empty funktioniert nicht. . .)
                if (lines[lineIndex + 1] == lines.LastOrDefault())
                {
                    break;
                }
                string[] header = lines[lineIndex].Split(' ');
                string key = header[0];
                string value = "";
                for (int i = 1; i < header.Length; i++)
                {
                    value += header[i] + " ";
                }
                headers.Add(key, value);
                Console.WriteLine($" KEY: {key} CONTENT: {value}");
                lineIndex++;
            }
            //letzte Zeile mit mitgeschickten Daten
            string payload = lines.LastOrDefault();
            Console.WriteLine("THE DATA IS HERE: " + payload + "\n");

            return new HttpRequest(method, path, payload, headers);
        }

        //ganzen HTTP Request einlesen, später parsen
        private string ReadToEnd(StreamReader reader)
        {
            var data = new StringBuilder(200); //kann mehr char storen wenn benötigt, modifikation erstellt keine neue instanz !

            var chars = new char[1024];

            var bytesRead = reader.Read(chars, 0, chars.Length);
            data.Append(chars, 0, bytesRead);   

            Console.WriteLine(data.ToString());

            return data.ToString();
        }

        private void SendResponse(HttpResponse response, TcpClient client)
        {
            // "using" keyword -> immediately dispose and close stream when going out of scope
            using NetworkStream stream = client.GetStream();
            using var writer = new StreamWriter(stream);

            Console.WriteLine("StatusCode should be: "+ (int)response.StatusCode);
            writer.Write($"HTTP/1.1 {(int)response.StatusCode} {response.StatusCode}\r\n");
            if (!string.IsNullOrEmpty(response.Payload))
            {
                Console.WriteLine("Payload should be: "+response.Payload);
                var payload = Encoding.UTF8.GetBytes(response.Payload);
                writer.Write($"Content-Length: {payload.Length}\r\n");
                writer.Write("\r\n");
                writer.Write(Encoding.UTF8.GetString(payload));
            }
            else
            {
                writer.Write("\r\n");
            }
        }

        public int GetPort()
        {
            return _serverPort;
        }



    }
}
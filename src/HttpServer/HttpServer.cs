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
        private readonly TcpListener _listener;
        private bool _listening;
        private int _serverPort;

        public HttpServer(IPAddress address, int port)
        {
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

        //Erste line holen, splittenab
        //letzte line deserializen

        //Get requests via stream from client
        public void HandleClient(TcpClient client)
        {
            StreamReader streamReader = new StreamReader(client.GetStream());
            string data = ReadToEnd(256, streamReader);

            //TODO: route rauslesen, switch statement für route, nach verarbeitung response schicken UND DANN IWANN DATABASE
            string[] getData = data.Split(' ');
            string method = getData[0];
            string path = getData[1];
            Console.WriteLine("METHOD: "+getData[0]);
            Console.WriteLine("PATH: " + getData[1]);
            Dictionary<string, string> headers = new Dictionary<string, string>();

            string[] lines = data.Split('\n');
            int lineIndex = 1;
            //key, value von header einlesen
            while (true)
            {
                //Bei Leerzeile stoppen (string empty funktioniert nicht. . .)
                if (lines[lineIndex+1]== lines.LastOrDefault())
                {
                    break;
                }
                string[] header = lines[lineIndex].Split(' ');
                string key = header[0];
                string value = "";
                for(int i=1;i<header.Length;i++)
                {
                    value += header[i]+" ";
                }
                headers.Add(key, value);
                Console.WriteLine($"KEY: {key} CONTENT: {value}");
                lineIndex++;
            }
            //letzte Zeile mit mitgeschickten Daten
            string payload = lines.LastOrDefault();
            Console.WriteLine("THE DATA IS HERE: "+payload);

            HttpRequest request = new HttpRequest(method, path, payload, headers);
        }

        //ganzen HTTP Request einlesen, später parsen
        private string ReadToEnd(int len, StreamReader reader)
        {
            var data = new StringBuilder(200); //kann mehr char storen wenn benötigt, modifikation erstellt keine neue instanz !
         
                var chars = new char[1024];
                var bytesReadTotal = 0;

                var bytesRead = reader.Read(chars, 0, chars.Length);
                bytesReadTotal += bytesRead;
                data.Append(chars, 0, bytesRead);


                Console.WriteLine(data.ToString());

            return data.ToString();
        }

       

    }
}

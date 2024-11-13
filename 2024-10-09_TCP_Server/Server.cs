using PlayGroundLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _2024_10_09_TCP_Server
{
    public class Server
    {

        public int port = 7531;

        private PlayGroundsRepository _playgroundRepository = new PlayGroundsRepository();        
        
        public void run()
        {
            Console.WriteLine("TCP Server String");

            TcpListener listener = new TcpListener(IPAddress.Any, port);

            listener.Start();

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                IPEndPoint clientEndPoint = socket.Client.RemoteEndPoint as IPEndPoint;
                Console.WriteLine("Client connected: " + clientEndPoint.Address);

                Task.Run(() => HandleClient(socket));
            }

            listener.Stop();
        }

        void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            while (socket.Connected)
            {
                // Client Input as String
                string? command = reader.ReadLine();

                // Parse as Int
                int ChildAge = int.Parse(command);

                // Find Object with Corresponding Age
                List<PlayGround> ?search = _playgroundRepository.GetAll().FindAll(x => x.MinAge <= ChildAge);

                if (search != null)
                {
                    string result = JsonSerializer.Serialize<List<PlayGround>>(search);
                    writer.Write(result);
                }


                writer.Flush();
            }
        }

    }
}

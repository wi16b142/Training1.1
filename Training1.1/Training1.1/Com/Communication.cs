using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Training1._1.Com
{
    public class Communication
    {
        private bool isServer = false;
        Socket serverSocket;
        Socket clientSocket;
        Thread acceptingThread;
        Thread receivingThread;
        Action<string> guiUpdate;
        Action<string> newMsg;
        List<ClientHandler> Clients = new List<ClientHandler>();
        byte[] buffer = new byte[512];
        const int port = 10100;

        public bool IsServer { get => isServer; set => isServer = value; }

        public Communication(bool  isServer, Action<string> guiUpdate)
        {
            IsServer = isServer;
            this.guiUpdate = guiUpdate;

            if (isServer)
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, port));
                serverSocket.Listen(10);
                StartAccepting();
            }
            else
            {
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Loopback, port);
                clientSocket = client.Client;
                StartReceiving();
            }

        }

        private void StartAccepting()
        {
            acceptingThread = new Thread(Accept);
            acceptingThread.IsBackground = true;
            acceptingThread.Start();
        }

        private void Accept()
        {
            while (acceptingThread.IsAlive)
            {
                Clients.Add(new ClientHandler(serverSocket.Accept()));
            }
        }

        private void StartReceiving()
        {
            receivingThread = new Thread(Receive);
            receivingThread.IsBackground = true;
            receivingThread.Start();
        }

        private void Receive()
        {
            while (receivingThread.IsAlive)
            {
                int length = clientSocket.Receive(buffer);
                string msg = Encoding.UTF8.GetString(buffer, 0, length);

                guiUpdate(msg);
            }
        }

        public void Send(string msg)
        {
            foreach (var client in Clients)
            {
                client.Send(msg);
            }
        }
        


    }
}

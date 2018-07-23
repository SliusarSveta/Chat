using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace Server
{
    class Server
    {
        int port;
        List<ProcessClient> clients = new List<ProcessClient>();
        TcpListener listener;

        internal protected Server(int port)
        {
            this.port = port;
            listener = new TcpListener(IPAddress.Any, port);
            ServerWork();
        }

        protected void ServerWork()
        {
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                ProcessClient processClient = new ProcessClient(client,this);
                clients.Add(processClient);
                Thread thread = new Thread(new ThreadStart(processClient.Processing));
                thread.Start();
            }
        }

        protected internal void Spreading(string message, string id)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                byte[] chars = Encoding.ASCII.GetBytes(message);
                if (clients[i].id != id)
                {
                    clients[i].stream.Write(chars, 0, chars.Length);
                }
            }
        }
    }
}

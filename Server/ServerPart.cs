using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;

namespace Server
{
    [DataContract]
    public class ServerPart
    {
        int port;
        [DataMember (Name ="Clients")]
        public List<ProcessClient> clients = new List<ProcessClient>();
        public TcpListener listener;

        internal ServerPart(int port)
        {
            this.port = port;
        }

        public void ServerWork()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ProcessClient processClient = new ProcessClient(client, this);
                    clients.Add(processClient);
                    processClient.registerOnServer();
                    string users = JsonConvert.SerializeObject(clients);
                    byte[] data = Encoding.ASCII.GetBytes(users);
                    processClient.stream.Write(data, 0, data.Length);
                    Thread.Sleep(500);
                    processClient.message = "connected";
                    Spreading(JsonConvert.SerializeObject(processClient), processClient.id);
                    Thread thread = new Thread(new ThreadStart(processClient.Processing));
                    thread.Start();
                }
            }
            catch 
            {
                //MainWindow.window.chatInfo.AppendText(ex.Message);
                Disconnect();
            }
        }

        public void Disconnect()
        {
            listener.Stop();
            for (int i=0; i<clients.Count; i++)
            {
                clients[i].Finish();
            }
        }

        protected internal void RemoveConnection(string id)
        {
            ProcessClient client = clients.FirstOrDefault(c => c.id == id);
            if (client != null)
            {
                clients.Remove(client);
                client.Finish();
            }
        }

        internal void Spreading(string message, string id)
        {
            byte[] chars = Encoding.ASCII.GetBytes(message);
            if (clients != null)
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i].id != id)
                    {
                        clients[i].stream.Write(chars, 0, chars.Length);
                    }
                }
            }
            return;
        }
    }
}


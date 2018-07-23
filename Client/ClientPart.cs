using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Input;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client
{
    [DataContract]
    public class ClientPart
    {
        static NetworkStream stream;
        TcpClient client;
        IPEndPoint point;
        Chat chat;
        [DataMember]
        string nick { get; set; }
        [DataMember]
        string color { get; set; }
        [DataMember]
        string message { get; set; }

       [JsonConstructor]
        public ClientPart(string color, string nick, string message)
        {
            this.color = color;
            this.nick = nick;
            this.message = message;
        }

        public ClientPart(string ipAdress, int port)
        {
            setChatWindow();
            point = new IPEndPoint(IPAddress.Parse(ipAdress), port);
            client = new TcpClient();
            try
            {
                Authorize();
                Thread receiveThread = new Thread(new ThreadStart(Processing));
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Can not connect to server.\r{0}", ex.Message));
            }
        }

        public void setChatWindow()
        {
            chat = new Chat();
            chat.send.Click += send_Click;
            chat.message.KeyUp += enter_Input;
            chat.disconnectButton.Click += disconnect_Click;
            chat.Show();
        }

        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            Disconnect();
        }
        private void send_Click(object sender, RoutedEventArgs e)
        {
            string request = chat.message.Text;
            if (request != "")
            {
                writeData(request);
                chat.message.Text = "";
            }
        }

        private void enter_Input(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                send_Click(sender, e);
            }
        }       

        public void Authorize()
        {
            client.Connect(point);
            stream = client.GetStream();
            this.color = MainWindow.window.nick.Foreground.ToString();
            this.nick = MainWindow.window.nick.Text;
            writeData(color + nick);
        }        
        
        public string readData()
        {
            byte[] buffer = new byte[1024];
            int answer = 0;
            try
            {
                do
                {
                    answer = stream.Read(buffer, 0, buffer.Length);
                }
                while (stream.DataAvailable);

                return Encoding.ASCII.GetString(buffer, 0, answer);
            }
            catch
            {
                Disconnect();
                return "";
            }
        }

        public void writeData(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        public void AddUserToListBox(ClientPart client)
        {
            chat.Dispatcher.BeginInvoke(
                (Action)(() =>
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = client.nick + " online";
                    item.Foreground = (Brush)new BrushConverter().ConvertFromString(client.color);
                    chat.users.Items.Add(item);
                }));
        }

        public void RemoveAtListBox(int index)
        {
            chat.Dispatcher.BeginInvoke(
                (Action)(() =>
                {
                    chat.users.Items.RemoveAt(index);
                }));
        }

        public void AddUsersToListBox(List<ClientPart> clients)
        {
            chat.Dispatcher.BeginInvoke(                
                (Action)(() =>
                {
                    chat.users.Items.Clear();
                    chat.users.Items.Refresh();
                    foreach (ClientPart client in clients)
                    {
                        AddUserToListBox(client);   
                    }
                }));
        }

        public void Processing()
        {
            string info = readData();

            List<ClientPart> clients = JsonConvert.DeserializeObject<List<ClientPart>>(info);

            AddUsersToListBox(clients);

            while (client.Connected)
            {
                info = readData();
                if (info != "")
                {
                    ClientPart client = JsonConvert.DeserializeObject<ClientPart>(info);
                    if (client.message == "connected")
                    {
                        clients.Add(client);
                        AddUserToListBox(client);
                    }
                    else if (client.message == "disconnected")
                    {
                        RemoveAtListBox(clients.IndexOf(client));
                        clients.Remove(client);
                    }
                    else
                    {
                        showMessageFromClient(client);
                    }
                }
            }           
        }

        void showMessageFromClient(ClientPart client)
        {
            chat.Dispatcher.BeginInvoke(
                (Action)(() =>
                {
                    Brush brush = brushFromString(client.color);
                    TextPointer previewTextEnd = chat.chatText.Document.ContentEnd;
                    TextRange nickRange = new TextRange(previewTextEnd, previewTextEnd);
                    nickRange.Text = client.nick + " ";
                    nickRange.ApplyPropertyValue(RichTextBox.ForegroundProperty, brush);
                    TextPointer nickRangeEnd = chat.chatText.Document.ContentEnd;
                    TextRange messageRange = new TextRange(nickRangeEnd, nickRangeEnd);
                    messageRange.Text = client.message + "\r";
                    messageRange.ClearAllProperties();
                    chat.chatText.ScrollToEnd();
                }));
        }

        Brush brushFromString(string color)
        {
            Brush brush = (Brush)new BrushConverter().ConvertFromString(color);
            return brush;
        }

        public void Disconnect()
        {
            if (stream != null)
            {
                stream.Close();
            }
            if (client != null)
                client.Close();
            Console.WriteLine(client.Connected);
            chat.Dispatcher.BeginInvoke(
                (Action)(() =>
                {
                    chat.Close();
                }));
     
        }

        public override bool Equals(object obj)
        {
            ClientPart client = (ClientPart)obj;
            if ((client.nick == this.nick) && (client.color == this.color))
                return true;
            return false;
        }

    }
}

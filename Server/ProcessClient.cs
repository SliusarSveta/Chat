using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace Server
{
    [DataContract]
    public class ProcessClient
    {
        public TcpClient client;
        ServerPart server;
        internal protected string id;
        internal protected NetworkStream stream;
        [DataMember]
        public string nick;
        [DataMember]
        public string color;
        [DataMember]
        public string message;

        public ProcessClient() { }
        internal protected ProcessClient(TcpClient client, ServerPart server)
        {
            this.client = client;
            id = Guid.NewGuid().ToString();
            this.server = server;
            stream = client.GetStream();
            string message = GetMessage();
            this.color = message.Substring(0, 9);
            this.nick = message.Substring(9);
        }

        public void registerOnServer()
        {            
            string info = nick + " connected to the chat at " + DateTime.Now.TimeOfDay + "\r";
            changeStringColor(info);
        }

        void changeStringColor(string str)
        {
            MainWindow.window.Dispatcher.BeginInvoke(
            (Action)(() =>
            {
                TextPointer textEnd = MainWindow.window.chatInfo.Document.ContentEnd;
                TextRange range = new TextRange(textEnd, textEnd);
                range.Text = str;
                range.ApplyPropertyValue(RichTextBox.ForegroundProperty, brushFromString(color));
                MainWindow.window.chatInfo.ScrollToEnd();
            }));
        }
        Brush brushFromString(string color)
        {
            Brush brush = (Brush)new BrushConverter().ConvertFromString(color);
            return brush;
        }

        internal protected void Processing()
        {
            try
            {
                while (client.Connected)
                {
                   
                    this.message = GetMessage();
                    if (this.message != "")
                    {
                        string data = JsonConvert.SerializeObject(this);
                        server.Spreading(data, "");
                    }
                    else
                    { 
                        string info = this.nick + " disconnected at " + DateTime.Now.TimeOfDay + "\r";
                        changeStringColor(info);                        
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                this.message = "disconnected";
                server.Spreading(JsonConvert.SerializeObject(this), this.id);
                server.RemoveConnection(this.id);
            }
        }
        private string GetMessage()
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);

            return builder.ToString();
        }

        public void Finish()
        {
            if (this.stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }
    }  
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Server
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow window = null;
        static ServerPart server;
        static Thread listen;
        public MainWindow()
        {
            window = this;
            InitializeComponent();            
        }

        private void startClick(object sender, RoutedEventArgs e)
        {
            try
            {
                server = new ServerPart(Convert.ToInt32(portBox.Text));
                listen = new Thread(new ThreadStart(server.ServerWork));
                listen.Start();
                this.chatInfo.AppendText("Server waiting for connecting...\r");
            }
            catch (Exception ex)
            {
                server.Disconnect();
                chatInfo.AppendText(String.Format("Can not start server work. {0}", ex.Message));
            }
        }

        private void finishClick(object sender, RoutedEventArgs e)
        {
            server.Disconnect();
            this.Close();
            Environment.Exit(0);
        }
    }
}

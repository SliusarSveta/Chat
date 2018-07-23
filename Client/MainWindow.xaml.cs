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

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClientPart client;
        public static MainWindow window = null;
        public MainWindow()
        {
            window = this;
            InitializeComponent();
        }       

        private void connect_Click(object sender, RoutedEventArgs e)
        {
            client = new ClientPart(ipBox.Text, Convert.ToInt32(portBox.Text));     
            
            this.Close();
        }

        private void close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

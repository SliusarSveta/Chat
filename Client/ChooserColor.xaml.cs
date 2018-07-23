using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для ChooserColor.xaml
    /// </summary>
    public partial class ChooserColor : UserControl
    {
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Brush), typeof(ChooserColor));
        public ChooserColor()
        {
            InitializeComponent();
        }

        public Brush SelectedColor
        {
            get { return (Brush)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }
    }
}

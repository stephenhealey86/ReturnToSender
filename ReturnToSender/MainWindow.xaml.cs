using ReturnToSender.ViewModels;
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

namespace ReturnToSender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.DataContext = new MainWindowViewModel(this);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Style _style = null;
            if (Microsoft.Windows.Shell.SystemParameters2.Current.IsGlassEnabled == true)
            {
                _style = (Style)Resources["CustomWindowStyle"];
            }
            this.Style = _style;
        }
    }
}

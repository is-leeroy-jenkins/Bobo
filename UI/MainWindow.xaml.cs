using System.Windows;

namespace Bobo
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();

            DataContext = mainViewModel;            
        }
    }
}

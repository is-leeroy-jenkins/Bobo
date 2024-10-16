
namespace Bobo
{
    using System.Windows;

    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;            
        }
    }
}

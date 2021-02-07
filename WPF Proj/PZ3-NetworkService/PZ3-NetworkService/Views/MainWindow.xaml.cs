using PZ3_NetworkService.ViewModels;
using System.Windows;

namespace PZ3_NetworkService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = new MainWindowViewModel(); };
        }
    }
}

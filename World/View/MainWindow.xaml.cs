using System.Windows;
using World.ViewModel;
using Newtonsoft.Json;

namespace World.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DataManageVM();
        }
    }
}

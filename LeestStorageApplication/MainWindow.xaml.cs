
using System.Windows;
using LeestStorageServer;

namespace LeestStorageApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            CommunicationHandler handler = new CommunicationHandler();
            InitializeComponent();
            
        }
    }
}

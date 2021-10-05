using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using LeestStorageServer;

namespace LeestStorageApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly ViewModel viewModel;

        public MainWindow()
        {

            //CommunicationHandler handler = new CommunicationHandler();
            InitializeComponent();
            viewModel = new ViewModel();
            this.DataContext = viewModel;

        }

    }

}

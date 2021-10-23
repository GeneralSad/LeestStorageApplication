using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LeestStorageApplication
{
    public partial class App : Application
    {

        private ViewModel viewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow();

            viewModel = new ViewModel();

            window.DataContext = viewModel;

            window.Show();
            window.Closed += viewModel.Window_Closed;
        }

    }
}

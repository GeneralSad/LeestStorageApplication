using System;
using System.Windows;

namespace LeestStorageApplication
{
    public partial class App : Application
    {

        private ViewModel viewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow window = new();

            viewModel = new ViewModel();

            window.DataContext = viewModel;

            window.Show();
            window.Closed += viewModel.Window_Closed;
        }

    }
}

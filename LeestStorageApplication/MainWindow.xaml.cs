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
            /*
            DirectoryFolder folder1 = new DirectoryFolder("hell", null);
            DirectoryFolder folder2 = new DirectoryFolder("yeah", folder1);
            DirectoryFolder folder3 = new DirectoryFolder("naw", folder1);
            DirectoryFolder folder4 = new DirectoryFolder("man", folder2);

            DirectoryFile file1 = new DirectoryFile("ThatsRad.txt", folder2); //Hell/Yeah
            DirectoryFile file2 = new DirectoryFile("ThatsNotSoRad.txt", folder3); //Hell/Naw
            DirectoryFile file3 = new DirectoryFile("ThatsRad.txt", folder4); //Hell/Yeah/Man

            Debug.WriteLine("DirectoryFile 1: " + file1.getFilePath());
            Debug.WriteLine("DirectoryFile 2: " + file2.getFilePath());
            Debug.WriteLine("DirectoryFile 3: " + file3.getFilePath());


            List<Item> Items = new List<Item>{
                new Item{Name= "Hell", Path=folder1.getFilePath()},
                new Item{Name= "Yeah", Path=folder2.getFilePath()},
                new Item{Name= "Naw", Path=folder3.getFilePath()},
                new Item{Name= "ThatsRad.txt", Path=file3.getFilePath()}
            };
            */
            
            InitializeComponent();
            viewModel = new ViewModel();
            this.DataContext = viewModel;

        }

    }

}

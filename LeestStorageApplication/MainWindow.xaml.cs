using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //Client client = new Client(25565);

            Folder folder1 = new Folder("hell", null);
            Folder folder2 = new Folder("yeah", folder1);
            Folder folder3 = new Folder("naw", folder1);
            Folder folder4 = new Folder("man", folder2);

            File file1 = new File("ThatsRad.txt", folder2); //Hell/Yeah
            File file2 = new File("ThatsNotSoRad.txt", folder3); //Hell/Naw
            File file3 = new File("ThatsRad.txt", folder4); //Hell/Yeah/Man

            Debug.WriteLine("File 1: " + file1.getFilePath());
            Debug.WriteLine("File 2: " + file2.getFilePath());
            Debug.WriteLine("File 3: " + file3.getFilePath());

            CommunicationHandler handler = new CommunicationHandler();
            InitializeComponent();
            
        }
    }
}

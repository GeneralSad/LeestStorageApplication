using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace LeestStorageApplication
{

    public class ViewModel : BindableBase, INotifyPropertyChanged, ItemListCallback
    {

        public DelegateCommand<object> cReload { get; private set; }
        public DelegateCommand<object> cDownload { get; private set; }
        public DelegateCommand<object> cUpload { get; private set; }
        public DelegateCommand<object> cDelete { get; private set; }
        public DelegateCommand<object> cEnterFile { get; private set; }

        public ObservableCollection<IDirectoryItem> Items { get; set; }

        CommunicationHandler handler;

        public ViewModel()
        {
            this.handler = new CommunicationHandler(this);

            cReload = new DelegateCommand<object>(Reload, canSubmit);
            cDownload = new DelegateCommand<object>(Download, canSubmit);
            cUpload = new DelegateCommand<object>(Upload, canSubmit);
            cDelete = new DelegateCommand<object>(Delete, canSubmit);
            cEnterFile = new DelegateCommand<object>(EnterFile, canSubmit);

            //DirectoryFolder folder1 = new DirectoryFolder("hell", null);
            //DirectoryFolder folder2 = new DirectoryFolder("yeah", folder1);
            //DirectoryFolder folder3 = new DirectoryFolder("naw", folder1);
            //DirectoryFolder folder4 = new DirectoryFolder("man", folder2);

            //DirectoryFile file1 = new DirectoryFile("ThatsRad.txt", folder2); //Hell/Yeah
            //DirectoryFile file2 = new DirectoryFile("ThatsNotSoRad.txt", folder3); //Hell/Naw
            //DirectoryFile file3 = new DirectoryFile("ThatsRad.txt", folder4); //Hell/Yeah/Man

            //Debug.WriteLine("DirectoryFile 1: " + file1.GetFilePath());
            //Debug.WriteLine("DirectoryFile 2: " + file2.GetFilePath());
            //Debug.WriteLine("DirectoryFile 3: " + file3.GetFilePath());

            Items = new ObservableCollection<IDirectoryItem>();
            //Items.Add(new Item { Name = "Hell", Path = folder1.GetFilePath() });
            //Items.Add(new Item { Name = "Yeah", Path = folder2.GetFilePath() });
            //Items.Add(new Item { Name = "Naw", Path = folder3.GetFilePath() });
            //Items.Add(new Item { Name = "ThatsRad.txt", Path = file3.GetFilePath() });
        }

        public IDirectoryItem SelectedItem { get; set; }

        public bool canSubmit(object parameter)
        {
            return true;
        }

        public async void Reload(object parameter)
        {
            await handler.SendMessage(new { type = "DirectoryRequest" } );
            Debug.WriteLine("Reloaded");
        }

        public void Download(object parameter)
        {
            ListView listView = (ListView)parameter;
            int number = Items.IndexOf((IDirectoryItem)listView.SelectedItem);
            Debug.WriteLine("Download: " + number);
        }

        public void Upload(object parameter)
        {
            Debug.WriteLine("Upload");
        }

        public void Delete(object parameter)
        {
            ListView listView = (ListView)parameter;
            int number = Items.IndexOf((IDirectoryItem)listView.SelectedItem);
            Debug.WriteLine("Delete: " + number);
        }

        public void EnterFile(object parameter)
        {
            ListView listView = (ListView)parameter;
            int number = Items.IndexOf((IDirectoryItem)listView.SelectedItem);
            Debug.WriteLine("Enter: " + number);
        }

        public void notify(ObservableCollection<IDirectoryItem> observable)
        {
            this.Items = observable;
        }
    }

}

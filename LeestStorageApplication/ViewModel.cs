using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Prism.Services.Dialogs;

namespace LeestStorageApplication
{

    public class ViewModel : BindableBase, INotifyPropertyChanged, ItemListCallback
    {

        public DelegateCommand<object> cBack { get; private set; }
        public DelegateCommand<object> cReload { get; private set; }
        public DelegateCommand<object> cDownload { get; private set; }
        public DelegateCommand<object> cUpload { get; private set; }
        public DelegateCommand<object> cDelete { get; private set; }
        public DelegateCommand<object> cEnterFile { get; private set; }

        public ObservableCollection<IDirectoryItem> Items { get; set; }

        private OpenFileDialog openFileDialog { get; set; }

        private CommunicationHandler handler { get; set; }

        public ViewModel()
        {
            this.handler = new CommunicationHandler(this);

            cBack = new DelegateCommand<object>(Back, canSubmit);
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
            await this.handler.Reload();
        }

        public async void Download(object parameter)
        {
            ListView listView = (ListView)parameter;
            int number = Items.IndexOf((IDirectoryItem)listView.SelectedItem);
            if (number != -1)
            {
                await this.handler.DownloadRequest(this.Items[number].Name);
            }
        }

        public async void Upload(object parameter)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select file to upload";
            openFileDialog.Multiselect = false;
            openFileDialog.ValidateNames = true;
            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                await this.handler.UploadRequest(openFileDialog.FileName);
                Debug.WriteLine(openFileDialog.FileName);
            }

            Debug.WriteLine("Upload");
        }

        public async void Delete(object parameter)
        {

            ListView listView = (ListView)parameter;
            int number = Items.IndexOf((IDirectoryItem)listView.SelectedItem);
            if (number != -1)
            {
                string fileName = this.Items[number].Name;
                Debug.WriteLine($@"Delete: {number} {fileName}");

                string messageBoxText;


                if (fileName.Contains('.'))
                {
                    messageBoxText = "Are you sure you want to delete this File?";
                }
                else
                {
                    messageBoxText = "Are you sure you want to delete this folder and all of the files in it?";
                }

                MessageBoxResult messageBoxResult = messageBoxResult = MessageBox.Show(messageBoxText, "Confirm delete operation", MessageBoxButton.YesNo);


                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    await this.handler.DeleteRequest(fileName);
                }
            }
        }

        //This button is not async because allowing the user to use other buttons while loading will break the application
        public async void EnterFile(object parameter)
        {
            ListView listView = (ListView)parameter;
            int number = Items.IndexOf((IDirectoryItem)listView.SelectedItem);

            if (number != -1)
            {
                string directory = this.Items[number].Name;
                Debug.WriteLine("Enter: " + directory);
                if (!directory.Contains("."))
                {
                    await this.handler.IntoDirectoryRequest(directory);
                }
            }
        }

        public async void Back(object parameter)
        {
            await this.handler.OutOfDirectoryRequest();
            Debug.WriteLine("Back");
        }

        public void notify(ObservableCollection<IDirectoryItem> observable)
        {
            this.Items = observable;
        }

        public async void Window_Closed(object sender, EventArgs e)
        {
            await this.handler.CloseConnection();

        }
    }

}

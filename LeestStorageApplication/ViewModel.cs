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

        public DelegateCommand cBack { get; private set; }
        public DelegateCommand cReload { get; private set; }
        public DelegateCommand cDownload { get; private set; }
        public DelegateCommand cUpload { get; private set; }
        public DelegateCommand cDelete { get; private set; }
        public DelegateCommand cEnterFile { get; private set; }
        public DelegateCommand cToggleCreateFolder { get; private set; }
        public DelegateCommand cCreateFolder { get; private set; }

        public ObservableCollection<IDirectoryItem> Items { get; set; }

        private OpenFileDialog OpenFileDialog { get; set; }

        private CommunicationHandler Handler { get; set; }

        public ViewModel()
        {
            this.Handler = new CommunicationHandler(this);

            cBack = new DelegateCommand(Back);
            cReload = new DelegateCommand(Reload);
            cDownload = new DelegateCommand(Download);
            cUpload = new DelegateCommand(Upload);
            cDelete = new DelegateCommand(Delete);
            cEnterFile = new DelegateCommand(EnterFile);
            cToggleCreateFolder = new DelegateCommand(ToggleCreateFolder);
            cCreateFolder = new DelegateCommand(CreateFolder);

            Items = new ObservableCollection<IDirectoryItem>();
        }

        public IDirectoryItem SelectedItem { get; set; }
        public ListView Listview { get; set; }
        public bool PopupVisible { get; set; }
        public string PopupFolderName { get; set; }

        public async void Reload()
        {
           await this.Handler.Reload();
        }

        public async void Download()
        {
            int number = Items.IndexOf(SelectedItem);
            if(number != -1)
            {
                await this.Handler.DownloadRequest(this.Items[number].Name);
            }
        }

        public async void Upload()
        {
            OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Title = "Select file to upload";
            OpenFileDialog.Multiselect = false;
            OpenFileDialog.ValidateNames = true;
            Nullable<bool> result = OpenFileDialog.ShowDialog();

            if (result == true)
            {
                await this.Handler.UploadRequest(OpenFileDialog.FileName);
                Debug.WriteLine(OpenFileDialog.FileName);
            }

            Debug.WriteLine("Upload");
        }

        public void ToggleCreateFolder()
        {

            PopupVisible = !PopupVisible;

        }

        public async void CreateFolder()
        {
            PopupVisible = false;
            await this.Handler.CreateFolderRequest(PopupFolderName);
            Debug.WriteLine("Creating: " + PopupFolderName);
            PopupFolderName = "";
        }

        public async void Delete()
        {
            int number = Items.IndexOf(SelectedItem);
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

                    await this.Handler.DeleteRequest(fileName);

                }
            }
        }

        //This button is not async because allowing the user to use other buttons while loading will break the application
        public async void EnterFile()
        {
            int number = Items.IndexOf(SelectedItem);

            if (number != -1)
            {
                string directory = this.Items[number].Name;
                Debug.WriteLine("Enter: " + directory);
                if (!directory.Contains("."))
                {
                   await this.Handler.IntoDirectoryRequest(directory);
                }
            }
        }

        public async void Back()
        {
            await this.Handler.OutOfDirectoryRequest();
            Debug.WriteLine("Back");
        }

        public void notify(ObservableCollection<IDirectoryItem> observable)
        {
            this.Items = observable;
        }

        public async void Window_Closed(object sender, EventArgs e)
        {
            await this.Handler.CloseConnection();

        }

    }

}

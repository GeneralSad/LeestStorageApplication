using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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

    public class ViewModel : BindableBase, INotifyPropertyChanged
    {

        public DelegateCommand<object> cReload { get; private set; }
        public DelegateCommand<object> cDownload { get; private set; }
        public DelegateCommand<object> cUpload { get; private set; }
        public DelegateCommand<object> cDelete { get; private set; }
        public DelegateCommand<object> cMoreInfo { get; private set; }
        public DelegateCommand<object> cEnterFile { get; private set; }

        public List<Item> Items { get; set; }

        private Item mSelectedItem = null;

        public ViewModel()
        {
            cReload = new DelegateCommand<object>(Reload, canSubmit);
            cDownload = new DelegateCommand<object>(Download, canSubmit);
            cUpload = new DelegateCommand<object>(Upload, canSubmit);
            cDelete = new DelegateCommand<object>(Delete, canSubmit);
            cMoreInfo = new DelegateCommand<object>(MoreInfo, canSubmit);
            cEnterFile = new DelegateCommand<object>(EnterFile, canSubmit);

            Folder folder1 = new Folder("hell", null);
            Folder folder2 = new Folder("yeah", folder1);
            Folder folder3 = new Folder("naw", folder1);
            Folder folder4 = new Folder("man", folder2);

            File file1 = new File("ThatsRad.txt", folder2); //Hell/Yeah
            File file2 = new File("ThatsNotSoRad.txt", folder3); //Hell/Naw
            File file3 = new File("ThatsRad.txt", folder4); //Hell/Yeah/Man

            Debug.WriteLine("File 1: " + file1.GetFilePath());
            Debug.WriteLine("File 2: " + file2.GetFilePath());
            Debug.WriteLine("File 3: " + file3.GetFilePath());

            Items = new List<Item>();
            Items.Add(new Item { Name = "Hell", Path = folder1.GetFilePath() });
            Items.Add(new Item { Name = "Yeah", Path = folder2.GetFilePath() });
            Items.Add(new Item { Name = "Naw", Path = folder3.GetFilePath() });
            Items.Add(new Item { Name = "ThatsRad.txt", Path = file3.GetFilePath() });
        }

        public Item SelectedItem
        {
            get { return mSelectedItem; }
            set
            {
                if (mSelectedItem!= value)
                {
                    if (mSelectedItem == null)
                    {
                        mSelectedItem = new Item();
                    }
                    mSelectedItem = value;
                }
            }
        }

        public bool canSubmit(object parameter)
        {
            return true;
        }

        public void Reload(object parameter)
        {
            Debug.WriteLine("Reload");
        }

        public void Download(object parameter)
        {
            Debug.WriteLine("Download");
        }

        public void Upload(object parameter)
        {
            Debug.WriteLine("Upload");
        }

        public void Delete(object parameter)
        {
            Debug.WriteLine("Delete");
        }

        public void MoreInfo(object parameter)
        {
            Debug.WriteLine("More Info");
        }

        public void EnterFile(object ListView)
        {
            Debug.WriteLine("Click Click");
        }

    }

    public class Item
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string DetailInfo
        {
            get
            {
                return "Path: " + Path;
            }
        }
    }

}

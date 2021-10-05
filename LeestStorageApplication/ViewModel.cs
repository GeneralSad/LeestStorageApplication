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

    public class ViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private List<Item> items;

        private Item mSelectedItem = null;

        public ViewModel()
        {
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

            items = new List<Item>();
            items.Add(new Item { Name = "Hell", Path = folder1.getFilePath() });
            items.Add(new Item { Name = "Yeah", Path = folder2.getFilePath() });
            items.Add(new Item { Name = "Naw", Path = folder3.getFilePath() });
            items.Add(new Item { Name = "ThatsRad.txt", Path = file3.getFilePath() });
        }

        public List<Item> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChange("DirectoryListView");
            }
        }

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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

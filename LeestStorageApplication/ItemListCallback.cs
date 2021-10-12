﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeestStorageApplication
{
    public interface ItemListCallback
    {
        public abstract void notify(ObservableCollection<IDirectoryItem> observable);
    }
}
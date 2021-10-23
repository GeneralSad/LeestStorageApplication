using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeestStorageServer
{
    interface ServerCallback
    {
        void RemoveClientHandlerFromList(ClientHandler handler);

        void RefreshDirectoryForAllClientsInDirectory(string directory);
    }
}

using System;

namespace LeestStorageServer
{
    interface ServerCallback
    {
        void RemoveClientHandlerFromList(ClientHandler handler);

        void RefreshDirectoryForAllClientsInDirectory(string directory);

        bool CheckIfFileIsClearToEdit(string file);

        void AddFileBeingEdited(string file);

        void RemoveFileBeingEdited(string file);
    }
}

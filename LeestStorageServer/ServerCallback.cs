using System;

namespace LeestStorageServer
{
    public interface ServerCallback
    {
        void RemoveClientHandlerFromList(ClientHandler handler);

        void RefreshDirectoryForAllClientsInDirectory(string directory);

        bool CheckIfFileIsClearToEdit(string file);

        void AddFileBeingEdited(string file);

        void RemoveFileBeingEdited(string file);
    }
}

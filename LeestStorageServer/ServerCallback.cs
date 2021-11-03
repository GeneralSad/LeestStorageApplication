using System;

namespace LeestStorageServer
{
    public interface ServerCallback
    {
        void RemoveClientHandlerFromList(ClientHandler handler);

        void RefreshDirectoryForAllClientsInDirectory(string directory);

        bool CheckIfFileIsClearToEdit(string filePath);

        void AddFileBeingEdited(string filePath);

        void RemoveFileBeingEdited(string filePath);
    }
}

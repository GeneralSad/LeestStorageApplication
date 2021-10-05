using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationObjects
{
    interface MessageCallback
    {
        public void MessageReceived(string message);
    }
}

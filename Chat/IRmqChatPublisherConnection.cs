using System;

namespace Chat
{
    public interface IRmqChatPublisherConnection: IDisposable
    {
        string NickName { get; set; }
        void Publish(string message);
    }
}
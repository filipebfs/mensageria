using System;

namespace Chat
{
    public interface IRmqChatConsumerConnection: IDisposable
    {
        Action<string, string> MessageReceived
        {
            get; set;
        }
    }
}
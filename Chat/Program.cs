using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Practices.Unity;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var unity = ChatUnityContainer.GetNewContainer())
                {
                    new Program().DoMain(unity);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}\n\n{1}", e.Message, e.StackTrace);
            }
        }

        private void DoMain(UnityContainer unity)
        {
            Console.Write("nickname: ");
            var nickname = "";
            while (string.IsNullOrEmpty(nickname))
            {
                nickname = Console.ReadLine();
            }

            var rmqConnFactory = unity.Resolve<RmqChatConnectionFactory>();

            using (var connection = rmqConnFactory.CreateConsumerChannel())
            {
                Console.WriteLine("CHAT\n====");

                connection.MessageReceived += (pubNickname, mensagem) =>
                {
                    Console.WriteLine("[{0}]: {1}", pubNickname, mensagem);
                };

                using (var connection2 = rmqConnFactory.CreatePublisherChannel())
                {
                    connection2.NickName = nickname;

                    while (true)
                    {
                        Console.Write("> ");
                        var message = Console.ReadLine();

                        connection2.Publish(message);
                    }
                }
            }
        }
    }
}

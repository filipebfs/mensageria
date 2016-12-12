using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public static class ChatUnityContainer
    {
        public static UnityContainer GetNewContainer()
        {
            var unity = new UnityContainer();

            unity.RegisterType<RabbitMQ.Client.IConnectionFactory, RabbitMQ.Client.ConnectionFactory>();

            return unity;
        }
    }
}

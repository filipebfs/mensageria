using RabbitMQ.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public class RmqChatConnectionFactory
    {
        private IConnectionFactory _rmqConnFactory;
        private string _hostName;

        public RmqChatConnectionFactory(IConnectionFactory rabbitmqConnectionFactory)
        {
            if (rabbitmqConnectionFactory == null) throw new ArgumentNullException("rabbitmqConnectionFactory");

            _rmqConnFactory = rabbitmqConnectionFactory;
            _hostName = System.Configuration.ConfigurationManager.AppSettings["host_name"];
        }

        public IRmqChatPublisherConnection CreatePublisherChannel()
        {
            var connection = _rmqConnFactory.CreateConnection(new[] { _hostName });
            var publisher = new RmqChatPublisherConnection(connection);

            return publisher;
        }


        public IRmqChatConsumerConnection CreateConsumerChannel()
        {
            var connection = _rmqConnFactory.CreateConnection(new[] { _hostName });
            var consumer = new RmqChatConsumerConnection(connection);

            return consumer;
        }
    }
}

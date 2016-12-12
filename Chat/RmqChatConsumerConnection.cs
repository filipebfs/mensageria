using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public class RmqChatConsumerConnection: IRmqChatConsumerConnection
    {
        private IModel _rmqChannel;
        private IConnection _rmqConn;

        public RmqChatConsumerConnection(IConnection RabbitmqConnection)
        {
            if (RabbitmqConnection == null) throw new ArgumentNullException("RabbitmqConnection");

            _rmqConn = RabbitmqConnection;
            _rmqChannel = _rmqConn.CreateModel();

            var exchangeName = System.Configuration.ConfigurationManager.AppSettings["fanout_exchange_name"];

            _rmqChannel.ExchangeDeclare(exchange: exchangeName, type: "fanout");

            var queueName = _rmqChannel.QueueDeclare().QueueName;
            _rmqChannel.QueueBind(queue: queueName,
                              exchange: exchangeName,
                              routingKey: "");

            var consumer = new EventingBasicConsumer(_rmqChannel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var bodyMessage = Encoding.UTF8.GetString(body);
                var spdMsg = bodyMessage.Split('§');
                var nickname = spdMsg[0];
                var message = spdMsg[1];
                if (this.MessageReceived != null) this.MessageReceived(nickname, message);
            };

            _rmqChannel.BasicConsume(queue: queueName,
                                 noAck: true,
                                 consumer: consumer);
        }

        public Action<string, string> MessageReceived
        {
            get; set;
        }

        public void Dispose()
        {
            _rmqChannel.Dispose();
            _rmqConn.Dispose();
        }
    }
}

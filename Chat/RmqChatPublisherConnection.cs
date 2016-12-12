using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public class RmqChatPublisherConnection: IRmqChatPublisherConnection
    {
        private IConnection _rmqConn;
        private IModel _rmqChannel;
        private string _exchangeName;

        public RmqChatPublisherConnection(IConnection RabbitmqConnection)
        {
            if (RabbitmqConnection == null) throw new ArgumentNullException("RabbitmqConnection");

            _rmqConn = RabbitmqConnection;
            _rmqChannel = _rmqConn.CreateModel();

            _exchangeName = System.Configuration.ConfigurationManager.AppSettings["fanout_exchange_name"];

            _rmqChannel.ExchangeDeclare(exchange: _exchangeName, type: "fanout");
        }

        public string NickName { get; set; }

        public void Publish(string message)
        {
            var bodyMessage = String.Format("{0}§{1}", this.NickName ?? "Annon", message);
            var body = Encoding.UTF8.GetBytes(bodyMessage);
            _rmqChannel.BasicPublish(exchange: _exchangeName,
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
        }

        public void Dispose()
        {
            _rmqChannel.Dispose();
            _rmqConn.Dispose();
        }
    }
}

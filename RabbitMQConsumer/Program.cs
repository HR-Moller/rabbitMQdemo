using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace RabbitMQConsumer
{
    public class Program
    {
        private static string _vhost = "microhost";
        private static string _hostName = "83.229.84.17";
        private static int _port = 5672;
        private static string _username = "myusername";
        private static string _password = "apollo1150";

        private static ConnectionFactory _connectionFactory = new ConnectionFactory()
        {
            VirtualHost = _vhost,
            HostName = _hostName,
            Port = _port,
            UserName = _username,
            Password = _password,

        };

        private static IConnection _connection = _connectionFactory.CreateConnection();
        private static string _exchange = "demo";
        private static string _queue = "demoQueue";
        private static IModel _channel = _connection.CreateModel();
        private static EventingBasicConsumer _consumer = new EventingBasicConsumer(_channel);


        public static void Main(string[] args)
        {
            _channel.ExchangeDeclare(_exchange, type: ExchangeType.Fanout);
            _channel.QueueDeclare(_queue, false, false, false);
            _channel.QueueBind(_queue, _exchange, "");
            string message = "";
            bool _isprinted = false;

            Console.WriteLine($"Connected to virtualhost: {_vhost}");
            Console.WriteLine($"Using echange: {_exchange}");
            Console.WriteLine($"Reading from Queue: {_queue}");

            do
            {
                _consumer.Received += (model, ea) =>
                {
                    byte[] body = ea.Body.ToArray();
                    message = Encoding.UTF8.GetString(body);

                    if (!_isprinted)
                    {
                        Console.WriteLine($"Received the folllowing message: -- {message} -- on Queue: {_queue} at {DateTime.Now}");
                        _isprinted = true;
                    }
                };

                _channel.BasicConsume(_queue, true, _consumer);
                _isprinted = false;
                //Console.ReadKey();


            }while(true);
        }

    }
}

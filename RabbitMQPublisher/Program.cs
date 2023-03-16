using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQPublisher
{
    public static class Program
    {
        private static string _vhost = "myvirtualhost";
        private static string _hostName = "myhost"; // Ip or Domain
        private static int _port = 5672;
        private static string _username = "myusername";
        private static string _password = "mypassword";
        private static ConnectionFactory _connectionFactory = new ConnectionFactory()
        {
            HostName = _hostName,
            VirtualHost = _vhost,
            Port = _port,
            UserName = _username,
            Password = _password,

        };
        private static IConnection _connection = _connectionFactory.CreateConnection();
        private static string _exchange = "demo";
        private static IModel _channel = _connection.CreateModel();
        private static EventingBasicConsumer _consumer = new EventingBasicConsumer(_channel);

        public static void Main(string[] args)
        {
            _channel.ExchangeDeclare(_exchange, type: ExchangeType.Fanout);

            Console.WriteLine($"Connected to virtualhost: {_vhost}");
            Console.WriteLine($"Using exchange: {_exchange}");

            do
            {
                
                Console.WriteLine("Enter message:");

                var message = Console.ReadLine();
                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(_exchange, "", null, body);
                Console.WriteLine($"Published message: {message} to Exchange: {_exchange}");

            }while(true);
        }
    }
}
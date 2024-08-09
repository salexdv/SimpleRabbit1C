using System;
using System.Runtime.InteropServices;
using System.Text;
using RabbitMQ.Client;


namespace SimpleRabbit1C
{

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("8BFB6E3E-3D00-4F51-8AC7-5DCBD9F0CABA")]
    [ProgId("AddIn.SimpleRabbit1C")]
    public class SimpleRabbit1C
    {

        #region "Properties"
        private IConnection connection = null;
        private IModel channel = null;

        public string HostName;
        public int Port;
        public string VirtualHost;
        public string UserName;
        public string Password;
        public string QueueName;
        #endregion

        #region "Methods"
        public SimpleRabbit1C()
        {
            HostName = "localhost";
            Port = 5672;
            VirtualHost = "/";
            UserName = "";
            Password = "";
            QueueName = "";
        }

        private bool CheckConnection()
        {
            if (connection == null)
            {
                throw new Exception("Connection is not active");
            }

            if (String.IsNullOrEmpty(QueueName))
            {
                throw new Exception("Queue is not declared");
            }
            return true;
        }

        public bool CreateConnection()
        {

            if (connection != null)
            {
                connection.Close();
            }

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = HostName;
            factory.Port = Port;
            factory.UserName = UserName;
            factory.Password = Password;
            factory.VirtualHost = VirtualHost;

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            return true;
        }

        public bool CloseConnection()
        {

            if (connection == null)
            {
                throw new Exception("Connection is not active");
            }

            channel.Close();
            channel.Dispose();
            connection.Close();
            connection.Dispose();
            return true;

        }

        public bool QueueDeclare(string name, bool durable)
        {

            if (connection == null)
            {
                throw new Exception("Connection is not active");
            }

            QueueName = channel.QueueDeclare(name, durable, false, false, null);
            return true;

        }

        public bool SendMessage(string text)
        {

            CheckConnection();
            channel.BasicPublish("", QueueName, null, Encoding.UTF8.GetBytes(text));
            return true;

        }

        public string ReceiveMessage(bool autoAck, ref ulong deliveryTag)
        {

            CheckConnection();

            BasicGetResult message = channel.BasicGet(QueueName, autoAck);
            if (message == null)
            {
                return null;
            }
            else
            {
                deliveryTag = message.DeliveryTag;
                return Encoding.UTF8.GetString(message.Body.ToArray());
            }

        }

        public bool AckMessage(ulong deliveryTag)
        {

            CheckConnection();
            channel.BasicAck(deliveryTag, false);
            return true;

        }

        public bool HasMessage()
        {
            CheckConnection();
            BasicGetResult message = channel.BasicGet(QueueName, false);
            return message != null;
        }
        #endregion

    }
}

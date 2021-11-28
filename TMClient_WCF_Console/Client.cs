using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ServiceModel;
using System.Xml;
using System.Threading;
using System.Collections.ObjectModel;

namespace TMClient_WCF_Console
{
    class Client : ITMServiceCallback
    {
        Uri address = null;
        NetTcpBinding binding = null;
        EndpointAddress endpoint = null;
        DuplexChannelFactory<ITMService> factory = null;
        ITMService channel = null;
        User user = null;
        public bool checkConnection = false;
        InstanceContext context = null;

        public void SomeWork()
        {
            
            #region OutData
            Console.WriteLine("User " + user.Name + "  " + user.Host);
            #endregion

            #region One
            ObservableCollection<Task> tasks = channel.GetTasks();

            foreach (Task task in tasks)
            {
                Console.WriteLine("Task " + task.Guid);
            }

            int index = new Random().Next(0, 1000);
            tasks[index].User = user;

            Thread.Sleep(5000);

            Console.WriteLine(Environment.NewLine + index + " " + tasks[index].Guid);
            Console.WriteLine(Environment.NewLine);

            // Call callback
            channel.ChangeTask(tasks[index]);

            #endregion

        }

        public void Init()
        {
            //address = new Uri("net.tcp://192.168.0.162:4004/ITMService");
            address = new Uri("net.tcp://localhost:4004/ITMService");
            binding = new NetTcpBinding();
            endpoint = new EndpointAddress(address);
            context = new InstanceContext(this);

            #region Binding
            // Возвращает или задает интервал времени для закрытия подключения до того, как транспорт создаст исключение.
            binding.CloseTimeout = TimeSpan.FromMinutes(10);
            // Возвращает или задает интервал времени для открытия подключения до того, как транспорт создаст исключение.
            binding.OpenTimeout = TimeSpan.FromMinutes(10);

            binding.ListenBacklog = 10;
            binding.MaxConnections = 10;


            // Получает или задает максимальный допустимый размер (в байтах) буферного пула, в котором хранятся сообщения TCP, обработанные привязкой.
            binding.MaxBufferPoolSize = 524888;
            //Получает или задает максимальный размер (в байтах) полученного сообщения, обрабатываемого привязкой.
            binding.MaxReceivedMessageSize = 2147483647;

            binding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;

            //binding.ReaderQuotas.MaxArrayLength = 2147483647;
            //binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            //binding.ReaderQuotas.MaxStringContentLength = 2147483647;

            binding.ReceiveTimeout = TimeSpan.MaxValue;
            binding.SendTimeout = TimeSpan.FromMinutes(10f);

            // NetTcpBinding
            // CloseTimeout - Возвращает или задает интервал времени для закрытия подключения до того, как транспорт создаст исключение.(Унаследовано от Binding)
            // EnvelopeVersion - Возвращает версию протокола SOAP, используемого для сообщений, обрабатываемых этой привязкой.
            // MaxBufferPoolSize - Получает или задает максимальный допустимый размер(в байтах) буферного пула, в котором хранятся сообщения TCP, обработанные привязкой.
            // MaxBufferSize - Возвращает или задает значение, указывающее максимальный размер буфера, используемого для хранения сообщений в памяти(в байтах).
            // MaxReceivedMessageSize - Получает или задает максимальный размер(в байтах) полученного сообщения, обрабатываемого привязкой.
            // MessageVersion - Возвращает версию сообщения, используемую клиентами и службами, настроенными с использованием привязки.(Унаследовано от Binding)
            // Name - Возвращает или задает имя привязки.(Унаследовано от Binding)
            // Namespace - Возвращает или задает пространство имен XML привязки.(Унаследовано от Binding)
            // OpenTimeout - Возвращает или задает интервал времени для открытия подключения до того, как транспорт создаст исключение.(Унаследовано от Binding)
            // ReaderQuotas - Возвращает или задает ограничения по сложности сообщений SOAP, которые могут обрабатываться конечными точками, настроенными с этой привязкой.
            // ReceiveTimeout - Возвращает или задает интервал времени бездействия подключения, в течение которого сообщения приложения не получаются, до его сброса.(Унаследовано от Binding)
            // Scheme - Возвращает схему универсального кода ресурса (URI)для транспорта.
            // Security - Возвращает объект, указывающий тип безопасности, который используется со службами, настроенными с этой привязкой.
            // SendTimeout - Возвращает или задает интервал времени для завершения операции записи до того, как транспорт создаст исключение.(Унаследовано от Binding)
            // TransferMode - Возвращает или задает значение, которое определяет, используется ли в службе, настроенной с помощью привязки, потоковый или буферизованный режим передачи сообщений(или оба режима).

            // ReaderQuotas
            // Max - Возвращает экземпляр данного класса с максимальными значениями для всех свойств.
            // MaxArrayLength - Получает или задает максимально допустимую длину массива.
            // MaxBytesPerRead - Получает или задает максимально допустимое число байтов, возвращаемых для каждой операции чтения.
            // MaxDepth - Получает или задает максимальную глубину вложенного узла.
            // MaxNameTableCharCount - Получает или задает максимальное количество символов в имени таблицы.
            // MaxStringContentLength - Получает или задает максимальную длину строки, возвращаемую модулем чтения.
            #endregion
        }

        public bool Connect()
        {
            Thread.Sleep(5000);
            
            try
            {
                factory = new DuplexChannelFactory<ITMService>(context, binding, endpoint);
                channel = factory.CreateChannel();

                user = new User()
                {
                    Name = Environment.UserName,
                    Host = Dns.GetHostName(),
                    Description = "",
                    OCtx = null,
                    Guid = new Guid()
                };

                user.Guid = channel.Connect(user);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void Disconnect()
        {
            if(channel != null)
            {
                if(user != null)
                {
                    channel.Disconnect(user.Guid);
                    Console.WriteLine("Call Disconnect " + user.Guid.ToString());
                }
                    

                factory = null;
                channel = null;

                #region OutData
                Console.WriteLine("Call Disconnect");
                #endregion
            }

        }

        // Callback
        public void NotifyChangeTaskCallback(Task task)
        {
            Console.WriteLine("NotifyChangeTaskCallback  " + task.Title + "  " + task.Guid);
        }

        public void SendMessageCallback(string msg)
        {
            Console.WriteLine("SendMessageCallback  " + msg);
        }
    }
}

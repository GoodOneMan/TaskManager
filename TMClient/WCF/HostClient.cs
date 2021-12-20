using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Net;
using System.Xml;
using TMStructure;
using System.Threading;
using System.Collections.ObjectModel;
using TMClient.MVVM.Model;

namespace TMClient.WCF
{
    class HostClient : IContract_Callback
    {
        Uri address = null;
        NetTcpBinding binding = null;
        DuplexChannelFactory<IContract_Service> factory = null;
        IContract_Service channel = null;
        InstanceContext context = null;

        Storage Storage = null;

        private static HostClient instance = null;
        public static HostClient GetClient()
        {
            if (instance == null)
                instance = new HostClient();

            return instance;
        }

        private HostClient()
        {
            InitClient();
            Storage = Storage.GetStorage();
        }

        private void InitClient()
        {
            #region Address
            address = new Uri("net.tcp://localhost:4004/IContract_Service");
            //address = new Uri("net.tcp://192.168.0.162:4004/IContract_Service");
            #endregion

            #region Binding Contract
            binding = new NetTcpBinding();
            binding.CloseTimeout = TimeSpan.FromMinutes(10); // Возвращает или задает интервал времени для закрытия подключения до того, как транспорт создаст исключение.
            binding.OpenTimeout = TimeSpan.FromMinutes(10); // Возвращает или задает интервал времени для открытия подключения до того, как транспорт создаст исключение.
            binding.ListenBacklog = 10;
            binding.MaxConnections = 10;
            binding.MaxBufferPoolSize = 524888; // Получает или задает максимальный допустимый размер (в байтах) буферного пула, в котором хранятся сообщения TCP, обработанные привязкой.
            binding.MaxReceivedMessageSize = 2147483647; //Получает или задает максимальный размер (в байтах) полученного сообщения, обрабатываемого привязкой.

            binding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;

            //binding.ReaderQuotas.MaxArrayLength = 2147483647;
            //binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            //binding.ReaderQuotas.MaxStringContentLength = 2147483647;

            binding.ReceiveTimeout = TimeSpan.MaxValue;
            binding.SendTimeout = TimeSpan.FromMinutes(10f);

            #region Help
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

            #endregion

            context = new InstanceContext(this);
            factory = new DuplexChannelFactory<IContract_Service>(context, binding, new EndpointAddress(address));
            channel = factory.CreateChannel();
        }

        #region IContract_Service
        public bool Connect()
        {
            try
            {

                //Storage.DispatcherUI.Invoke(() => Storage.CurrentUser = channel.Connect(Environment.UserName, Dns.GetHostName()));
                Storage.CurrentUser = channel.Connect(Environment.UserName, Dns.GetHostName());
                return true;
            }
            catch (Exception e)
            {
                Thread.Sleep(1000);
                System.Windows.MessageBox.Show(e.Message + Environment.NewLine + " Возможно следует перезагрузить компьютер");

                //Connect();

                return false;
            }
        }

        public bool Desconnect()
        {
            if (channel != null)
            {
                if (Storage.CurrentUser != null)
                    channel.Disconnect(Storage.CurrentUser.Guid);

                factory = null;
                channel = null;

                return true;
            }

            return false;
        }
        #endregion

        #region IContract_Callback
        public ObservableCollection<Task> GetTasks()
        {
            return channel.GetTasks();
        }
        public void SendTask(Task task)
        {
            channel.SetTask(Storage.CurrentUser.Guid, task);
        }
        public void SendTasks(ObservableCollection<Task> tasks)
        {
            channel.SetTasks(Storage.CurrentUser.Guid, tasks);
        }
        #endregion

        #region IContract_Callback
        public void ContractCallback_Task(User User, Task task)
        {
            //int index = Storage.GetStorage().Tasks.IndexOf(Storage.GetStorage().Tasks.FirstOrDefault(item => item.Guid == task.Guid));
            //if(index != -1)
            //{
            //    Storage.GetStorage().Tasks[index] = task;
            //}
            //else
            //{
            //    Storage.GetStorage().Tasks.Add(task);
            //}
            //Storage.NotifyObservers();
            Storage.ImplementTask(task);
        }
        public void ContractCallback_AllTasks(User User, ObservableCollection<Task> tasks)
        {
            Storage.GetStorage().Tasks = tasks;
        }
        #endregion
    }
}

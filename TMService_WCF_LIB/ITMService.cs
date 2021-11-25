using System;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace TMService_WCF_LIB
{
    [ServiceContract(CallbackContract = typeof(ITMServiceCallback))]
    public interface ITMService
    {
        [OperationContract]
        User Connect();

        [OperationContract]
        bool Disconnect(Guid guid);

        [OperationContract]
        ObservableCollection<Task> GetTasks();

        [OperationContract]
        void ChangeTask(Task task);
    }

    public interface ITMServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void NotifyChangeTaskCallback(Task task);
        [OperationContract]
        void SendMessageCallback(string msg);
    }
}

using System;
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
        Task[] GetTasks();

        [OperationContract]
        void ChangeTask(Task task);
    }

    public interface ITMServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void NotifyChangeTaskCallback(Task task);
    }
}

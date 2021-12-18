using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using TMStructure;

namespace TMService.WCF
{
    [ServiceContract(CallbackContract = typeof(IContract_Callback))]
    public interface IContract_Service
    {
        [OperationContract]
        User Connect(string UserName, string UserHost);

        [OperationContract]
        bool Disconnect(Guid UserGuid);

        [OperationContract]
        ObservableCollection<Task> GetTasks();

        [OperationContract]
        Task GetTask(Guid TaskGuid);

        [OperationContract]
        bool SetTask(Guid UserGuid, Task Task);

        [OperationContract]
        bool SetTasks(Guid UserGuid, ObservableCollection<Task> Tasks);
    }

    public interface IContract_Callback
    {
        [OperationContract(IsOneWay = true)]
        void ContractCallback_Task(User User, Task Task);

        [OperationContract(IsOneWay = true)]
        void ContractCallback_AllTasks(User User, ObservableCollection<Task> Tasks);
    }
}

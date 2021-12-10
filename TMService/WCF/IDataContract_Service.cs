using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using TMStructure;

namespace TMService.WCF
{
    [ServiceContract(CallbackContract = typeof(IDataContract_Callback))]
    public interface IDataContract_Service
    {
        [OperationContract]
        ObservableCollection<Task> GetTasks();

        [OperationContract]
        Task GetTask(Guid guid);

        [OperationContract]
        bool SetTask(Task task);
        [OperationContract]
        bool SetTasks(ObservableCollection<Task> tasks);
    }

    public interface IDataContract_Callback
    {
        [OperationContract(IsOneWay = true)]
        void DataContractCallback_Task(string msg, Task task);
        [OperationContract(IsOneWay = true)]
        void DataContractCallback_AllTasks(string msg, ObservableCollection<Task> tasks);
    }
}

using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using TMServer_WPF.CORE;

namespace TMServer_WPF.WCF
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
    }

    public interface IDataContract_Callback
    {
        [OperationContract(IsOneWay = true)]
        void DataContractCallback(string msg);
    }
}

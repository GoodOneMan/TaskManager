using System;
using System.ServiceModel;
using TMClient_WPF.CORE;

namespace TMClient_WPF.WCF
{
    [ServiceContract(CallbackContract = typeof(IContract_Callback))]
    public interface IContract_Service
    {
        [OperationContract]
        User Connect(string name, string host);

        [OperationContract]
        bool Disconnect(Guid guid);
    }

    public interface IContract_Callback
    {
        [OperationContract(IsOneWay = true)]
        void ContractCallback(string msg);
    }
}

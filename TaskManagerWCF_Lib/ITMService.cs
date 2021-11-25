using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Net;
using TaskManagerWCF_Lib.Struct;

namespace TaskManagerWCF_Lib
{
    [ServiceContract(CallbackContract = typeof(ITMServiceCallback))]
    public interface ITMService
    {
        [OperationContract]
        Guid Connect(UserStruct user);
        [OperationContract]
        void Disconnect(Guid guid);
        [OperationContract]
        TaskStruct[] GetTasks();
        [OperationContract]
        void UpdataTasks(TaskStruct task);
    }

    public interface ITMServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void NotifyUpdataTasksCallback(TaskStruct task);
    }
}

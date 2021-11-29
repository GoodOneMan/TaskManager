using System;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace TMService_WCF_LIB
{
    [ServiceContract(CallbackContract = typeof(ITMServiceCallback))]
    public interface ITMService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Guid Connect(User user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [OperationContract]
        bool Disconnect(Guid guid);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ObservableCollection<Task> GetTasks();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        [OperationContract]
        void ChangeTask(Task task);
    }

    public interface ITMServiceCallback
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        //[OperationContract(IsOneWay = true)]
        [OperationContract]
        void NotifyChangeTaskCallback(Task task);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        [OperationContract]
        void SendMessageCallback(string msg);
    }
}

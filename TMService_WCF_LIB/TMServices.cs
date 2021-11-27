using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TMService_WCF_LIB
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class TMServices : ITMService
    {
        #region Property
        /// <summary>
        /// 
        /// </summary>
        private static List<User> Users = new List<User>();
        /// <summary>
        /// 
        /// </summary>
        private static ObservableCollection<Task> Tasks = new ObservableCollection<Task>();
        #endregion

        #region Method
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Guid Connect(User _user)
        {
            //var user = Users.FirstOrDefault(item => item.Name == _user.Name && item.Host == _user.Host);
            User user = null;
            if (user != null)
            {
                if (user.OCtx == null)
                    user.OCtx = OperationContext.Current;

                // Output console
                Console.WriteLine("Exist user " + user.Host);
                return user.Guid;
            }
            else
            {
                _user.Guid = Guid.NewGuid();
                _user.OCtx = OperationContext.Current;
                Users.Add(_user);

                // Output console
                Console.WriteLine("Add user " + _user.Host);
                return _user.Guid;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool Disconnect(Guid guid)
        {
            var user = Users.FirstOrDefault(u => u.Guid == guid);
            if (user != null)
            {
                // Output console
                Console.WriteLine("Remove user " + user.Host);

                Users.Remove(user);
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Task> GetTasks()
        {
            Tasks = Tests.Datas.GetTasks();

            return Tasks;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public void ChangeTask(Task _task)
        {
            if (_task != null)
            {
                Task task = Tasks.FirstOrDefault(item => item.Guid == _task.Guid);
                task = _task;

                SendMassage("Task " + task.Guid + " change");
                NotifyChangeTask(task);
            }
            else{
                SendMassage("Task not change, task null");
            }
        }
        #endregion

        #region Callback Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public void NotifyChangeTask(Task task)
        {
            foreach(User user in Users)
            {
                user.OCtx.GetCallbackChannel<ITMServiceCallback>().NotifyChangeTaskCallback(task);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void SendMassage(string msg)
        {
            foreach (User user in Users)
            {
                user.OCtx.GetCallbackChannel<ITMServiceCallback>().SendMessageCallback(msg);
            }
        }
        #endregion
    }
}

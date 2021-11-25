using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TMService_WCF_LIB
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TMServices : ITMService
    {
        #region Property
        private static List<User> Users = new List<User>();
        private static ObservableCollection<Task> Tasks = new ObservableCollection<Task>();
        #endregion

        #region Method
        public User Connect()
        {
            User user = new User();

            user.Guid = Guid.NewGuid();
            user.Name = Environment.UserName;
            user.Host = System.Net.Dns.GetHostName();
            user.Description = "";
            user.OCtx = OperationContext.Current;

            Users.Add(user);

            return user;
        }

        public bool Disconnect(Guid guid)
        {
            var user = Users.FirstOrDefault(u => u.Guid == guid);
            if (user != null)
            {
                Users.Remove(user);
                return true;
            }
            else
                return false;
        }

        public ObservableCollection<Task> GetTasks()
        {
            Tasks = Tests.Datas.GetTasks();

            return Tasks;
        }

        public void ChangeTask(Task task)
        {
            SendMassage("Massage Task " + task.Guid.ToString());
            SendMassage("Massage User " + task.User.Guid.ToString());

            var t = Tasks.FirstOrDefault(item => item.Guid == task.Guid);
            if (t != null)
            {
                t = task;
                // Call callback
                NotifyChangeTask(task);
            }
        }
        #endregion

        #region Callback Method
        public void NotifyChangeTask(Task task)
        {
            foreach(User user in Users)
            {
                user.OCtx.GetCallbackChannel<ITMServiceCallback>().NotifyChangeTaskCallback(task);
            }
        }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TMService_WCF_LIB
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class TMServices : ITMService
    {
        #region Property
        private static List<User> Users = new List<User>();
        private static List<Task> Tasks = new List<Task>();
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
         
        public Task[] GetTasks()
        {
            Tasks = Tests.Datas.GetTasks();

            return Tasks.ToArray();
        }

        public void ChangeTask(Task task)
        {
            var t = Tasks.FirstOrDefault(item => item.Guid == task.Guid);
            if (t != null)
            {
                t = task;
                // Call callback
                NotifyChangeTask(task);
            }
        }
        #endregion

        // ITMServiceCallback
        public void NotifyChangeTask(Task task)
        {
            foreach(User user in Users)
            {
                user.OCtx.GetCallbackChannel<ITMServiceCallback>().NotifyChangeTaskCallback(task);
            }
        }
    }
}

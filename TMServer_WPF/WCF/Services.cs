using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using TMServer_WPF.CORE;
using TMServer_WPF.MVVM.Model;

namespace TMServer_WPF.WCF
{
    #region Events class
    public class UserChangedEventArgs : EventArgs
    {
        public readonly string Message;
        public readonly User User;

        public UserChangedEventArgs(string message, User user)
        {
            Message = message;
            User = user;
        }
    }

    public class TaskChangedEventArgs : EventArgs
    {
        public readonly string Message;
        public readonly Task Task;

        public TaskChangedEventArgs(string message, Task task)
        {
            Message = message;
            Task = task;
        }
    }
    #endregion

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    class Services : IContract_Service, IDataContract_Service, IObserver
    {
        #region Events
        // User
        public static event EventHandler<UserChangedEventArgs> UserChanged;
        protected virtual void OnUserChanged(UserChangedEventArgs e)
        {
            if (UserChanged != null) UserChanged(this, e);
        }

        // Task
        public static event EventHandler<TaskChangedEventArgs> TaskChanged;
        protected virtual void OnTaskChanged(TaskChangedEventArgs e)
        {
            if (TaskChanged != null) TaskChanged(this, e);
        }
        #endregion

        #region Internal method
        Storage Storage = Storage.GetStorage();
        public Services(){
            Storage.AddObserver(this);
        }
        #endregion

        #region IContract_Service
        public User Connect(string name, string host)
        {
            User user = Storage.Users.FirstOrDefault(item => item.Host == host && item.Name == name);

            if(user == null)
            {
                user = new User();

                //Add data in user [Name, Host, Description, Guid, OCtx]
                user.Name = name;
                user.Host = host;
                user.Description = GetDescription(host);
                user.Guid = Guid.NewGuid();
                user.OCtx = OperationContext.Current;

                Storage.Users.Add(user);
                // Call event
                OnUserChanged(new UserChangedEventArgs("пользователь подключился ", user));
            }
            else
            {
                // Call event
                OnUserChanged(new UserChangedEventArgs("пользователь уже подключин ", user));
            }
            return user;
        }

        public bool Disconnect(Guid guid)
        {
            User user = Storage.Users.FirstOrDefault(item => item.Guid == guid);
            if(user != null)
            {
                Storage.Users.Remove(user);
                // Call event
                OnUserChanged(new UserChangedEventArgs("пользователь отключился ", user));
                return true;
            }
            return false;
        }

        // Call ComputersInLocalNetwork.GetServerList and finde records 
        private string GetDescription(string host)
        {
            Dictionary<string, string> hosts = Storage.Hosts;
            KeyValuePair<string, string> h = hosts.FirstOrDefault(item => item.Key == host);
            if (h.Key != null)
                return h.Value;
            else
                return "unknown";
        }
        #endregion

        #region IContract_Callback
        private void Contract_Callback(string msg)
        {
            foreach(User user in Storage.Users)
            {
                user.OCtx.GetCallbackChannel<IContract_Callback>().ContractCallback(msg);
            }
        }
        #endregion

        #region IDataContract_Service
        public CORE.Task GetTask(Guid guid)
        {
            return Storage.Tasks.FirstOrDefault(item => item.Guid == guid);
        }

        public ObservableCollection<CORE.Task> GetTasks()
        {
            return Storage.Tasks;
        }

        public bool SetTask(CORE.Task task)
        {
            var t = Storage.Tasks.FirstOrDefault(iten => iten.Guid == task.Guid);
            if (t == null)
                return false;

            t = task;

            // Call event
            OnTaskChanged(new TaskChangedEventArgs("задача обновлена ", task));

            // Call callback
            string msg = "пользователь " + task.User.Name + " " 
                + task.User.Description + " обновил задачу " 
                + task.Title + " " + task.Guid;

            DataContract_Callback(msg, task);

            return true;
        }
        #endregion

        #region IDataContract_Callback
        private void DataContract_Callback(string msg, Task task)
        {
            foreach(User user in Storage.Users)
            {
                user.OCtx.GetCallbackChannel<IDataContract_Callback>().DataContractCallback(msg, task);
            }
        }
        #endregion

        #region IObserver
        public void UpdateProperty(Type type)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

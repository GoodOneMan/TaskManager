using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using TMService.CORE;
using TMService.MVVM.Model;
using TMStructure;

namespace TMService.WCF
{

    #region Events class
    public class LogChangedEventArgs : EventArgs
    {
        public readonly string Message;

        public LogChangedEventArgs(string message)
        {
            Message = message;
        }
    }
    #endregion

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant, IncludeExceptionDetailInFaults = true, UseSynchronizationContext = false)]
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    class Services : IContract_Service
    {
        #region Events
        // Log
        public static event EventHandler<LogChangedEventArgs> LogChanged;
        public virtual void OnLogChanged(LogChangedEventArgs e)
        {
            if (LogChanged != null) LogChanged(this, e);
        }
        #endregion

        Storage Storage = null;
        
        public Services()
        {
            Storage = Storage.GetStorage();
        }

        #region IContract_Service
        public User Connect(string UserName, string UserHost)
        {
            User user = Storage.Users.FirstOrDefault(item => item.Name == UserName && item.Host == UserHost);

            if (user == null)
            {
                user = new User();
                user.Name = UserName;
                user.Host = UserHost;
                user.Description = Storage.Hosts.FirstOrDefault(item => item.Key == UserHost).Value;
                user.Guid = Guid.NewGuid();
                user.OCtx = OperationContext.Current;

                Storage.DispatcherUI.Invoke(()=> { Storage.Users.Add(user); });

                OnLogChanged(new LogChangedEventArgs(String.Format("новый пользователь {0} подключился в {1}", user.Description, DateTime.Now.ToString())));
            }
            else
            {
                user.OCtx = OperationContext.Current;
                OnLogChanged(new LogChangedEventArgs(String.Format("пользователь {0} {1} найден ", user.Description, user.Name)));
            }

            Storage.TaskChanged += ServiceEvent_TaskChanged;
            Storage.TasksChanged += ServiceEvent_TasksChanged;
            
            return user;
        }
        // User guid
        public bool Disconnect(Guid UserGuid)
        {
            Storage.TaskChanged -= ServiceEvent_TaskChanged;
            Storage.TasksChanged -= ServiceEvent_TasksChanged;

            User user = Storage.Users.FirstOrDefault(item => item.Guid == UserGuid);
            if (user != null)
            {
                OnLogChanged(new LogChangedEventArgs(String.Format("пользователь {0} {1} отключился", user.Description, user.Name)));
                return true;
            }
            return false;
        }
        // Task guid
        public Task GetTask(Guid TaskGuid)
        {
            return Storage.Tasks.FirstOrDefault(item => item.Guid == TaskGuid);
        }
        // 
        public ObservableCollection<Task> GetTasks()
        {
            return Storage.Tasks;
        }
        // User guid
        public bool SetTask(Guid UserGuid, Task task)
        {
            if (task == null)
                return false;

            Storage.DispatcherUI.Invoke(() =>
            {
                int index = Storage.Tasks.IndexOf(Storage.Tasks.FirstOrDefault(iten => iten.Guid == task.Guid));
                Storage.Tasks[index] = task;
                Storage.NotifyObservers();
            });

            Callback_Task(UserGuid, Storage.Task);

            return true;
        }
        // User guid
        public bool SetTasks(Guid UserGuid, ObservableCollection<Task> tasks)
        {
            if (tasks == null)
                return false;

            Storage.Tasks = tasks;

            Callback_AllTasks(UserGuid, Storage.Tasks);

            return true;
        }
        #endregion

        #region IContract_Callback
        private void Callback_Task(Guid UserGuid, Task task)
        {
            User CallbackUser = Storage.Users.FirstOrDefault(item => item.Guid == UserGuid);
            foreach (User user in Storage.Users)
            {
                if(user.Guid != UserGuid)
                {
                    try
                    {
                        user.OCtx.GetCallbackChannel<IContract_Callback>().ContractCallback_Task(CallbackUser, Storage.Task);
                    }
                    catch (Exception ex) { }
                }
            }
        }

        private void Callback_AllTasks(Guid UserGuid, ObservableCollection<Task> tasks)
        {
            User CallbackUser = Storage.Users.FirstOrDefault(item => item.Guid == UserGuid);
            foreach (User user in Storage.Users)
            {
                if (user.Guid != UserGuid)
                {
                    try
                    {
                        user.OCtx.GetCallbackChannel<IContract_Callback>().ContractCallback_AllTasks(CallbackUser, Storage.Tasks);
                    }
                    catch (Exception ex) { }
                }
            }
        }
        #endregion

        #region Event changed task
        public void ServiceEvent_TaskChanged(object sender, TaskChangedEventArgs e)
        {
            if(e.User == null)
                Callback_Task(Storage.CurrentUser.Guid, e.Task);
            else
                Callback_Task(e.User.Guid, e.Task);
        }
        public void ServiceEvent_TasksChanged(object sender, TasksChangedEventArgs e)
        {
            if (e.User == null)
                Callback_AllTasks(Storage.CurrentUser.Guid, e.Tasks);
            else
                Callback_AllTasks(e.User.Guid, e.Tasks);
        }
        #endregion
    }
}

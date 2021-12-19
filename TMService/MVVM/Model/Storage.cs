using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using TMService.CORE;
using TMService.WCF;
using TMStructure;

namespace TMService.MVVM.Model
{
    #region Events class
    public class TasksChangedEventArgs : EventArgs
    {
        public readonly User User;
        public readonly ObservableCollection<Task> Tasks;

        public TasksChangedEventArgs(User user, ObservableCollection<Task> tasks)
        {
            User = user;
            Tasks = tasks;
        }
    }

    public class TaskChangedEventArgs : EventArgs
    {
        public readonly User User;
        public readonly Task Task;

        public TaskChangedEventArgs(User user, Task task)
        {
            User = user;
            Task = task;
        }
    }
    #endregion


    class Storage : IObservable
    {
        #region Events
        // Tasks
        public event EventHandler<TasksChangedEventArgs> TasksChanged;
        public virtual void OnTasksChanged(TasksChangedEventArgs e)
        {
            if (TasksChanged != null) TasksChanged(this, e);
        }

        // Task
        public event EventHandler<TaskChangedEventArgs> TaskChanged;
        public virtual void OnTaskChanged(TaskChangedEventArgs e)
        {
            if (TaskChanged != null) TaskChanged(this, e);
        }
        #endregion

        #region IObservable
        private List<IObserver> observers = new List<IObserver>();

        public void AddObserver(IObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(IObserver o)
        {
            observers.Remove(o);
        }

        public void NotifyObservers()
        {
            foreach (IObserver observer in observers)
                observer.UpdateProperty();
        }
        #endregion

        #region Property
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyObservers();
            }
        }

        private Task _task;
        public Task Task
        {
            get { return _task; }
            set
            {
                _task = value;
                //_task = BlockedTask(_task);
                NotifyObservers();
            }
        }

        private ObservableCollection<Task> _tasks;
        public ObservableCollection<Task> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                BlockedTasks(_tasks);
                NotifyObservers();
            }
        }

        private ObservableCollection<string> _log;
        public ObservableCollection<string> Log
        {
            get { return _log; }
            set
            {
                _log = value;
                NotifyObservers();
            }
        }
        #endregion

        public User CurrentUser;
        public Dictionary<string, string> Hosts;
        public System.Windows.Threading.Dispatcher DispatcherUI = null;
        
        private static Storage instance = null;
        public static Storage GetStorage()
        {
            if (instance == null)
                instance = new Storage();

            return instance;
        }

        private Storage() {
            CurrentUser = new User()
            {
                Name = Environment.UserName,
                Host = "Service host",
                Description = "Current",
                Guid = Guid.NewGuid(),
                OCtx = null
            };
            DispatcherUI = System.Windows.Application.Current.Dispatcher;
            Services.LogChanged += ServiceEvent_LogChanged;
        }

        public void GetState()
        {
            DataBase.GetDB().Connection();

            Tasks = DataBase.GetDB().GetT();
            Users = DataBase.GetDB().GetU();

            //if (Users.FirstOrDefault(item => item.Name == CurrentUser.Name && item.Host == CurrentUser.Host) == null)
            //    Users.Add(CurrentUser);

            User User = Users.FirstOrDefault(item => item.Name == CurrentUser.Name && item.Host == CurrentUser.Host);
            if(User == null)
            {
                CurrentUser = new User()
                {
                    Name = Environment.UserName,
                    Host = "Service host",
                    Description = "Current",
                    Guid = Guid.NewGuid(),
                    OCtx = null
                };

                Users.Add(CurrentUser);
            }
            else
            {
                CurrentUser = User;
            }


            Log = new ObservableCollection<string>();
            Hosts = new Dictionary<string, string>();
        }

        public bool SetState()
        {
            bool flag = false;

            DataBase.GetDB().Disconnection(Users, Tasks);
            Tasks = new ObservableCollection<Task>();
            Users = new ObservableCollection<User>();

            return flag;
        }

        public void ImplementTask(Task task)
        {
            int index = Tasks.IndexOf(Tasks.FirstOrDefault(iten => iten.Guid == task.Guid));
            if(index != -1)
            {
                Tasks[index] = BlockedTask(task);
            }
            else
            {
                Tasks.Add(BlockedTask(task));
            }
            
            NotifyObservers();
        }

        public void ServiceEvent_LogChanged(object sender, LogChangedEventArgs e)
        {
            DispatcherUI.Invoke(() => Log.Add(e.Message));
        }

        #region Blocked
        private ObservableCollection<Task> BlockedTasks(ObservableCollection<Task> collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i] = BlockedTask(collection[i]);
            }

            return collection;
        }

        private Task BlockedTask(Task task)
        {
            // Enable
            if (task.IsChecked && task.BlockedUser != null)
                if (task.BlockedUser.Guid == CurrentUser.Guid)
                    task.Enable = true;
                else
                    task.Enable = false;
            else
                task.Enable = true;

            // Edit enable
            if (CurrentUser.Guid == task.User.Guid)
                task.EditEnable = true;
            else
                task.EditEnable = false;

            return task;
        }
        #endregion
    }
}

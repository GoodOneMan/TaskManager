using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TMService.CORE;
using TMStructure;

namespace TMService.MVVM.Model
{
    class Storage: IObservable
    {
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

        public void NotifyObservers(Type type)
        {
            foreach (IObserver observer in observers)
                observer.UpdateProperty(type);
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
                NotifyObservers(typeof(ObservableCollection<User>));
            }
        }

        private ObservableCollection<Task> _tasks;
        public ObservableCollection<Task> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                NotifyObservers(typeof(ObservableCollection<Task>));
            }
        }

        private Task _task;
        public Task Task
        {
            get { return _task; }
            set
            {
                _task = value;
                NotifyObservers(typeof(Task));
            }
        }
        
        private ObservableCollection<string> _log;
        public ObservableCollection<string> Log
        {
            get { return _log; }
            set
            {
                _log = value;
                NotifyObservers(typeof(ObservableCollection<string>));
            }
        }
        #endregion

        public Dictionary<string, string> Hosts;

        private static Storage instance = null;
        public static Storage GetStorage()
        {
            if (instance == null)
                instance = new Storage();

            return instance;
        }

        public Storage() {

        }

        public void GetAllData()
        {
            Tasks = DataBase.GetDB().GetAllTasks();
            Users = DataBase.GetDB().GetAllUsers();
            Log = new ObservableCollection<string>();
            Hosts = new Dictionary<string, string>();
        }
        
    }
}

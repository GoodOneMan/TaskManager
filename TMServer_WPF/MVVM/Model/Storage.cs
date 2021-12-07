using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using TMServer_WPF.CORE;
using TMServer_WPF.WCF;

namespace TMServer_WPF.MVVM.Model
{
    #region Storage
    class Storage : BaseViewModel, IObservable
    {
        #region IObservable
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

        #region Events

        #endregion

        private static Storage instance = null;
        public static Storage GetStorage()
        {
            if (instance == null)
                instance = new Storage();

            return instance;
        }
        private SQLite_Model db = null;

        private Storage()
        {
            observers = new List<IObserver>();
        }

        public void Init()
        {
            db = SQLite_Model.GetDB();

            Hosts = ComputersInLocalNetwork.GetServerList(ComputersInLocalNetwork.SV_101_TYPES.SV_TYPE_ALL);

            Users = db.GetAllUsers();
            Tasks = db.GetAllTasks();
        }

        private List<IObserver> observers;
        public Dictionary<string, string> Hosts;

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyObservers(typeof(User));
            }
        }

        private ObservableCollection<Task> _tasks;
        public ObservableCollection<Task> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                NotifyObservers(typeof(Task));
            }
        }

        private Task _selectTask;
        public Task SelectTask
        {
            get { return _selectTask; }
            set
            {
                _selectTask = value;
                NotifyObservers(typeof(Task));
            }
        }
    }
    #endregion
}

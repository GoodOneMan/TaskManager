using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TMClient.CORE;
using TMClient.WCF;
using TMStructure;

namespace TMClient.MVVM.Model
{
    class Storage : IObservable
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
        #endregion

        public HostClient HostClient = null;
        private static Storage instance = null;
        public static Storage GetStorage()
        {
            if (instance == null)
                instance = new Storage();

            return instance;
        }
        
        public Storage()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TMClient_WPF.CORE;
//susing System.Threading.Tasks;

namespace TMClient_WPF.MVVM.Model
{
    interface IObservable
    {
        void AddObserver(IObserver o);
        void RemoveObserver(IObserver o);
        void NotifyObservers(Type type);
    }

    interface IObserver
    {
        void UpdateProperty(Type type);
    }

    class Storage : IObservable
    {
        #region IObservable
        private List<IObserver> observers;

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

        private static Storage instance = null;
        public static Storage GetStorage()
        {
            if (instance == null)
                instance = new Storage();

            return instance;
        }

        private Storage()
        {
            observers = new List<IObserver>();
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
}

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

        public void NotifyObservers()
        {
            foreach (IObserver observer in observers)
                observer.UpdateProperty();
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
                BlockedTasks(_tasks);
                NotifyObservers();
            }
        }
        #endregion

        public User CurrentUser;
        public Task Task;

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

        public void ImplementTask(Task task)
        {
            int index = Tasks.IndexOf(Tasks.FirstOrDefault(iten => iten.Guid == task.Guid));
            Tasks[index] = task;
            Task = null;
            NotifyObservers();
        }

        private ObservableCollection<Task> BlockedTasks(ObservableCollection<Task> collection)
        {
            foreach (Task task in collection)
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
            }

            return collection;
        }
        
    }
}

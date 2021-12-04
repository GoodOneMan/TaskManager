using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using TMServer_WPF.WCF;
using TMServer_WPF.MVVM.Model;

namespace TMServer_WPF.CORE
{
    #region Data structures class
    // User
    [DataContract(Namespace = "TManager")]
    public class User
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Host { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public Guid Guid { get; set; }

        public OperationContext OCtx { get; set; }
    }

    // Task
    [DataContract(Namespace = "TManager")]
    public class Task
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public Guid Guid { get; set; }
        [DataMember]
        public ObservableCollection<Comment> Comments { get; set; }
        [DataMember]
        public bool IsChecked { get; set; }
        [DataMember]
        public bool State { get; set; }
        [DataMember]
        public string Hint { get; set; }
        [DataMember]
        public User User { get; set; }
    }

    // Comment
    [DataContract(Namespace = "TManager")]
    public class Comment
    {
        [DataMember]
        public User User { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public Guid TaskGuid { get; set; }
        [DataMember]
        public Guid Guid { get; set; }
    }
    #endregion

    #region Events class

    #endregion

    #region Storage
    class Storage : IObservable
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

        public void NotifyObservers()
        {
            foreach (IObserver observer in observers)
                observer.UpdateProperty();
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
            db = SQLite_Model.GetDB();
            Users = Tests.Datas_Test.GetUsers();
            Tasks = Tests.Datas_Test.GetTasks();

            Hosts = ComputersInLocalNetwork.GetServerList(ComputersInLocalNetwork.SV_101_TYPES.SV_TYPE_ALL);
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
                NotifyObservers();
            }
        }
    }
    #endregion
}

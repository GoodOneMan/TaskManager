using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using TMServer_WPF.WCF;

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
    }
    #endregion

    #region Events class

    #endregion

    #region Storage
    // Storage
    public class Storage
    {
        #region Events

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
            Users = new ObservableCollection<User>();
            //Tasks = new ObservableCollection<Task>();
            Tasks = Tests.Datas_Test.GetTasks();

            Hosts = ComputersInLocalNetwork.GetServerList(ComputersInLocalNetwork.SV_101_TYPES.SV_TYPE_ALL);
        }

        public ObservableCollection<User> Users;
        public ObservableCollection<Task> Tasks;
        public Dictionary<string, string> Hosts;
    }
    #endregion
}

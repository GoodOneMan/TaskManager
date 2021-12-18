using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace TMStructure
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
        [DataMember]
        public User BlockedUser { get; set; }
        [DataMember]
        public bool Enable { get; set; }
        [DataMember]
        public bool EditEnable { get; set; }
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
}

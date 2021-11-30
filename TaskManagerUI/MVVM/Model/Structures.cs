using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerUI.Core;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace TaskManagerUI.MVVM.Model
{
    [DataContract]
    class TaskStruct : BaseViewModel
    {
        private string _title;
        [DataMember]
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private string _description;
        [DataMember]
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private string _user;
        [DataMember]
        public string User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        private bool _isChecked;
        [DataMember]
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        private bool _state;
        [DataMember]
        public bool State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged("State");
            }
        }

        private string _comment;
        [DataMember]
        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged("Comment");
            }
        }

        private List<string> commentCard;
        [DataMember]
        public List<string> CommentCard
        {
            get { return commentCard; }
            set
            {
                commentCard = value;
                OnPropertyChanged("CommentCard");
            }
        }
        
        private Guid _guidTask;
        [DataMember]
        public Guid GuidTask
        {
            get { return _guidTask; }
            set 
            { 
                _guidTask = value;
                OnPropertyChanged("GuidTask");
            }
        }
    }

    [DataContract]
    class UserStruct : BaseViewModel
    {
        private Guid _userGuid;
        [DataMember]
        public Guid UserGuid
        {
            get { return _userGuid; }
            set
            {
                _userGuid = value;
                OnPropertyChanged("UserGuid");
            }
        }

        private string _name;
        [DataMember]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _host;
        [DataMember]
        public string Host
        {
            get { return _host; }
            set
            {
                _host = value;
                OnPropertyChanged("Host");
            }
        }

        private ICollection<TaskStruct> _tasks;
        [DataMember]
        public ICollection<TaskStruct> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                OnPropertyChanged("Tasks");
            }
        }
    }
}

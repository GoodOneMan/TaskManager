using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerUI.Core;

namespace TaskManagerUI.MVVM.Model
{
    class TaskStruct : BaseViewModel
    {
        private string _title;
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

    class UserStruct : BaseViewModel
    {
        private Guid _userGuid;
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

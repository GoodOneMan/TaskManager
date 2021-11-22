using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerUI.Core;

namespace TaskManagerUI.MVVM.ViewModel
{
    class CommentWindowModel : BaseViewModel
    {
        private string _currentUser;
        public string CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged("CurrentUser");
            }
        }

        public CommentWindowModel()
        {
            CurrentUser = Environment.UserName;
        }
    }
}

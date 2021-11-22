using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerUI.Core;
using TaskManagerUI.MVVM.Model;

namespace TaskManagerUI.MVVM.ViewModel
{
    class CommentWindowModel : BaseViewModel
    {
        private string _currentUser;
        public string CurrentUser
        {
            get {return _currentUser;}
            set
            {
                _currentUser = value;
                OnPropertyChanged("CurrentUser");
            }
        }

        private string _textComment;
        public string TextComment
        {
            get
            {
                if (Repository.GetRepository().SelectedTask.Comment != null)
                    _textComment = Repository.GetRepository().SelectedTask.Comment;
                return _textComment;
            }
            set
            {
                _textComment = value;
                OnPropertyChanged("TextComment");
            }
        }

        public CommentWindowModel()
        {
            CurrentUser = Environment.UserName;
        }

        private RelayCommand _setComment;
        public RelayCommand SetComment
        {
            get
            {
                return _setComment ?? (_setComment = new RelayCommand(
                    obj =>
                    {
                        if(Repository.GetRepository().SelectedTask != null)
                        {
                            Repository.GetRepository().SelectedTask.Comment += Environment.NewLine + Environment.UserName +
                                                                           Environment.NewLine + TextComment;

                            Repository.GetRepository().CommentWindow.Close();
                            Repository.GetRepository().SelectedTask = null;
                        }
                    }));
            }
        }
    }
}

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

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public string TextComment
        {
            get
            {
                return Repository.GetRepository().SelectedTask.Comment;
            }
            set
            {
                Repository.GetRepository().SelectedTask.Comment = value;
                OnPropertyChanged("TextComment");
            }
        }
        public List<string> TextCommentCard
        {
            get
            {
                return Repository.GetRepository().SelectedTask.CommentCard;
            }
            set
            {
                Repository.GetRepository().SelectedTask.CommentCard = value;
                OnPropertyChanged("TextCommentCard");
            }
        }

        private RelayCommand _setComment;
        public RelayCommand SetComment
        {
            get
            {
                return _setComment ?? (_setComment = new RelayCommand(
                    obj =>
                    {
                        //if(Repository.GetRepository().SelectedTask != null)
                        //{
                        //    if(!String.IsNullOrWhiteSpace(Message))
                        //        Repository.GetRepository().SelectedTask.Comment += Environment.NewLine + Environment.UserName + Environment.NewLine + Message;

                        //    Repository.GetRepository().CommentWindow.Close();
                        //    Repository.GetRepository().SelectedTask = null;
                        //}

                        if (Repository.GetRepository().SelectedTask != null)
                        {
                            if (!String.IsNullOrWhiteSpace(Message))
                                Repository.GetRepository().SelectedTask.CommentCard.Add(Environment.UserName + Environment.NewLine + Message);

                            Repository.GetRepository().CommentWindow.Close();
                            Repository.GetRepository().SelectedTask = null;
                        }
                    }));
            }
        }
    }
}

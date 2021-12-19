using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TMClient.CORE;
using TMClient.MVVM.Model;
using TMClient.MVVM.View;
using TMClient.WCF;
using TMStructure;

namespace TMClient.MVVM.ViewModel
{
    class CommentViewModel : BaseViewModel
    {
        public Storage Storage = null;

        #region Property
        private ObservableCollection<Comment> _comments;
        public ObservableCollection<Comment> Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
                OnPropertyChanged("Comments");
            }
        }

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
        #endregion

        #region Command
        private RelayCommand _setComment;
        public RelayCommand SetComment_Command
        {
            get
            {
                return _setComment ?? (_setComment = new RelayCommand(
                  obj =>
                  {
                      CommentView view = (CommentView)obj;
                      if (!String.IsNullOrEmpty(Message))
                      {
                          Comment Comment = new Comment();
                          Comment.Guid = Guid.NewGuid();
                          Comment.Message = Message;
                          Comment.TaskGuid = Storage.Task.Guid;
                          Comment.User = Storage.CurrentUser;

                          Comments.Add(Comment);

                          if (Storage.Task.Hint == "комментариев нет")
                              Storage.Task.Hint = "";

                          Storage.Task.Hint += String.Format("{0}{1}{2}{1}", Comment.User.Name, Environment.NewLine, Comment.Message);
                          
                          HostClient.GetClient().SendTask(Storage.Task);
                          
                      }

                      Storage.ImplementTask(Storage.Task);
                      Storage.Task = null;
                      view.Close();
                  }));
            }
        }
        #endregion

        public CommentViewModel()
        {
            Storage = Storage.GetStorage();
            Comments = Storage.Task.Comments;
        }
    }
}

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
                          Comments.Add(
                              new Comment()
                              {
                                  Guid = Guid.NewGuid(),
                                  Message = Message,
                                  TaskGuid = Storage.Task.Guid,
                                  User = Storage.CurrentUser
                              }
                            );
                          HostClient.GetClient().SendTask(Storage.Task);
                      }

                      Storage.ImplementTask(Storage.Task);
                      Storage.NotifyObservers();

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMClient.CORE;
using TMClient.MVVM.Model;
using TMClient.MVVM.View;
using TMStructure;

namespace TMClient.MVVM.ViewModel
{
    class CommentViewModel : BaseViewModel
    {
        Storage Storage = null;

        #region Property
        private Task _task;
        public Task Task
        {
            get { return _task; }
            set
            {
                _task = value;
                OnPropertyChanged("Task");
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
                          Task.Comments.Add(
                              new Comment()
                              {
                                  Guid = Guid.NewGuid(),
                                  Message = Message,
                                  TaskGuid = Task.Guid,
                                  User = Task.User
                              }
                            );
                          Storage.NotifyObservers(typeof(Task));
                          if (Storage.Task != null)
                          {
                              Storage.HostClient.SendTask(Storage.Task);
                              Storage.Task = null;
                          }

                      }
                      view.Close();
                  }));
            }
        }
        #endregion

        public CommentViewModel()
        {
            Storage = Storage.GetStorage();
            Task = Storage.Task;
        }
    }
}

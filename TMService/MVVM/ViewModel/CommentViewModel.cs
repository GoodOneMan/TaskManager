using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMService.CORE;
using TMService.MVVM.Model;
using TMService.MVVM.View;
using TMStructure;

namespace TMService.MVVM.ViewModel
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

        public void UpdateProperty(Type type)
        {
            //throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMClient_WPF.CORE;
using TMClient_WPF.MVVM.Model;
using TMClient_WPF.MVVM.View;

namespace TMClient_WPF.MVVM.ViewModel
{
    class CommentWindow_ViewModel : BaseViewModel, IObserver
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
                      CommentWindow_View window = (CommentWindow_View)obj;

                      Task.Comments.Add(
                          new Comment()
                          {
                              Guid = Guid.NewGuid(),
                              Message = Message,
                              TaskGuid = Task.Guid,
                              User = Task.User
                          }
                        );

                      Task.Description = Message;

                      Storage.NotifyObservers(typeof(Task));
                      Storage.RemoveObserver(this);
                      window.Close();
                  }));
            }
        }
        #endregion

        public CommentWindow_ViewModel()
        {
            Storage = Storage.GetStorage();
            Storage.AddObserver(this);
            Task = Storage.SelectTask;
        }

        public void UpdateProperty(Type type)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using TMService.CORE;
using TMService.MVVM.Model;
using TMService.MVVM.View;
using TMStructure;

namespace TMService.MVVM.ViewModel
{
    class TaskViewModel : BaseViewModel
    {
        Storage Storage = null;

        #region Property
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

        #endregion

        #region Add task
        private RelayCommand _add;
        public RelayCommand Add_Command
        {
            get { return _add ?? (_add = new RelayCommand(
                  obj =>
                  {
                      TaskView view = (TaskView)obj;

                      if (!String.IsNullOrEmpty(Title) && !String.IsNullOrEmpty(Description))
                      {
                          Task task = new Task();

                          task.Title = Title;
                          task.Description = Description;
                          task.Comments = new ObservableCollection<Comment>();
                          task.Guid = Guid.NewGuid();
                          task.IsChecked = false;
                          task.State = false;
                          task.Hint = "новая задача";
                          //task.User = Storage.Users.FirstOrDefault(item => item.Host == Dns.GetHostName());
                          task.User = new User { Name = Environment.UserName, Host = Dns.GetHostName(), Guid = Guid.NewGuid(), Description = "" };

                          Storage.Tasks.Add(task);
                          Storage.NotifyObservers(typeof(ObservableCollection<Task>));
                      }
                      view.Close();
                  }));
            }
        }
        #endregion

        public TaskViewModel()
        {
            Storage = Storage.GetStorage();
        }
    }
}

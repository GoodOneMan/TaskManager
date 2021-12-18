using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using TMClient.CORE;
using TMClient.MVVM.Model;
using TMClient.MVVM.View;
using TMClient.WCF;
using TMStructure;

namespace TMClient.MVVM.ViewModel
{
    class TaskViewModel : BaseViewModel
    {
        Storage Storage = null;
        bool IsNew = false;

        #region Property
        private string _viewTitle;
        public string ViewTitle
        {
            get { return _viewTitle; }
            set
            {
                _viewTitle = value;
                OnPropertyChanged("ViewTitle");
            }
        }
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
            get
            {
                return _add ?? (_add = new RelayCommand(
                obj =>
                {
                    TaskView view = (TaskView)obj;

                    if (!String.IsNullOrEmpty(Title) && !String.IsNullOrEmpty(Description))
                    {
                        if (IsNew)
                        {
                            Task task = new Task();

                            task.Title = Title;
                            task.Description = Description;
                            task.Comments = new ObservableCollection<Comment>();
                            task.Guid = Guid.NewGuid();
                            task.IsChecked = false;
                            task.State = false;
                            task.Hint = "комментариев нет";
                            task.User = Storage.CurrentUser;
                            task.Enable = true;
                            task.BlockedUser = null;
                            task.EditEnable = true;

                            Storage.Tasks.Add(task);
                            HostClient.GetClient().SendTasks(Storage.Tasks);
                        }
                        else
                        {
                            Storage.Task.Title = Title;
                            Storage.Task.Description = Description;
                            HostClient.GetClient().SendTask(Storage.Task);
                            Storage.ImplementTask(Storage.Task);
                        }

                        Storage.NotifyObservers();
                    }
                    else
                    {
                        Storage.Task = null;
                    }

                    view.Close();
                }));
            }
        }
        #endregion

        public TaskViewModel()
        {
            Storage = Storage.GetStorage();

            if (Storage.Task != null)
            {
                ViewTitle = "редактировать задачу";
                Title = Storage.Task.Title;
                Description = Storage.Task.Description;
                IsNew = false;
            }
            else
            {
                ViewTitle = "добавить задачу";
                IsNew = true;
            }
        }
    }
}

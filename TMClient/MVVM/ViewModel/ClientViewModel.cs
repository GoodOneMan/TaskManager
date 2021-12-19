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
    class ClientViewModel : BaseViewModel, IObserver
    {
        Storage Storage = null;
        HostClient HostClient = null;

        public ClientViewModel()
        {
            StartClient();
        }

        private void StartClient()
        {
            Storage = Storage.GetStorage();
            Storage.AddObserver(this);
            HostClient = HostClient.GetClient();

            if (HostClient.Connect())
            {
                Storage.Tasks = HostClient.GetTasks();
            }
        }

        #region Property
        private ObservableCollection<Task> _tasks;
        public ObservableCollection<Task> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                OnPropertyChanged("Tasks");
            }
        }
        #endregion

        #region Command
        public RelayCommand _closeWindow;
        public RelayCommand CloseWindow_Command
        {
            get
            {
                return _closeWindow ?? (_closeWindow = new RelayCommand(
                    obj =>
                    {
                        if (HostClient != null && HostClient.Desconnect())
                        {
                            
                        }

                        ClientView view = (ClientView)obj;
                        view.Close();

                    }));
            }
        }
        public RelayCommand _minWindow;
        public RelayCommand MinWindow_Command
        {
            get
            {
                return _minWindow ?? (_minWindow = new RelayCommand(
                    obj =>
                    {
                        ClientView view = (ClientView)obj;
                        view.WindowState = System.Windows.WindowState.Minimized;
                    }));
            }
        }

        private RelayCommand _isChecked;
        public RelayCommand IsChecked_Command
        {
            get
            {
                return _isChecked ?? (_isChecked = new RelayCommand(
                    obj =>
                    {
                        if (obj != null)
                        {
                            Task task = Storage.Tasks.FirstOrDefault(item => item.Guid == ((Task)obj).Guid);

                            if (task.IsChecked)
                            {
                                task.BlockedUser = Storage.CurrentUser;
                                task.Enable = false;
                            }
                            else
                            {
                                task.BlockedUser = null;
                                task.Enable = true;
                            }

                            ObservableCollection<Task> temp = new ObservableCollection<Task>();
                            foreach (Task t in Storage.Tasks)
                                if (t.IsChecked)
                                    temp.Add(t);

                            foreach (Task t in Storage.Tasks)
                                if (!t.IsChecked)
                                    temp.Add(t);

                            Storage.Tasks = temp;
                            HostClient.GetClient().SendTasks(Storage.Tasks);
                        }
                    }));
            }
        }

        private RelayCommand _setComment;
        public RelayCommand SetComment_Command
        {
            get
            {
                return _setComment ?? (_setComment = new RelayCommand(
                    obj =>
                    {
                        Storage.Task = (Task)obj;
                        new CommentView().Show();
                    }));
            }
        }

        private RelayCommand _editTask;
        public RelayCommand EditTask_Command
        {
            get
            {
                return _editTask ?? (_editTask = new RelayCommand(
                    obj =>
                    {
                        Storage.Task = (Task)obj;
                        new TaskView().Show();
                    }));
            }
        }
        private RelayCommand _addask;
        public RelayCommand AddTask_Command
        {
            get
            {
                return _addask ?? (_addask = new RelayCommand(
                    obj =>
                    {
                        new TaskView().Show();
                    }));
            }
        }
        #endregion

        #region IObserver
        public void UpdateProperty()
        {
            Tasks = new ObservableCollection<Task>();
            Tasks = Storage.Tasks;
            OnPropertyChanged("Tasks");
        }
        #endregion
    }
}

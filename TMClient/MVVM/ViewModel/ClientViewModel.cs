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
        public ClientViewModel()
        {
            Storage = Storage.GetStorage();
            Storage.AddObserver(this);

            Storage.HostClient = HostClient.GetClient();
            if (Storage.HostClient.Connect())
            {
                Storage.Tasks = Storage.HostClient.GetTasks();
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
                        if (Storage.HostClient != null && Storage.HostClient.Desconnect())
                        {
                            ClientView view = (ClientView)obj;
                            view.Close();
                        }

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

                            ObservableCollection<Task> temp = new ObservableCollection<Task>();
                            foreach (Task t in Storage.Tasks)
                            {
                                if (t.IsChecked)
                                    temp.Add(t);
                            }
                            foreach (Task t in Storage.Tasks)
                            {
                                if (!t.IsChecked)
                                    temp.Add(t);
                            }
                            Storage.Tasks.Clear();
                            Storage.Tasks = temp;

                            // Send service
                            HostClient.GetClient().SendTask(task);
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
                        Task task = (Task)obj;
                        Storage.Task = task;
                        new CommentView().Show();
                    }));
            }
        }
        public RelayCommand _changedTask;
        public RelayCommand ChangedTask_Command
        {
            get
            {
                return _changedTask ?? (_changedTask = new RelayCommand(
                    obj =>
                    {
                        Task task = (Task)obj;

                        if (task != null)
                        {

                        }

                    }));
            }
        }
        #endregion

        public void UpdateProperty(Type type)
        {
            Tasks = new ObservableCollection<Task>();
            Tasks = Storage.Tasks;
            OnPropertyChanged("Tasks");
        }
    }
}

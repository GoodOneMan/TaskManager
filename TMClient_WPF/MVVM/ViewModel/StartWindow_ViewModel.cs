using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TMClient_WPF.CORE;
using TMClient_WPF.MVVM.Model;
using TMClient_WPF.MVVM.View;
using TMClient_WPF.WCF;

namespace TMClient_WPF.MVVM.ViewModel
{
    class StartWindow_ViewModel : BaseViewModel, IObserver
    {
        Storage Storage = null;
        HostClient HostClient = null;

        public StartWindow_ViewModel()
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
                        if (HostClient.Desconnect())
                        {
                            StartWindow_View window = (StartWindow_View)obj;
                            window.Close();
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
                        StartWindow_View window = (StartWindow_View)obj;
                        window.WindowState = System.Windows.WindowState.Minimized;
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
                        Storage.SelectTask = task;
                        new CommentWindow_View().Show();
                    }));
            }
        }
        public RelayCommand _changedTask;
        public RelayCommand ChangedTask_Command
        {
            get
            {
                return _minWindow ?? (_minWindow = new RelayCommand(
                    obj =>
                    {
                        Task task = (Task)obj;

                        if(task != null)
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TMClient_WPF.CORE;
using TMClient_WPF.MVVM.View;
using TMClient_WPF.WCF;

namespace TMClient_WPF.MVVM.ViewModel
{
    class StartWindow_ViewModel : BaseViewModel
    {
        HostClient HostClient = null;

        public StartWindow_ViewModel()
        {
            HostClient = HostClient.GetClient();
            if (HostClient.Connect())
            {
                Tasks = HostClient.GetTasks();
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
    }
}

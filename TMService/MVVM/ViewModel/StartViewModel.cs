using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TMService.CORE;
using TMService.MVVM.Model;
using TMService.MVVM.View;
using TMService.WCF;
using TMStructure;

namespace TMService.MVVM.ViewModel
{
    class StartViewModel : BaseViewModel, IObserver
    {
        private bool isRun;
        private static string GrayColor;
        private static string RedColor;
        private Storage Storage;
        private HostServices HostServices;

        #region Property
        private string _buttonIcon;
        public string ButtonIcon
        {
            get { return _buttonIcon; }
            set
            {
                _buttonIcon = value;
                OnPropertyChanged("ButtonIcon");
            }
        }
        private string _buttonColor;
        public string ButtonColor
        {
            get { return _buttonColor; }
            set
            {
                _buttonColor = value;
                OnPropertyChanged("ButtonColor");
            }
        }
        private string _textColor;
        public string TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;
                OnPropertyChanged("TextColor");
            }
        }
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged("Users");
            }
        }

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

        private ObservableCollection<string> _log;
        public ObservableCollection<string> Log
        {
            get { return _log; }
            set
            {
                _log = value;
                OnPropertyChanged("Log");
            }
        }

        // ???
        private bool _userSelect = true;
        public bool UserSelect
        {
            get { return _userSelect; }
            set
            {
                _userSelect = value;
                OnPropertyChanged("UserSelect");
            }
        }
        #endregion

        #region Command
        private RelayCommand _closeWindow;
        public RelayCommand CloseWindow_Command
        {
            get
            {
                return _closeWindow ?? (_closeWindow = new RelayCommand(
                    obj =>
                    {
                        StartView view = (StartView)obj;
                        view.Close();
                    }));
            }
        }
        private RelayCommand _minWindow;
        public RelayCommand MinWindow_Command
        {
            get
            {
                return _minWindow ?? (_minWindow = new RelayCommand(
                    obj =>
                    {
                        StartView view = (StartView)obj;
                        view.WindowState = System.Windows.WindowState.Minimized;
                    }));
            }
        }
        private RelayCommand _maxWindow;
        public RelayCommand MaxWindow_Command
        {
            get
            {
                return _maxWindow ?? (_maxWindow = new RelayCommand(
                    obj =>
                    {
                        StartView view = (StartView)obj;
                        if (view.WindowState == System.Windows.WindowState.Normal)
                            view.WindowState = System.Windows.WindowState.Maximized;
                        else
                            view.WindowState = System.Windows.WindowState.Normal;
                    }));
            }
        }

        private RelayCommand _startOrStopService;
        public RelayCommand StartStopService_Command
        {
            get
            {
                return _startOrStopService ?? (_startOrStopService = new RelayCommand(
                    obj =>
                    {
                        if (isRun)
                            StopService();
                        else
                            StartService();
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
                            Task task = (Task)obj;

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

                            Storage.OnTaskChanged(new TaskChangedEventArgs(Storage.CurrentUser, task));
                            Storage.ImplementTask(task);
                        }
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
        #endregion

        public StartViewModel()
        {
            // internal 
            isRun = false;
            GrayColor = "#40626C";
            RedColor = "#AE6E64";

            // property changed
            _buttonIcon = "Play";
            _buttonColor = GrayColor;
            _textColor = RedColor;
            _text = "сервер не запущен";

            // Storage
            Storage = Storage.GetStorage();
            Storage.AddObserver(this);
            
            // Services
            HostServices = new HostServices();
        }

        private void StartService()
        {
            isRun = true;
            ButtonIcon = "Stop";
            ButtonColor = RedColor;
            TextColor = GrayColor;
            Text = "сервер запущен";

            //service
            HostServices.InitHost();
            HostServices.StartHost();

            Storage.GetState();
        }

        private void StopService()
        {
            isRun = false;
            ButtonIcon = "Play";
            ButtonColor = GrayColor;
            TextColor = RedColor;
            Text = "сервер остановлен";

            //service
            HostServices.StopHost();

            if (!Storage.SetState())
            {

            }
        }

        // Updata Storage Property
        public void UpdateProperty()
        {
            Tasks = new ObservableCollection<Task>();
            Tasks = Storage.Tasks;
            OnPropertyChanged("Tasks");

            Users = new ObservableCollection<User>();
            Users = Storage.Users;
            OnPropertyChanged("Users");

            Log = new ObservableCollection<string>();
            Log = Storage.Log;
            OnPropertyChanged("Log");
        }
  
    }
}

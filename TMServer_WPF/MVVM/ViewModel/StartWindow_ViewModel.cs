using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using TMServer_WPF.CORE;
using TMServer_WPF.MVVM.View;

namespace TMServer_WPF.MVVM.ViewModel
{
    class StartWindow_ViewModel : BaseViewModel
    {
        private bool isRun;
        private static string GrayColor;
        private static string RedColor;
        private WCF.HostServices HostServices;
        private Storage Storage;

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
                        StartWindow_View window = (StartWindow_View)obj;
                        window.Close();
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
        public RelayCommand _maxWindow;
        public RelayCommand MaxWindow_Command
        {
            get
            {
                return _maxWindow ?? (_maxWindow = new RelayCommand(
                    obj =>
                    {
                        StartWindow_View window = (StartWindow_View)obj;
                        if(window.WindowState == System.Windows.WindowState.Normal)
                            window.WindowState = System.Windows.WindowState.Maximized;
                        else
                            window.WindowState = System.Windows.WindowState.Normal;
                    }));
            }
        }
        public RelayCommand _startOrStopService;
        public RelayCommand StartStopService_Command
        {
            get
            {
                return _startOrStopService ?? (_startOrStopService = new RelayCommand(
                    obj =>
                    {
                        if (isRun)
                        {
                            InitStopService();
                        }
                        else
                        {
                            InitStartService();
                        }
                    }));
            }
        }
        #endregion

        #region Internal method
        public StartWindow_ViewModel()
        {
            InitWindow();
            //
            Model.SQLite_Model.GetDB();
        }

        private void InitWindow()
        {
            // internal 
            isRun = false;
            GrayColor = "#40626C";
            RedColor = "#AE6E64";
            HostServices = null;
            Storage = Storage.GetStorage();
            // service
            HostServices = new WCF.HostServices();
            // property changed
            _buttonIcon = "Play";
            _buttonColor = GrayColor;
            _textColor = RedColor;
            _text = "сервер не запущен";
            Log = new ObservableCollection<string>();
            Tasks = Storage.Tasks;
            Users = Storage.Users;
        }
        private void InitStartService()
        {
            // Config values
            isRun = true;
            ButtonIcon = "Stop";
            ButtonColor = RedColor;
            TextColor = GrayColor;
            Text = "сервер запущен";

            // Subscribe to event
            WCF.Services.UserChanged += ServiceEvent_UserChanged;
            WCF.Services.TaskChanged += ServiceEvent_TaskChanged;

            // Start service
            HostServices.InitHost();
            HostServices.StartHost();
            Log.Add("сервис запущен " + DateTime.Now.ToString());

            // Test
            // Tests.Datas_Test.FillDB();
        }
        private void InitStopService()
        {
            // Config values
            isRun = false;
            ButtonIcon = "Play";
            ButtonColor = GrayColor;
            TextColor = RedColor;
            Text = "сервер остановлен";

            // Subscribe to event
            WCF.Services.UserChanged -= ServiceEvent_UserChanged;
            WCF.Services.TaskChanged -= ServiceEvent_TaskChanged;

            // Stop service
            HostServices.StopHost();
            Log.Add("сервис остановлен " + DateTime.Now.ToString());
        }
        #endregion

        #region ServiceEvent
        public void ServiceEvent_UserChanged(object sender, WCF.UserChangedEventArgs e)
        {
            Log.Add(e.Message + e.User.Name);
        }
        public void ServiceEvent_TaskChanged(object sender, WCF.TaskChangedEventArgs e)
        {
            Log.Add(e.Message + e.Task.Title);
        }
        #endregion
    }
}

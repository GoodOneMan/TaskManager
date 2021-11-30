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
        private bool isRun = false;
        private static string greenColor = "#8FBC8F";
        private static string redColor = "#CD5C5C";
        WCF.HostServices HostServices = null;

        #region Property
        private string _buttonIcon = "Play";
        public string ButtonIcon
        {
            get { return _buttonIcon; }
            set
            {
                _buttonIcon = value;
                OnPropertyChanged("ButtonIcon");
            }
        }
        private string _buttonColor = greenColor;
        public string ButtonColor
        {
            get { return _buttonColor; }
            set
            {
                _buttonColor = value;
                OnPropertyChanged("ButtonColor");
            }
        }
        private string _textColor = redColor;
        public string TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;
                OnPropertyChanged("TextColor");
            }
        }
        private string _text = "сервер не запущен";
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
                            // Stop service
                            isRun = false;
                            ButtonIcon = "Play";
                            ButtonColor = greenColor;
                            TextColor = redColor;
                            Text = "сервер остановлен";

                            HostServices.StopHost();
                        }
                        else
                        {
                            // Start service
                            isRun = true;
                            ButtonIcon = "Stop";
                            ButtonColor = redColor;
                            TextColor = greenColor;
                            Text = "сервер запущен";

                            HostServices.InitHost();
                            HostServices.StartHost();
                        }
                    }));
            }
        }
        #endregion

        #region Internal method
        public StartWindow_ViewModel()
        {
            Init();
        }

        private void Init()
        {
            // test data
            Users = Tests.Datas_Test.GetUsers();
            Tasks = Tests.Datas_Test.GetTasks();

            HostServices = new WCF.HostServices();
        }
        #endregion
    }
}

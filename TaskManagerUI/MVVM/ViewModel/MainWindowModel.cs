using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TaskManagerUI.Core;
using TaskManagerUI.MVVM.Model;

namespace TaskManagerUI.MVVM.ViewModel
{
    class MainWindowModel : BaseViewModel, TMService.ITMServiceCallback
    {
        // Property
        public ObservableCollection<TaskStruct> Tasks
        {
            get { return Repository.GetRepository().Tasks; }
            set
            {
                Repository.GetRepository().Tasks = value;
                OnPropertyChanged("Tasks");
            }
        }

        private UserStruct _user;
        public UserStruct User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        // Service property
        public static TMService.TMServiceClient client;

        // Construct
        public MainWindowModel()
        {
            User = Tests.UserStructTest.GetCurrentUser();
            //Tasks = new ObservableCollection<TaskStruct>(Tests.TaskStructTest.GetCollectionTasks());

            // client part
            client = new TMService.TMServiceClient(new System.ServiceModel.InstanceContext(this));

            #region Two
            var tasks = client.GetTasks();

            Tasks = new ObservableCollection<TaskStruct>();
            foreach (var t in tasks)
            {
                Tasks.Add(
                    new TaskStruct()
                    {
                        User = t.User,
                        Title = t.Title,
                        Comment = t.Comment,
                        State = t.State,
                        CommentCard = t.CommentCard.ToList(),
                        Description = t.Description,
                        GuidTask = t.GuidTask,
                        IsChecked = t.IsChecked
                    }
                    );
            }

            
            #endregion

            #region One
            //TMService.UserStruct user = new TMService.UserStruct()
            //{
            //    UserGuid = User.UserGuid,
            //    Host = User.Host,
            //    Name = User.Name
            //};
            //client.Connect(user);
            #endregion

        }

        // Command
        private RelayCommand _isCheckedModel;
        public RelayCommand IsCheckedCommand
        {
            get
            {
                return _isCheckedModel ?? (_isCheckedModel = new RelayCommand(
                    obj =>
                    {
                        TaskStruct task = Tasks.FirstOrDefault(item => item.GuidTask == ((TaskStruct)obj).GuidTask);

                        if (task != null)
                        {
                            if (task.IsChecked)
                                task.User = Environment.UserName;
                            else
                                task.User = "";
                        }

                        ObservableCollection<TaskStruct> temp = new ObservableCollection<TaskStruct>();
                        foreach (TaskStruct t in Tasks)
                        {
                            if (t.IsChecked)
                                temp.Add(t);
                        }
                        foreach (TaskStruct t in Tasks)
                        {
                            if (!t.IsChecked)
                                temp.Add(t);
                        }
                        Tasks.Clear();
                        Tasks = temp;

                    }));
            }
        }

        private RelayCommand _setComment;
        public RelayCommand SetComment
        {
            get
            {
                return _setComment ?? (_setComment = new RelayCommand(
                    obj =>
                    {
                        TaskStruct task = Tasks.FirstOrDefault(item => item == (TaskStruct)obj);
                        if (task != null)
                        {
                            Repository.GetRepository().SelectedTask = task;
                            Repository.GetRepository().CommentWindow = new View.CommentWindow();
                            Repository.GetRepository().CommentWindow.Show();
                        }
                    }));
            }
        }

        private RelayCommand _closeTask;
        public RelayCommand CloseTask
        {
            get
            {
                return _closeTask ?? (_closeTask = new RelayCommand(
                    obj =>
                    {
                        TaskStruct task = Tasks.FirstOrDefault(item => item == (TaskStruct)obj);
                        if (task != null)
                            Tasks.Remove(task);
                    }));
            }
        }

        private RelayCommand _closeApplication;
        public RelayCommand CloseApplication
        {
            get
            {
                return _closeApplication ?? (_closeApplication = new RelayCommand(
                    obj =>
                    {
                        Application.Current.MainWindow.Close();
                    }));
            }
        }

        private RelayCommand _hideApplication;
        public RelayCommand HideApplication
        {
            get
            {
                return _hideApplication ?? (_hideApplication = new RelayCommand(
                    obj =>
                    {
                        Application.Current.MainWindow.WindowState = WindowState.Minimized;

                        // Test get list hosts
                        //var list = ComputersInLocalNetwork.GetServerList(ComputersInLocalNetwork.SV_101_TYPES.SV_TYPE_ALL);
                        //using (StreamWriter sw = new StreamWriter(@"D:\task_host.txt"))
                        //{
                        //    foreach(string str in list)
                        //    {
                        //        sw.WriteLine(str);
                        //    }
                        //}
                    }));
            }
        }

        // Service callback method
        public void NotifyUpdataTasksCallback(TMService.TaskStruct _task)
        {
            var task = Tasks.FirstOrDefault(t => t.GuidTask == _task.GuidTask);

            if(task != null)
            {
                task.User = _task.User;
                task.IsChecked = _task.IsChecked;
                task.State = _task.State;
                task.User = _task.User;
                task.CommentCard = new List<string>(_task.CommentCard);
            }
            else
            {
                Tasks.Add(new TaskStruct()
                {
                    Title = _task.Title,
                    IsChecked = _task.IsChecked,
                    State = _task.State,
                    User = _task.User,
                    GuidTask = _task.GuidTask,
                    Description = _task.Description,
                    Comment = _task.Comment,
                    CommentCard = new List<string>(_task.CommentCard)
                });
            }
        }
    }
}

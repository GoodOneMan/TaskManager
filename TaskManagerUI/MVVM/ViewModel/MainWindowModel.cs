using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TaskManagerUI.Core;
using TaskManagerUI.MVVM.Model;

namespace TaskManagerUI.MVVM.ViewModel
{
    class MainWindowModel : BaseViewModel
    {

        #region Test data
        public MainWindowModel()
        {

            Tasks = new List<TaskStruct>();

            for (int i = 0; i < 2000; i++)
            {
                TaskStruct t = new TaskStruct
                {
                    Title = "Task " + i.ToString(),
                    Description = "Description " + i.ToString(),
                    IsChecked = false,
                    State = false,
                    User = ""
                };

                Tasks.Add(t);
            }
        }

        //public ObservableCollection<TaskStruct> _tasks;
        //public ObservableCollection<TaskStruct> Tasks
        //{
        //    get { return _tasks; }
        //    set
        //    {
        //        _tasks = value;
        //        OnPropertyChanged("Tasks");
        //    }
        //}

        public List<TaskStruct> _tasks;
        public List<TaskStruct> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                OnPropertyChanged("Tasks");
            }
        }

        private RelayCommand _isCheckedModel;
        public RelayCommand IsCheckedCommand
        {
            get
            {
                return _isCheckedModel ?? (_isCheckedModel = new RelayCommand(
                    obj =>
                    {
                        TaskStruct task = Tasks.FirstOrDefault(item => item == (TaskStruct)obj);

                        if(task != null)
                        {
                            if(task.IsChecked)
                                task.User = Environment.UserName;
                            else
                                task.User = "";
                        }

                        List<TaskStruct> temp = new List<TaskStruct>();
                        foreach(TaskStruct t in Tasks)
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

                        //Tasks.OrderBy(item => item.IsChecked ? 0 : 1);
                        //List<TaskStruct> tmpTasks = new List<TaskStruct>();
                        //foreach(TaskStruct t in Tasks)
                        //{
                        //    if (t.IsChecked)
                        //        tmpTasks.Add(t);
                        //}

                        //foreach (TaskStruct t in Tasks)
                        //{
                        //    if (!t.IsChecked)
                        //        tmpTasks.Add(t);
                        //}

                        //Tasks.Clear();
                        //foreach (TaskStruct t in tmpTasks)
                        //{
                        //    Tasks.Add(t);
                        //}

                        //ObservableCollection<TaskStruct> temp;
                        //temp = new ObservableCollection<TaskStruct>(Tasks.OrderBy(item => item.IsChecked));
                        //Tasks = temp;

                        //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Tasks);
                        //view.SortDescriptions.Add(new SortDescription("Age", ListSortDirection.Ascending));

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


                    }));
            }
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManagerUI.Core;

namespace TaskManagerUI.MVVM.Model
{
    class Repository : BaseViewModel
    {
        private static Repository instance;

        public static Repository GetRepository()
        {
            if (instance == null)
                instance = new Repository();

            return instance;
        }

        private Repository()
        {
            Tasks = new ObservableCollection<TaskStruct>();
            SelectedTask = null;
            CommentWindow = null;
        }

        public ObservableCollection<TaskStruct> Tasks;
        public TaskStruct SelectedTask;
        public Window CommentWindow;
    }
}

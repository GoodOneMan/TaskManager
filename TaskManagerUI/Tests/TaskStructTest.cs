using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerUI.MVVM.Model;

namespace TaskManagerUI.Tests
{
    class TaskStructTest
    {
        public static ICollection<TaskStruct> GetCollectionTasks()
        {
            ICollection<TaskStruct>  Tasks = new ObservableCollection<TaskStruct>();

            for (int i = 0; i < 2000; i++)
            {
                TaskStruct t = new TaskStruct
                {
                    Title = "Task " + i.ToString(),
                    Description = "Description " + i.ToString(),
                    IsChecked = false,
                    State = false,
                    User = "",
                    Comment = "",
                    CommentCard = new List<string>(),
                    GuidTask = Guid.NewGuid()
                };

                Tasks.Add(t);
            }

            return Tasks;
        }
    }
}

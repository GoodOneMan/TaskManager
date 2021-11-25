using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMService_WCF_LIB.Tests
{
    class Datas
    {
        public static ObservableCollection<Task> GetTasks()
        {
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();

            for (int i = 0; i < 400000; i++)
            {
                Task t = new Task
                {
                    Title = "Task Service" + i.ToString(),
                    Description = "Description " + i.ToString(),
                    IsChecked = false,
                    State = false,
                    User = null,
                    Hint = "test datas",
                    //Comments = new List<Comment>().ToArray(),
                    Comments = new ObservableCollection<Comment>(),
                    Guid = Guid.NewGuid()
                };

                tasks.Add(t);
            }

            return tasks;
        }
    }
}

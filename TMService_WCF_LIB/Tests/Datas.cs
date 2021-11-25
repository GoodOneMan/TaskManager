using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMService_WCF_LIB.Tests
{
    class Datas
    {
        public static List<Task> GetTasks()
        {
            List<Task> tasks = new List<Task>();

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
                    Comments = new List<Comment>().ToArray(),
                    Guid = Guid.NewGuid()
                };

                tasks.Add(t);
            }

            return tasks;
        }
    }
}

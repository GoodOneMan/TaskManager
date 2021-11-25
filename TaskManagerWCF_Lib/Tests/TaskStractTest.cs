using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerWCF_Lib.Struct;

namespace TaskManagerWCF_Lib.Tests
{
    class TaskStractTest
    {
        public static TaskStruct[] GetTasks()
        {
            List<TaskStruct> Tasks = new List<TaskStruct>();

            for (int i = 0; i < 2000; i++)
            {
                TaskStruct t = new TaskStruct
                {
                    Title = "Task Service" + i.ToString(),
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

            return Tasks.ToArray();
        }
    }
}

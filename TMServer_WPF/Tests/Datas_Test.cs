using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using TMServer_WPF.CORE;

namespace TMServer_WPF.Tests
{
    class Datas_Test
    {
        public static ObservableCollection<User> GetUsers()
        {
            ObservableCollection<User> users = new ObservableCollection<User>();

            for(int i = 0; i < 200; i++)
            {
                users.Add(
                    new User()
                    {
                        Name = "user " + i,
                        Host = "host " + i,
                        Description = i.ToString(),
                        Guid = Guid.NewGuid(),
                        OCtx = null
                    }
                    );
            }

            return users;
        }
    
        public static ObservableCollection<Task> GetTasks()
        {
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();
            ObservableCollection<Comment> comments = null;

            User user = new User()
            {
                Name = "test user ",
                Host = "test host ",
                Description = "Some description ",
                Guid = Guid.NewGuid(),
                OCtx = null
            };

            for (int i = 0; i < 300; i++)
            {
                comments = new ObservableCollection<Comment>();
                comments.Add(
                    new Comment()
                    {
                        Message = "Some message \" comment \"",
                        User = user
                    }
                    );

                tasks.Add(
                    new Task()
                    {
                        Title = "Title " + i,
                        Description = "Description " + i,
                        Hint = "Hint " + i,
                        IsChecked = false,
                        State = false,
                        Comments = comments,
                        Guid = Guid.NewGuid(),
                        User = user
                    }
                    );
            }

            return tasks;
        }
    }
}

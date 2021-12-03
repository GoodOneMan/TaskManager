using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using TMServer_WPF.CORE;
using System.Threading;

namespace TMServer_WPF.Tests
{
    class Datas_Test
    {
        private static Random rnd = null;

        public static ObservableCollection<User> GetUsers()
        {
            #region One
            //ObservableCollection<User> users = new ObservableCollection<User>();
            //for(int i = 0; i < 200; i++)
            //{
            //    users.Add(
            //        new User()
            //        {
            //            Name = "user " + i,
            //            Host = "host " + i,
            //            Description = i.ToString(),
            //            Guid = Guid.NewGuid(),
            //            OCtx = null
            //        }
            //        );
            //}
            //return users;
            #endregion
            MVVM.Model.SQLite_Model sQLite_Model = MVVM.Model.SQLite_Model.GetDB();
            return sQLite_Model.GetAllUsers();
        }

        public static ObservableCollection<Task> GetTasks()
        {
            #region One
            //ObservableCollection<Task> tasks = new ObservableCollection<Task>();
            //ObservableCollection<Comment> comments = null;

            //User user = new User()
            //{
            //    Name = "test user ",
            //    Host = "test host ",
            //    Description = "Some description ",
            //    Guid = Guid.NewGuid(),
            //    OCtx = null
            //};

            //for (int i = 0; i < 10; i++)
            //{
            //    comments = new ObservableCollection<Comment>();
            //    comments.Add(
            //        new Comment()
            //        {
            //            Message = "Some message \" comment \"",
            //            User = user
            //        }
            //        );

            //    tasks.Add(
            //        new Task()
            //        {
            //            Title = "Title " + i,
            //            Description = "Description " + i,
            //            Hint = "Hint " + i,
            //            IsChecked = false,
            //            State = false,
            //            Comments = comments,
            //            Guid = Guid.NewGuid(),
            //            User = user
            //        }
            //        );
            //}
            //return tasks;
            #endregion
            MVVM.Model.SQLite_Model sQLite_Model = MVVM.Model.SQLite_Model.GetDB();
            return sQLite_Model.GetAllTasks();
        }

        public static void FillDB()
        {
            ObservableCollection<User> users = new ObservableCollection<User>();
            ObservableCollection<Comment> comments = new ObservableCollection<Comment>();
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();
            
            // User
            for(int i = 0; i < 10; i++)
            {
                users.Add(
                    new User()
                    {
                        Name = "name " + i,
                        Host = "host " + i,
                        Description = "description " + i,
                        Guid = Guid.NewGuid()
                    }
                    );
            }
            // Task
            for(int i = 0; i < 100; i++)
            {
                Thread.Sleep(1);
                int user_index = new Random().Next(0, 9);
                tasks.Add(
                    new Task()
                    {
                        Title = "Title " + i,
                        Description = "Description " + i,
                        Guid = Guid.NewGuid(),
                        Hint = "Hint " + i,
                        IsChecked = false,
                        State = false,
                        User = users[user_index],
                        Comments = new ObservableCollection<Comment>()
                    }
                    );
            }
            // Comment
            for(int i = 0; i < 1000; i++)
            {
                Thread.Sleep(1);
                int user_index = new Random().Next(0, 9);
                Thread.Sleep(1);
                int task_index = new Random().Next(0, 99);
                comments.Add(
                    new Comment()
                    {
                        Message = "",
                        User = users[user_index],
                        TaskGuid = tasks[task_index].Guid
                    }
                    );
            }
            // Comments to task
            foreach(Task task in tasks)
            {
                Thread.Sleep(1);
                rnd = new Random();
                int count = rnd.Next(0, 99);

                List<int> indexs = new List<int>();

                for(int i = 0; i < count; i++)
                {
                    Thread.Sleep(1);
                    rnd = new Random();
                    int index_comment = rnd.Next(0, 999);

                    if (!indexs.Contains(index_comment))
                    {
                        task.Comments.Add(comments[index_comment]);
                        indexs.Add(index_comment);
                    }
                }
            }

            // Write to DB
            MVVM.Model.SQLite_Model sQLite_Model = MVVM.Model.SQLite_Model.GetDB();

            foreach(User user in users)
            {
                sQLite_Model.InsertUser(user);
            }
            
            foreach(Task task in tasks)
            {
                sQLite_Model.InsertTask(task);
            }

            foreach (Comment comment in comments)
            {
                sQLite_Model.InsertComment(comment);
            }
        }
    }
}

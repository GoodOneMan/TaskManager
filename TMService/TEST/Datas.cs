using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using TMService.MVVM.Model;
using TMStructure;

namespace TMService.TEST
{
    class Datas
    {
        private static Random rnd = null;

        public static ObservableCollection<User> GetUsers()
        {
            DataBase DataBase = MVVM.Model.DataBase.GetDB();
            return DataBase.GetAllUsers();
        }

        public static ObservableCollection<Task> GetTasks()
        {
            DataBase DataBase = MVVM.Model.DataBase.GetDB();
            return DataBase.GetAllTasks();
        }

        public static void FillDB()
        {
            ObservableCollection<User> users = new ObservableCollection<User>();
            ObservableCollection<Comment> comments = new ObservableCollection<Comment>();
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();

            // User
            for (int i = 0; i < 30; i++)
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
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(1);
                int user_index = new Random().Next(0, 29);
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
            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(1);
                int user_index = new Random().Next(0, 29);
                Thread.Sleep(1);
                int task_index = new Random().Next(0, 99);
                comments.Add(
                    new Comment()
                    {
                        Message = "List C# - списки представляют собой удивительно гибкий инструмент по работе с коллекциями. Одной из главных особенностей списков является возможность использовать любой тип данных. Кроме того, в списках реализовано множество полезных методов.",
                        User = users[user_index],
                        TaskGuid = tasks[task_index].Guid,
                        Guid = Guid.NewGuid()
                    }
                    );
            }

            // Comments to task
            foreach (Task task in tasks)
            {
                Thread.Sleep(1);
                rnd = new Random();
                int count = rnd.Next(0, 999);

                List<int> indexs = new List<int>();

                for (int i = 0; i < count; i++)
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
            DataBase DataBase = MVVM.Model.DataBase.GetDB();

            DataBase.InsertAllUsers(users);
            DataBase.InsertAllTasks(tasks);
            DataBase.InsertAllComments(comments);

            Storage.GetStorage().Tasks = tasks;
            Storage.GetStorage().Users = users;
        }
    }
}

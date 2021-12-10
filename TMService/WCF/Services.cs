﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using TMService.CORE;
using TMService.MVVM.Model;
using TMStructure;

namespace TMService.WCF
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant, IncludeExceptionDetailInFaults = true, UseSynchronizationContext = false)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    class Services : IContract_Service, IDataContract_Service, IObserver
    {
        Storage Storage = null;
        //Guid Guid;
        public Services()
        {
            Storage = Storage.GetStorage();
            //Guid = Guid.NewGuid();
            Storage.AddObserver(this);
        }

        #region IContract_Service
        public User Connect(string name, string host)
        {
            User user = Storage.Users.FirstOrDefault(item => item.Host == host && item.Name == name);

            if (user == null)
            {
                user = new User();
                user.Name = name;
                user.Host = host;
                //user.Description = Storage.Hosts.FirstOrDefault(item => item.Key == host).Value;
                user.Description = "Пользователь — лицо или организация, которое использует действующую систему для выполнения конкретной функции.";
                user.Guid = Guid.NewGuid();
                user.OCtx = OperationContext.Current;

                Storage.Log.Add(String.Format("пользователь {0} подключился в {1}", user.Description, DateTime.Now.ToString()));

                Storage.Users.Add(user);
            }
            else
            {
                Storage.Log.Add(String.Format("пользователь {0} уже подключился", user.Description));
            }

            //Storage.AddObserver(this);

            return user;
        }
        public bool Disconnect(Guid guid)
        {
            User user = Storage.Users.FirstOrDefault(item => item.Guid == guid);
            if (user != null)
            {
                Storage.Users.Remove(user);
                Storage.Log.Add(String.Format("пользователь {0} отключился", user.Description));

                return true;
            }

            //Storage.RemoveObserver(this);

            return false;
        }
        #endregion

        #region IContract_Callback
        private void Contract_Callback(string msg)
        {
            foreach (User user in Storage.Users)
            {
                user.OCtx.GetCallbackChannel<IContract_Callback>().ContractCallback(msg);
            }
        }
        #endregion

        #region IDataContract_Service
        public Task GetTask(Guid guid)
        {
            return Storage.Tasks.FirstOrDefault(item => item.Guid == guid);
        }

        public ObservableCollection<Task> GetTasks()
        {
            return Storage.Tasks;
        }

        public bool SetTask(Task task)
        {
            if (task == null)
                return false;

            int index = Storage.Tasks.IndexOf(Storage.Tasks.FirstOrDefault(iten => iten.Guid == task.Guid));

            Storage.Tasks[index] = task;
            Storage.Task = task;

            // Callback
            string msg = "пользователь " + Storage.Task.User.Name + " "
            + Storage.Task.User.Description + " обновил задачу "
            + Storage.Task.Title + " " + Storage.Task.Guid;

            DataContract_Callback_Task(msg, Storage.Task);

            return true;
        }
        public bool SetTasks(ObservableCollection<Task> tasks)
        {
            if (tasks == null)
                return false;

            Storage.Tasks = tasks;

            // Callback
            string msg = "пользователь " + Storage.Task.User.Name + " "
            + Storage.Task.User.Description + " обновил задачу "
            + Storage.Task.Title + " " + Storage.Task.Guid;

            DataContract_Callback_AllTasks(msg, Storage.Tasks);

            return true;
        }
        #endregion

        #region IDataContract_Callback
        private void DataContract_Callback_Task(string msg, Task task)
        {
            foreach (User user in Storage.Users)
            {
                try
                {
                    if (user.Guid == task.User.Guid)
                        continue;

                    user.OCtx.GetCallbackChannel<IDataContract_Callback>().DataContractCallback_Task(msg, task);
                }
                catch { }
            }
        }
        private void DataContract_Callback_AllTasks(string msg, ObservableCollection<Task>  tasks)
        {
            foreach (User user in Storage.Users)
            {
                try
                {
                    user.OCtx.GetCallbackChannel<IDataContract_Callback>().DataContractCallback_AllTasks(msg, tasks);
                }
                catch(Exception ex) {
                    Storage.Log.Add("AllTasks " + ex.Message);
                }
            }
        }
        #endregion

        #region IObserver
        public void UpdateProperty(Type type, FlagAccess flag)
        {
            if(flag == FlagAccess.service)
            {
                if (type == typeof(ObservableCollection<Task>))
                {
                    string msg = "коллекция задач";

                    DataContract_Callback_AllTasks(msg, Storage.Tasks);
                }

                //if (type == typeof(Task))
                //{
                //    string msg = "пользователь " + Storage.Task.User.Name + " "
                //    + Storage.Task.User.Description + " обновил задачу "
                //    + Storage.Task.Title + " " + Storage.Task.Guid;
                //
                //    DataContract_Callback_Task(msg, Storage.Task);
                //}

                //if (type == typeof(ObservableCollection<User>))
                //{

                //}

                //if (type == typeof(ObservableCollection<string>))
                //{

                //}
            }
        }
        #endregion
    }
}

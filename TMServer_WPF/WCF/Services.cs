using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TMServer_WPF.CORE;

namespace TMServer_WPF.WCF
{
    class Services : IContract_Service, IDataContract_Service
    {
        #region Internal method
        Storage Storage = Storage.GetStorage();

        public Services()
        {

        }
        #endregion


        #region IContract_Service
        public User Connect(string name, string host)
        {
            User user = Storage.Users.FirstOrDefault(item => item.Host == host && item.Name == name);

            if(user == null)
            {
                user = new User();

                //Add data in user [Name, Host, Description, Guid, OCtx]
                user.Name = name;
                user.Host = host;
                user.Description = GetDescription(host);
                user.Guid = Guid.NewGuid();
                user.OCtx = OperationContext.Current;

                Storage.Users.Add(user);
            }

            return user;
        }

        public bool Disconnect(Guid guid)
        {
            User user = Storage.Users.FirstOrDefault(item => item.Guid == guid);

            if(user != null)
            {
                Storage.Users.Remove(user);
                return true;
            }

            return false;

        }

        // Call ComputersInLocalNetwork.GetServerList and finde records 
        private string GetDescription(string host)
        {
            #region Work
            //Dictionary<string, string> hosts = ComputersInLocalNetwork.GetServerList(ComputersInLocalNetwork.SV_101_TYPES.SV_TYPE_ALL);
            //KeyValuePair<string,string> h = hosts.FirstOrDefault(item => item.Key == host);
            //if (h.Key != null)
            //    return h.Value;
            //else
            //    return "unknown";
            #endregion

            return "test description";
        }
        
        #endregion

        #region IDataContract_Service
        public CORE.Task GetTask(Guid guid)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<CORE.Task> GetTasks()
        {
           return Tests.Datas_Test.GetTasks();
        }

        public bool SetTask(CORE.Task task)
        {
            throw new NotImplementedException();
        }

        #endregion



    }
}

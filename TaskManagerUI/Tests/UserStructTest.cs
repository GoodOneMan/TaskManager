using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerUI.Core;
using TaskManagerUI.MVVM.Model;

namespace TaskManagerUI.Tests
{
    class UserStructTest
    {
        public static UserStruct GetCurrentUser()
        {
            var list = ComputersInLocalNetwork.GetServerList(ComputersInLocalNetwork.SV_101_TYPES.SV_TYPE_ALL);
            UserStruct userStruct = null;

            foreach(string line in list)
            {
                string[] l = line.Split('|');

                if(l[0] == Environment.UserName)
                {
                    userStruct = new UserStruct()
                    {
                        Host = l[0],
                        Name = l[1],
                        UserGuid = Guid.NewGuid(),
                        Tasks = new ObservableCollection<TaskStruct>()
                    };

                    break;
                }
            }

            if(userStruct == null)
            {
                userStruct = new UserStruct()
                {
                    Host = "",
                    Name = Environment.UserName,
                    UserGuid = Guid.NewGuid(),
                    Tasks = new ObservableCollection<TaskStruct>()
                };
            }
            return userStruct;
        }
    }
}

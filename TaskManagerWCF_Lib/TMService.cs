using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using TaskManagerWCF_Lib.Struct;

namespace TaskManagerWCF_Lib
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TMService : ITMService
    {
        public static List<UserStruct> users = new List<UserStruct>();

        public Guid Connect(UserStruct _user)
        {
            var user = users.FirstOrDefault(u => u.UserGuid == _user.UserGuid);

            if (user != null)
                return user.UserGuid;

            _user.UserGuid = Guid.NewGuid();
            _user.OperationContext = OperationContext.Current;
            
            users.Add(_user);

            return _user.UserGuid;
        }

        public void Disconnect(Guid guid)
        {
            var user = users.FirstOrDefault(u => u.UserGuid == guid);

            if (user != null)
                users.Remove(user);
        }

        public void UpdataTasks(TaskStruct task)
        {
            throw new NotImplementedException();
        }
    }
}

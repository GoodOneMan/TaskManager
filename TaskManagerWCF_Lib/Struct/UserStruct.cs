using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace TaskManagerWCF_Lib.Struct
{
    [DataContract]
    public class UserStruct
    {
        [DataMember]
        public Guid UserGuid { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Host { get; set; }
        [DataMember]
        public ICollection<TaskStruct> Tasks { get; set; }

        public OperationContext OperationContext { get; set; }
    }

}

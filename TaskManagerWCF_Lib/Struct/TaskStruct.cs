using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace TaskManagerWCF_Lib.Struct
{
    [DataContract]
    public class TaskStruct
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string User { get; set; }
        [DataMember]
        public bool IsChecked { get; set; }
        [DataMember]
        public bool State { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public List<string> CommentCard { get; set; }
        [DataMember]
        public Guid GuidTask { get; set; }
    }
}

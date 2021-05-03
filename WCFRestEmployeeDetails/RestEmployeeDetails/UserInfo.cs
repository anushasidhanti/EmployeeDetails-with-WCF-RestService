using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestEmployeeDetails
{
    [DataContract]
    public class UserInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]

        public ICollection<Roles> UserRole { get; set; }
    }
    

    
}
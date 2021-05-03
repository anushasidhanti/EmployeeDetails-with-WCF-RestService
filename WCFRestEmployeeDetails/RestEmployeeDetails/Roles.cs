using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestEmployeeDetails
{
    [DataContract]
    public class Roles
    {
        [DataMember]
        public int Id { get; set; }


        [DataMember]
        public int UserId { get; set; }


        [DataMember]
        public string Role { get; set; }


        [DataMember]
        public  UserInfo UserTable { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class UserModifiedReq
    {
        public string name { set; get; }
        public string college { set; get; }
        public string department { get; set; }
        public int gender { set; get; }
        public string avater { get; set; }
        public string description { set; get; }
        public string birthday { set; get; }
        public string phone { get; set; }
        public string email { get; set; }
        public string primarySchool { get; set; }
        public string juniorHighSchool { get; set; }
        public string highSchool { get; set; }
        public string university { get; set; }
        public int admin { get; set; }
        public HobbyReq hobbyReq { get; set; }
       
    }
}

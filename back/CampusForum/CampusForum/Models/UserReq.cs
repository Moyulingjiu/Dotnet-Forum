using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class UserReq
    {
        public long studentId { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string college { set; get; }
        public string department { get; set; }
        public int gender { set; get; }
        public string avater { get; set; }
        public string description { set; get; }
        public string birthday { set; get; }
        public string phone { get; set; }
        public string email { get; set; }
    }
}

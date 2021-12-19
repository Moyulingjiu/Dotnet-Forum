using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace CampusForum.Models
{
    public class User
    {
        [Key]  
        public long student_id { set; get; }
        public string password { get; set; }
        public string name { set; get; }
        public string college { set; get; }
        public int gender { set; get; }
        public string avater { get; set; }
        public string description { set; get; }
        public DateTime birthday { set; get; }
        public string phone { get; set; }
        public string email { get; set; }
        public int sign_state { get; set; }
        public int disable { get; set; }
        //public DateTime gmt_create { set; get; }
        //public DateTime gmt_modified { set; get; }


        public User(User user)
        {
            this.student_id = user.student_id;
            this.password = user.password;
            this.name = user.name;
            this.college = user.college;
            this.gender = user.gender;
            this.avater = user.avater;
            this.description = user.description;
            this.birthday = user.birthday;
            this.phone = user.phone;
            this.email = user.email;
            this.sign_state = user.sign_state;
            this.disable = user.disable;
        }

        public User()
        {
        }
    }
}

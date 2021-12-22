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
        public long id { set; get; }
        public long student_id { set; get; }
        public string password { get; set; }
        public string name { set; get; }
        public string college { set; get; }
        public string department { get; set; }
        public int gender { set; get; }
        public string avater { get; set; }
        public string description { set; get; }
        public string birthday { set; get; }
        public string phone { get; set; }
        public string email { get; set; }
        public string primary_school { get; set; }
        public string junior_high_school { get; set; }
        public string high_school { get; set; }
        public string university { get; set; }
        public int admin { get; set; }
        public int sign_state { get; set; }
        public int disable { get; set; }
        public DateTime gmt_create { set; get; }
        public DateTime gmt_modified { set; get; }

        public User()
        {

        }

        public User(User user)
        {
            this.student_id = user.student_id;
            this.password = user.password;
            this.name = user.name;
            this.college = user.college;
            this.department = user.department;
            this.gender = user.gender;
            this.avater = user.avater;
            this.description = user.description;
            this.birthday = user.birthday;
            this.phone = user.phone;
            this.email = user.email;
            this.primary_school = user.primary_school;
            this.junior_high_school = user.junior_high_school;
            this.high_school = user.high_school;
            this.university = user.university;
            this.admin = user.admin;
            this.sign_state = user.sign_state;
            this.disable = user.disable;
        }

        public User(UserReq userReq)
        {
            this.student_id = userReq.studentId;
            this.password = userReq.password;
            this.name = userReq.name;
            this.college = userReq.college;
            this.department = userReq.department;
            this.gender = userReq.gender;
            this.avater = userReq.avater;
            this.description = userReq.description;
            this.birthday = userReq.birthday;
            this.phone = userReq.phone;
            this.email = userReq.email;

            this.sign_state = 0;
            this.disable = 0;

        }

    }
}

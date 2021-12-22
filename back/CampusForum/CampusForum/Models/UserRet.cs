using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class UserRet
    {
        public long id { get; set; }
        public long studentId { set; get; }
        public string name { set; get; }
        public string college { set; get; }
        public string department { get; set; }
        public int gender { set; get; }
        public string avater { get; set; }
        public string description { set; get; }
        public string  birthday { set; get; }
        public string phone { get; set; }
        public string email { get; set; }
        public string primarySchool { get; set; }
        public string juniorHighSchool { get; set; }
        public string highSchool { get; set; }
        public string university { get; set; }
        public int admin { get; set; }
        //public Hobby hobby { get; set; }
        public bool follow { get; set; }
        public int follower { get; set; }
        public int following { get; set; }
        public DateTime gmt_create { set; get; }
        public DateTime gmt_modified { set; get; }

        public UserRet()
        {

        }

        public UserRet(User user/*,Hobby hobby*/, bool follow,int follower,int following)
        {
            this.id = user.id;
            this.studentId = user.student_id;
            this.name = user.name;
            this.college = user.college;
            this.department = user.department;
            this.gender = user.gender;
            this.avater = user.avater;
            this.description = user.description;
            this.birthday = user.birthday;
            this.phone = user.phone;
            this.email = user.email;
            this.primarySchool = user.primary_school;
            this.juniorHighSchool = user.junior_high_school;
            this.highSchool = user.high_school;
            this.university = user.university;
            this.admin = user.admin;
            this.follow = follow;
            this.follower = follower;
            this.following = following;
            this.gmt_create = user.gmt_create;
            this.gmt_modified = user.gmt_modified;
            //this.hobby = hobby;
        }
    }
}

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
        public int id { set; get; }
        public long student_number { set; get; }
        public string name { set; get; }
        public string college { set; get; }
        public short grade { set; get; }
        public short sex { set; get; }
        public string photo { set; get; }
        public string sign { set; get; }
        public DateTime birthday { set; get; }
        public short judgestate { set; get; }
        public DateTime create_time { set; get; }
        public DateTime update_time { set; get; }

        public void Update(User user)
        {
            this.id = user.id;
            this.student_number = user.student_number;
            this.name = user.name;
            this.college = user.college;
            this.grade = user.grade;
            this.sex = user.sex;
            this.photo = user.photo;
            this.sign = user.sign;
            this.birthday = user.birthday;
            this.judgestate = user.judgestate;
            this.create_time = user.create_time;
            this.update_time = user.update_time;
        }
    }
}

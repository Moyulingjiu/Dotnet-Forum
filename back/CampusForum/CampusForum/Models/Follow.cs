using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class Follow
    {
        [Key]
        public int id { get; set; }
        public int user_id { get; set; }
        public int follower_id { get; set; }
        public DateTime gmt_create { set; get; }
        public DateTime gmt_modified { set; get; }

        public Follow()
        {

        }

        public Follow(Follow follow)
        {
            this.user_id = follow.user_id;
            this.follower_id = follow.follower_id;
        }





    }
}

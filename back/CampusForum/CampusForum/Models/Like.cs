using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class Like
    {
        [Key]
        public long id { get; set; }
        public long state_id { get; set; }
        public long user_id { get; set; }
        public int disable { get; set; }
        public DateTime gmt_create { set; get; }
        public DateTime gmt_modified { set; get; }

        public Like()
        {

        }

        public Like(Like like)
        {
            this.id = like.id;
            this.state_id = like.state_id;
            this.user_id = like.user_id;
            this.disable = like.disable;
        }
    }
}

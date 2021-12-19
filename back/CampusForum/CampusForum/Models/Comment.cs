using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class Comment
    {
        public int id { get; set; }
        public int state_id { get; set; }
        public long user_id { get; set; }
        public int father_id { get; set; }
        public string text { get; set; }
        public int disable { get; set; }
        public int reply_id { get; set; }
        public DateTime gmt_create { set; get; }
        public DateTime gmt_modified { set; get; }

    }
}

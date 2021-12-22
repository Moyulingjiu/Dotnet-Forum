using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class CommentReq
    {
        public long state_id { get; set; }
        public long father_id { get; set; }
        public long reply_id { get; set; }
        public string text { get; set; }
       
    }
}

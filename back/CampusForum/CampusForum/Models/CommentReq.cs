using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class CommentReq
    {
        public long stateId { get; set; }
        public long fatherId { get; set; }
        public long replyId { get; set; }
        public string text { get; set; }
       
    }
}

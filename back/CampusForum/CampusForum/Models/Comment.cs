using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class Comment
    {
        public long id { get; set; }
        public long state_id { get; set; }
        public long user_id { get; set; }
        public long father_id { get; set; }
        public string text { get; set; }
        public int disable { get; set; }
        public long reply_id { get; set; }
        public DateTime gmt_create { set; get; }
        public DateTime gmt_modified { set; get; }

        public Comment(CommentReq commentReq)
        {
            this.state_id = commentReq.state_id;
            this.father_id = commentReq.father_id;
            this.reply_id = commentReq.reply_id;
            this.text = commentReq.text;
        }

        public Comment()
        {

        }

    }
}

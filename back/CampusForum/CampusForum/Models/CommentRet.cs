using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class CommentRet
    {
        public long commentId { get; set; }
        public long state_id { get; set; }
        public long father_id { get; set; }
        public long reply_id { get; set; }
        public string text { get; set; }
        public long userId { get; set; }
        public string userName { get; set; }
        public string userAvater { get; set; }
        public DateTime gmt_create { set; get; }
        public DateTime gmt_modified { set; get; }

        public CommentRet(Comment comment,User user)
        {
            this.commentId = comment.id;
            this.state_id = comment.state_id;
            this.father_id = comment.father_id;
            this.reply_id = comment.reply_id;
            this.text = comment.text;
            this.userId = comment.user_id;
            this.userAvater = user.avater;
            this.userName = user.name;
            this.gmt_create = comment.gmt_create;
            this.gmt_modified = comment.gmt_modified;
        }
    }
}

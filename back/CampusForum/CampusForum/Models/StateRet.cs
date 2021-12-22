using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class StateRet
    {
        public long id { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public int shareState { get; set; }
        public bool like { get; set; }
        public int likeNumber { get; set; }
        public long userId { get; set; }
        public string userName { get; set; }
        public string userAvater { get; set; }
        public string gmt_create { set; get; }
        public string gmt_modified { set; get; }

        public StateRet()
        {

        }
        public StateRet(State state,StateText stateText,User user,int likenum,bool like)
        {
            this.id = state.id;
            this.title = state.title;
            this.shareState = state.share_state;
            this.text = stateText.text;
            this.like = like;
            this.likeNumber = likenum;
            this.userId = state.user_id;
            this.userName = user.name;
            this.userAvater = user.avater;

            //string createTimeSpan = (DateTime.Now - state.gmt_create).ToString();
            TimeSpan createTimeSpan = DateTime.Now.Subtract(state.gmt_create);
            TimeSpan modifiedTimeSpan = DateTime.Now - state.gmt_modified;

            if (createTimeSpan.TotalDays < 1) this.gmt_create = "几小时前";
            else if (createTimeSpan.TotalDays < 7) this.gmt_create = "几天前";
            else this.gmt_create = state.gmt_create.ToString();

            if (modifiedTimeSpan.TotalDays < 1) this.gmt_modified = "几小时前";
            else if (modifiedTimeSpan.TotalDays < 7) this.gmt_modified = "几天前";
            else this.gmt_modified = state.gmt_modified.ToString();

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class State
    {
        [Key]
        public long id { get; set; }
        public long user_id { get; set; }
        public string title { get; set; }
        public int share_state { get; set; }
        public int disable { get; set; }
        public DateTime gmt_create { set; get; }
        public DateTime gmt_modified { set; get; }

        public State()
        {

        }

        public State(State state)
        {
            this.id = state.id;
            this.user_id = state.user_id;
            this.title = state.title;
            this.share_state = state.share_state;
            this.disable = state.disable;
        }

    }
}

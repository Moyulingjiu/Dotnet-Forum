using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class StateText
    {
        [Key]
        public long id { get; set; }
        public long state_id { get; set; }
        public string text { get; set; }
        public int disable { get; set; }
        public DateTime gmt_create { set; get; }
        public DateTime gmt_modified { set; get; }

        public StateText()
        {

        }

        public StateText(StateText stateText)
        {
            this.id = stateText.id;
            this.state_id = stateText.state_id;
            this.text = stateText.text;
            this.disable = stateText.disable;
        }

    }
}

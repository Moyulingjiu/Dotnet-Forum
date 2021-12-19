using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class StateReq
    {
        public string title { get; set; }
        public string text { get; set; }
        public int shareState { get; set; }

        public StateReq()
        {

        }

        public StateReq(StateReq stateReq)
        {
            this.title = stateReq.title;
            this.text = stateReq.text;
            this.shareState = stateReq.shareState;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class LikeGroup
    {
        public long state_id { get; set; }
        public int count { get; set; }

        public LikeGroup(long state_id,int count)
        {
            this.state_id = state_id;
            this.count = count;
        }
    }
}

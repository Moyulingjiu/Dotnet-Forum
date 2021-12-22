using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class Likes
    {
        public long stateId { get; set; }
        public int count { get; set; }
        public DateTime gmt_create { get; set; }

        public Likes(long stateId,int count,DateTime gmt_create)
        {
            this.stateId = stateId;
            this.count = count;
            this.gmt_create = gmt_create;
        }
    }
}

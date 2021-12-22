using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class Check
    {
        [Key]
        public long id { get; set; }
        public long user_id { get; set; }
        public long reviewer_id { get; set; }
        public int state { get; set; }
        public DateTime gmt_create { get; set; }
        public DateTime gmt_modified { get; set; }
    }
}

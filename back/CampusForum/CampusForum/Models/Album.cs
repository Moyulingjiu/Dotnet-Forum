using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CampusForum.Models
{
    public class Album
    {
        [Key]
        public long id { set; get; }

        public long user_id { set; get; }
        public string name { set; get; }
        public string description { set; get; }
        public string cover { set; get; }
        public int disable { set; get; }
        public DateTime gmt_create { set; get; }
        public DateTime gmt_modified { set; get; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CampusForum.Models
{
    public class Album_picture
    {
        [Key]
        public long id { set; get; }

        public long album_id { set; get; }
        public string name { set; get; }
        public string url { set; get; }
        public DateTime gmt_create { set; get; }
        public DateTime gmt_modified { set; get; }
    }
}

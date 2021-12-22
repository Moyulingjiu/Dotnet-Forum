using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class Hobby
    {
        [Key]
        public long id { get; set; }
        public long user_id { get; set; }
        public string music { get; set; }
        public string book { get; set; }
        public string movie { get; set; }
        public string game { get; set; }
        public string comic { get; set; }
        public string sport { get; set; }
        public string other { get; set; }
        public DateTime gmt_create { get; set; }
        public DateTime gmt_modified { get; set; }

        public Hobby()
        {

        }
        public Hobby(HobbyReq hobbyReq)
        {
            this.music = hobbyReq.music;
            this.book = hobbyReq.book;
            this.movie = hobbyReq.movie;
            this.game = hobbyReq.game;
            this.comic = hobbyReq.comic;
            this.sport = hobbyReq.sport;
            this.other = hobbyReq.other;
        }

        public void changeHobby(HobbyReq hobbyReq)
        {
            this.music = hobbyReq.music;
            this.book = hobbyReq.book;
            this.movie = hobbyReq.movie;
            this.game = hobbyReq.game;
            this.comic = hobbyReq.comic;
            this.sport = hobbyReq.sport;
            this.other = hobbyReq.other;
        }
    }
}

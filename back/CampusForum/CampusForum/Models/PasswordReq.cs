using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class PasswordReq
    {
        public string old_password { get; set; }
        public string new_password { get; set; }
    }
}

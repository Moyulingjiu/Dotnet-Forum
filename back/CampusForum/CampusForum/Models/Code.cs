using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Models
{
    public class Code
    {
        public int code { get; set; }
        public string msg { get; set; }
        public Object data { get; set; }

        public Code()
        {

        }

        public Code(int code, string msg, Object data = null)
        {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }

        static public long TokenToId(string token)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
            string idStr = jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
            long id = long.Parse(idStr);
            return id;
        }
    }
}

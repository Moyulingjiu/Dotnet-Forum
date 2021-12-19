using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using CampusForum.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private CoreDbContext _coreDbContext;

        public UserController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public Code register(User user)
        {

            using (CoreDbContext coreDbContext = new CoreDbContext())
            {
                //todo查询数据库是否已有当前用户
                User existUser = _coreDbContext.Set<User>().Find(user.student_id);
                //如果数据库中没有当前用户，则新建用户生成token并将新用户存入数据库
                if (existUser == null)
                {
                    string token = generateToken(user.student_id, user.name);

                    //对密码加密处理
                    RSAKey.createRSAKey();
                    user.password = RSAKey.RSAEncrypt(user.password);

                    //写数据库
                    _coreDbContext.Set<User>().Add(user);
                    _coreDbContext.SaveChanges();

                    return new Code(200, "成功", new { id = user.student_id, token = token });

                }
                else
                {
                    return new Code(403, "用户已存在", null);
                }

            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpGet("login")]
        public Code login(long student_id, string password)
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {

                User user = _coreDbContext.Set<User>().Find(student_id);
                if (user != null)
                {
                    if (user.password == password)
                    {
                        string token = generateToken(student_id, user.name);
                        return new Code(200, "成功", new { token = token });
                    }
                    else return new Code(403, "密码错误", null);
                }
                else return new Code(404, "用户不存在", null);

            }

        }


        private string generateToken(long student_id, string name)
        {
            var claims = new Claim[]
            {
                new Claim("ID",student_id.ToString()),
                new Claim("Name",name),
            };

            string key = "dotnet_forum2021";
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: "https://localhost:44330",
                audience: "https://localhost:8080",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }

    }
}
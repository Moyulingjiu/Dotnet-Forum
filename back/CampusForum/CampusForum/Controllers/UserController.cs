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
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers
{
    [Route("/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;
        public UserController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }



        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public Code register(UserReq userReq)
        {
            
            using(CoreDbContext coreDbContext = new CoreDbContext())
            {
                //查询数据库是否已有当前用户
                User existUser = _coreDbContext.Set<User>().Where(d => d.student_id == userReq.studentId).FirstOrDefault();

                //如果数据库中没有当前用户，则新建用户生成token并将新用户存入数据库
                if (existUser == null)
                {
                    string token = generateToken(userReq.studentId, userReq.name);

                    //对密码加密处理
                    RSAKey.createRSAKey();
                    userReq.password = RSAKey.RSAEncrypt(userReq.password);

                    User user = new User(userReq);
                    user.gmt_create = DateTime.Now;
                    user.gmt_modified = DateTime.Now;
                    //写数据库
                    _coreDbContext.Set<User>().Add(user);
                    _coreDbContext.SaveChanges();
                    long userId = user.id;

                    return new Code(200, "成功", new { id = userId, token = token });
                }

                return new Code(403, "用户已存在", null);
            
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpGet("login")]
        public Code login(long studentId, string password)
        {
            using(CoreDbContext _coreDbContext = new CoreDbContext())
            {

                User user = _coreDbContext.Set<User>().Where(d => d.student_id == studentId).FirstOrDefault();
                
                if (user != null)
                {
                    if (user.disable == 1) return new Code(404, "用户已被删除", false);
                    //密码加密
                    RSAKey.createRSAKey();
                    
                    string decryptPassword = RSAKey.RSADecrypt(user.password);
                    if (decryptPassword == password)
                    {
                        string token = generateToken(studentId, user.name);
                        return new Code(200, "成功", new { token = token, gmt_create = user.gmt_create, gmt_modified = user.gmt_modified });
                    }
                    else return new Code(403, "密码错误", null);
                }
                else return new Code(404, "用户不存在", null);
                
            }
            
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        public Code logout()
        {
            string token = HttpContext.Request.Headers["token"];

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            string studentIdStr;
            try
            {
                JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
                jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
                studentIdStr = jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
            }
            catch(Exception)
            {
                return new Code(404, "token错误", false);
            }

            long studentId = long.Parse(studentIdStr);

            return new Code(200, "成功", true);
        }

        /// <summary>
        /// 更新用户数据
        /// </summary>
        /// <param name="userModifiedReq"></param>
        /// <returns></returns>
        [HttpPost("update/{userId}")]
        public Code updateInfo(UserModifiedReq userModifiedReq)
        {
            string token = HttpContext.Request.Headers["token"];

            long id = JwtToid(token);
            if (id == 0) return new Code(404, "token错误", null);

            string user_idStr = RouteData.Values["userId"].ToString();
            
            long userId = long.Parse(user_idStr);

            //修改自己的信息
            if(userId == 0)
            {
                User tokenUser = _coreDbContext.Set<User>().Find(id);

                tokenUser.modifyUser(userModifiedReq);
                bool nameChange = tokenUser.name == userModifiedReq.name;
                
                Hobby existHobby = _coreDbContext.Set<Hobby>().Where(d => d.user_id == id).FirstOrDefault();
                //新建爱好
                if(existHobby == null)
                {
                    using(CoreDbContext _coreDbContext = new CoreDbContext())
                    {
                        Hobby hobby = new Hobby(userModifiedReq.hobbyReq);
                        hobby.gmt_create = DateTime.Now;
                        hobby.gmt_modified = DateTime.Now;
                        hobby.user_id = id;
                        _coreDbContext.Set<Hobby>().Add(hobby);
                        _coreDbContext.SaveChanges();
                    }
                }
                //修改爱好
                else 
                {
                    existHobby.changeHobby(userModifiedReq.hobbyReq);
                    existHobby.gmt_modified = DateTime.Now;
                    _coreDbContext.Set<Hobby>().Update(existHobby);
                    _coreDbContext.SaveChanges();
                }

                //更新user对应的gmt_modified
                tokenUser.gmt_modified = DateTime.Now;
                _coreDbContext.Set<User>().Update(tokenUser);
                _coreDbContext.SaveChanges();
                
                if(nameChange == true)
                {
                    string newToken = generateToken(id, userModifiedReq.name);
                    return new Code(200, "成功", new { token = newToken });
                }
                return new Code(200, "成功", new { token = token });
           
            }

            User user = _coreDbContext.Set<User>().Find(userId);

            user.modifyUser(userModifiedReq);

            //更新user对应的gmt_modified
            user.gmt_modified = DateTime.Now;

            Hobby modifiedHobby = _coreDbContext.Set<Hobby>().Where(d => d.user_id == userId).FirstOrDefault();
            //新建爱好
            if (modifiedHobby == null)
            {
                Hobby hobby = new Hobby(userModifiedReq.hobbyReq);
                hobby.gmt_create = DateTime.Now;
                hobby.gmt_modified = DateTime.Now;
                hobby.user_id = id;
                _coreDbContext.Set<Hobby>().Add(hobby);
                _coreDbContext.SaveChanges();
            }
            //修改爱好
            else
            {
                modifiedHobby.changeHobby(userModifiedReq.hobbyReq);
                modifiedHobby.gmt_modified = DateTime.Now;
                _coreDbContext.Set<Hobby>().Update(modifiedHobby);
                _coreDbContext.SaveChanges();
            }

            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                _coreDbContext.Set<User>().Update(user);
                _coreDbContext.SaveChanges();

                string newToken = generateToken(user.student_id, user.name);
                return new Code(200, "成功", new { token = newToken });
            }

        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="passwordReq"></param>
        /// <returns></returns>
        [HttpPost("password/update")]
        public Code UpdatePassword(PasswordReq passwordReq)
        {
            string token = HttpContext.Request.Headers["token"];

            long id = JwtToid(token);
            if (id == 0) return new Code(404, "token错误", null);

            User user = _coreDbContext.Set<User>().Find(id);
            RSAKey.createRSAKey();
            string decryptPassword = RSAKey.RSADecrypt(user.password);
            if (passwordReq.old_password != decryptPassword)
            {
                return new Code(403, "旧密码错误", null);
            }
            user.password = RSAKey.RSAEncrypt(passwordReq.new_password);
            _coreDbContext.SaveChanges();
            return new Code(200, "成功修改", null);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete/{userId}")]
        public Code delete()
        {
            using(CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                User user = _coreDbContext.Set<User>().Find(id);

                string user_idStr = RouteData.Values["userId"].ToString();
                long user_id = long.Parse(user_idStr);
                if (id != user_id) return new Code(403, "不能删除他人信息", false);
                //逻辑删除
                user.disable = 1;  
      
                _coreDbContext.Set<User>().Update(user);
                _coreDbContext.SaveChanges();
                return new Code(200, "成功", true);
            }
        }

        /// <summary>
        /// 通过id查询用户 
        /// </summary>
        /// <returns></returns>
        [HttpGet("select/{userId}")]
        public Code getUserById()
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                string user_idStr = RouteData.Values["userId"].ToString();
                long user_id = long.Parse(user_idStr);


                if (user_id == 0)
                {
                    User user = _coreDbContext.Set<User>().Find(id);

                    int follower = _coreDbContext.Set<Follow>().Count(d => d.user_id == user.id);
                    int following = _coreDbContext.Set<Follow>().Count(d => d.follower_id == user.id);
                    Hobby hobby = _coreDbContext.Set<Hobby>().Where(d => d.user_id == id).FirstOrDefault();

                    UserRet userRet = new UserRet(user, hobby, false, follower, following);
                    return new Code(200, "成功", userRet);
                }
                else
                {
                    User user = _coreDbContext.Set<User>().Find(user_id);

                    int follower = _coreDbContext.Set<Follow>().Count(d => d.user_id == user.id);
                    int following = _coreDbContext.Set<Follow>().Count(d => d.follower_id == user.id);
                    Hobby hobby = _coreDbContext.Set<Hobby>().Where(d => d.user_id == id).FirstOrDefault();

                    int count = _coreDbContext.Set<Follow>().Count(d => d.user_id == user_id && d.follower_id == id);
                    UserRet userRet = new UserRet(user, hobby, count != 0, follower, following);
                    return new Code(200, "成功", userRet);
                }
            }
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("selectAll")]
        public Code getAllUsers(int page = 0, int pageSize = 10)
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", new { token = token });

                int total = _coreDbContext.Set<User>().Count();

                int pages = total / pageSize;
                if (total % pageSize != 0) pages += 1;

                if (page > ((pages - 1) > 0 ? (pages - 1) : 0)) return new Code(400, "页码超过记录数", null);

                List<User> userList = _coreDbContext.Set<User>().Skip(page * pageSize).Take(pageSize).ToList();
                List<UserRet> userRetList = new List<UserRet>();
                int follower, following;

                foreach (User user in userList)
                {
                    follower = _coreDbContext.Set<Follow>().Count(d => d.user_id == user.id);
                    following = _coreDbContext.Set<Follow>().Count(d => d.follower_id == user.id);
                    Hobby hobby = _coreDbContext.Set<Hobby>().Where(d => d.user_id == user.id).FirstOrDefault();

                    int count = _coreDbContext.Set<Follow>().Count(d => d.user_id == user.id && d.follower_id == id);
                    UserRet userRet = new UserRet(user, hobby, count != 0, follower, following);

                    userRetList.Add(userRet);
                }

                return new Code(200, "成功", new { total = pages, items = userRetList });
            }
        }

        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("followers")]
        public Code getFollowers(int page = 0, int pageSize = 10)
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                int total = _coreDbContext.Set<Follow>().Count(d => d.user_id == id);
                int pages = total / pageSize;
                if (total % pageSize != 0) pages += 1;

                if (page > ((pages - 1) > 0 ? (pages - 1) : 0)) return new Code(400, "页码超过记录数", null);
                List<Follow> followList = _coreDbContext.Set<Follow>().Where(d => d.user_id == id).Skip(page * pageSize).Take(pageSize).ToList();
                
                int follower, following;
                List<UserRet> userRetList = new List<UserRet>();

                foreach (Follow follow in followList)
                {
                    User user = _coreDbContext.Set<User>().Find(follow.follower_id);

                    follower = _coreDbContext.Set<Follow>().Count(d => d.user_id == follow.follower_id);
                    following = _coreDbContext.Set<Follow>().Count(d => d.follower_id == follow.follower_id);
                    Hobby hobby = _coreDbContext.Set<Hobby>().Where(d => d.user_id == id).FirstOrDefault();

                    int count = _coreDbContext.Set<Follow>().Count(d => d.user_id == follow.follower_id && d.follower_id == id);
                    UserRet userRet = new UserRet(user, hobby, count != 0, follower, following);
                    userRetList.Add(userRet);
                }

                return new Code(200, "成功", new { total = pages, items = userRetList });

            }
        }

        /// <summary>
        /// 模糊条件查询用户 测试查询姓名“张”正确返回 其余未测试
        /// </summary>
        /// <param name="token"></param>
        /// <param name="studentId"></param>
        /// <param name="name"></param>
        /// <param name="college"></param>
        /// <param name="gender"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("selectCondition")]
        public Code getUserByCondition(string token,long studentId,string name,string college,int gender, int page, int pageSize)
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                //string token = HttpContext.Request.Headers["token"];

                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                var queryResult = _coreDbContext.Set<User>().Select(d => d);
                //if(user.id!=null) queryResult = _coreDbContext.Set<User>().Select(d => d);
                //if (user.student_id.ToString() != null) queryResult = queryResult.Where(d => d.student_id == user.student_id);
                if (name != null) queryResult = queryResult.Where(d => d.name.Contains(name)||d.name.StartsWith(name)||d.name.EndsWith(name));
                if (college != null) queryResult = queryResult.Where(d => d.college.Contains(college)||d.college.StartsWith(college)||d.college.EndsWith(college));
                if (gender== 0|| gender == 1|| gender == 2) queryResult = queryResult.Where(d => d.gender == gender);
                List<User> queryUser = queryResult.ToList();

                int total = queryUser.Count();
                if (total == 0) return new Code(200, "成功", null);
                int pages = total / pageSize;
                if (total % pageSize != 0) pages += 1;
                if (page > ((pages - 1) > 0 ? (pages - 1) : 0)) return new Code(400, "页码超过记录数", null);
                
                int follower, following;
                List<UserRet> userRetList = new List<UserRet>();

                for(int i = page * pageSize; i < total; i++)
                {
                    User user = _coreDbContext.Set<User>().Find(queryUser[i].student_id);

                    follower = _coreDbContext.Set<Follow>().Count(d => d.user_id == queryUser[i].student_id);
                    following = _coreDbContext.Set<Follow>().Count(d => d.follower_id == queryUser[i].student_id);
                    Hobby hobby = _coreDbContext.Set<Hobby>().Where(d => d.user_id == id).FirstOrDefault();

                    UserRet userRet = new UserRet(user, hobby, true, follower, following);
                    userRetList.Add(userRet);
                }

                return new Code(200, "成功", new { total = pages, items = userRetList });
            }
        }


        /// <summary>
        /// 获取关注列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("followings")]
        public Code getFollowings(int page = 0, int pageSize = 10)
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                int total = _coreDbContext.Set<Follow>().Count(d => d.follower_id == id);
                int pages = total / pageSize;
                if (total % pageSize != 0) pages += 1;

                if (page > ((pages - 1) > 0 ? (pages - 1) : 0)) return new Code(400, "页码超过记录数", null);
                List<Follow> followList = _coreDbContext.Set<Follow>().Where(d => d.follower_id == id).Skip(page * pageSize).Take(pageSize).ToList();

                int follower, following;
                List<UserRet> userRetList = new List<UserRet>();

                foreach(Follow follow in followList)
                {
                    User user = _coreDbContext.Set<User>().Find(follow.user_id);

                    follower = _coreDbContext.Set<Follow>().Count(d => d.user_id == follow.user_id);
                    following = _coreDbContext.Set<Follow>().Count(d => d.follower_id == follow.user_id);
                    Hobby hobby = _coreDbContext.Set<Hobby>().Where(d => d.user_id == id).FirstOrDefault();

                    UserRet userRet = new UserRet(user, hobby, true, follower, following);
                    userRetList.Add(userRet);
                }

                return new Code(200, "成功", new { total = pages, items = userRetList });

            }
        }


        /// <summary>
        /// 关注某人
        /// </summary>
        /// <returns></returns>
        [HttpPost("follow/{userId}")]
        public Code follow()
        {
            using(CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                string user_idStr = RouteData.Values["userId"].ToString();
                long user_id = long.Parse(user_idStr);

                //user_id是否存在
                User user = _coreDbContext.Set<User>().Find(user_id);
                if(user == null)
                {
                    return new Code(404, "用户不存在", false);
                }

                if (user_id == id) return new Code(400, "不能关注自己", false);

                //是否已关注过
                int count = _coreDbContext.Set<Follow>().Count(d => d.follower_id == id && d.user_id == user_id);

                //未关注
                if(count == 0)
                {
                    Follow follow = new Follow();
                    follow.follower_id = id;
                    follow.user_id = user_id;
                    follow.gmt_create = DateTime.Now;
                    follow.gmt_modified = DateTime.Now;

                    _coreDbContext.Set<Follow>().Add(follow);
                    _coreDbContext.SaveChanges();

                    return new Code(200, "成功", true);
                }

                //不可重复关注
                return new Code(400, "不可重复关注", false);
            }
        }

        /// <summary>
        /// 取关某人
        /// </summary>
        /// <returns></returns>
        [HttpPost("unfollow/{userId}")]
        public Code unfollow()
        {
            using(CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                string user_idStr = RouteData.Values["userId"].ToString();
                long user_id = long.Parse(user_idStr);

                User user = _coreDbContext.Set<User>().Find(user_id);
                if (user == null) return new Code(404, "用户不存在", false);

                //是否关注过
                int count = _coreDbContext.Set<Follow>().Count(d => d.user_id == user_id && d.follower_id == id);

                if (count == 0) return new Code(400, "尚未关注无法执行取关操作", false);

                //取关操作
                Follow follow = _coreDbContext.Set<Follow>().Where(d => d.user_id == user_id && d.follower_id == id).FirstOrDefault();

                _coreDbContext.Set<Follow>().Remove(follow);
                _coreDbContext.SaveChanges();
                return new Code(200, "成功", true);

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


        private long JwtToid(string token)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            string studentIdStr;
            try
            {
                JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
                jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
                studentIdStr = jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
            }
            catch (Exception)
            {
                return 0;
            }

            long studentId = long.Parse(studentIdStr);
            long id = _coreDbContext.Set<User>().Where(d => d.student_id == studentId).FirstOrDefault().id;

            return id;
        }

    }
}
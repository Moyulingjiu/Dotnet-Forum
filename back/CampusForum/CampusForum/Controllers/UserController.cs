using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using CampusForum.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace WebApi.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;
        
        public UserController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        [Route("通过id获取用户信息")]
        [HttpGet]
        public User GetUsersById(int id)
        {
            return _coreDbContext.Set<User>().Find(id);
        }

        [Route("注册")]
        [HttpPost]
        public void RegisterUser(User newuser)
        {
            newuser.judgestate = 0;
            _coreDbContext.Set<User>().Add(newuser);
            _coreDbContext.SaveChanges();
        }

        [Route("通过用户的注册")]
        [HttpPut]
        public void RegisterPass(int id)
        {
            _coreDbContext.Set<User>().Find(id).judgestate = 1;
            _coreDbContext.SaveChanges();
        }

        [Route("不通过用户的注册")]
        [HttpPut]
        public void RegisterNotPass(int id)
        {
            if (_coreDbContext.Set<User>().Find(id).judgestate == 1) return;
            _coreDbContext.Set<User>().Remove(_coreDbContext.Set<User>().Find(id));
            _coreDbContext.SaveChanges();
        }

        [Route("修改用户信息")]
        [HttpPut]
        public void UpdateUser(User user)
        {
            _coreDbContext.Set<User>().Find(user.id).Update(user);
            _coreDbContext.SaveChanges();
        }
    }
}
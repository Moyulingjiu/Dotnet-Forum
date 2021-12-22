using CampusForum.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Controllers
{
    [Route("/state")]
    [ApiController]
    public class StateController: ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;

        public StateController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        /// <summary>
        /// 新建状态
        /// </summary>
        /// <param name="stateReq"></param>
        /// <returns></returns>
        [HttpPost("insert")]
        public Code insert(StateReq stateReq)
        {
            using(CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                //解析token
                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                //state表新增记录
                State state = new State();
                state.user_id = id;
                state.title = stateReq.title;
                state.share_state = stateReq.shareState;
                state.gmt_create = DateTime.Now;
                state.gmt_modified = DateTime.Now;

                _coreDbContext.Set<State>().Add(state);
                _coreDbContext.SaveChanges();

                //调用SaveChanges()之后，state.ID的值就是数据库中新加记录对应自增标识列的值
                long stateId = state.id;
                StateText stateText = new StateText();
                stateText.state_id = stateId;
                stateText.text = stateReq.text;
                stateText.gmt_modified = DateTime.Now;
                stateText.gmt_create = DateTime.Now;

                try
                {
                    _coreDbContext.Set<StateText>().Add(stateText);
                    _coreDbContext.SaveChanges();
                }
                catch(Exception)
                {
                    State insertState = _coreDbContext.Set<State>().Find(stateId);
                    _coreDbContext.Set<State>().Remove(insertState);
                    _coreDbContext.SaveChanges();
                    return new Code(500, "无法写入数据库", false);
                }
               
                return new Code(200, "成功", new { id = stateId });

            }
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="stateReq"></param>
        /// <returns></returns>
        [HttpPost("update/{stateId}")]
        public Code updateState(StateReq stateReq)
        {
            using(CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                //解析token
                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                string state_idStr = RouteData.Values["stateId"].ToString();
                long state_id = long.Parse(state_idStr);

                //修改State表
                State state = _coreDbContext.Set<State>().Find(state_id);
                if (state == null) return new Code(404, "没有该记录", false);

                //记录修改前的state表的数据值
                int oldShare = state.share_state;
                string oldTitle = state.title;
                DateTime oldModified = state.gmt_modified;

                state.share_state = stateReq.shareState;
                state.title = stateReq.title;
                state.gmt_modified = DateTime.Now;
                _coreDbContext.Set<State>().Update(state);
                _coreDbContext.SaveChanges();

                //修改stateText表
                try
                {
                    List<StateText> stateList = _coreDbContext.Set<StateText>().Where(d => d.state_id == state_id).ToList();
                    StateText updateStateText = stateList.First();
                    updateStateText.text = stateReq.text;
                    updateStateText.gmt_modified = DateTime.Now;
                    _coreDbContext.Set<StateText>().Update(updateStateText);
                    _coreDbContext.SaveChanges();
                }
                catch(Exception)
                {
                    State rollbackState = _coreDbContext.Set<State>().Find(state_id);
                    rollbackState.share_state = oldShare;
                    rollbackState.title = oldTitle;
                    rollbackState.gmt_modified = oldModified;
                    _coreDbContext.Set<State>().Update(rollbackState);
                    _coreDbContext.SaveChanges();
                    return new Code(500, "无法写入数据库", false);
                }

                return new Code(200, "成功", true);
            }
        }

        /// <summary>
        /// 删除状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete/{stateId}")]
        public Code deleteState()
        {
            using(CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                //解析token
                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                string state_idStr = RouteData.Values["stateId"].ToString();
                long state_id = long.Parse(state_idStr);

                State state = _coreDbContext.Set<State>().Find(state_id);
                if (state == null) return new Code(404, "没有该记录", false);
                if(state.disable == 1) return new Code(404,"状态已被删除",false);
                if(state.user_id !=id) return new Code(403,"只能删除自己的状态",false);
                state.disable = 1;
                _coreDbContext.Set<State>().Update(state);
                _coreDbContext.SaveChanges();

                try
                {
                    List<StateText> stateList = _coreDbContext.Set<StateText>().Where(d => d.state_id == state_id).ToList();
                    StateText updateStateText = stateList.First();
                    updateStateText.disable = 1;
                    _coreDbContext.Set<StateText>().Update(updateStateText);
                    _coreDbContext.SaveChanges();
                }
                catch (Exception)
                {
                    State rollbackState = _coreDbContext.Set<State>().Find(state_id);
                    rollbackState.disable = 0;
                    _coreDbContext.Set<State>().Update(rollbackState);
                    _coreDbContext.SaveChanges();
                    return new Code(500, "无法写入数据库", false);
                }

                return new Code(200, "成功", true);
            }
        }

        /// <summary>
        /// 通过id查询状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("select/{stateId}")]
        public Code getStateById()
        {
            using(CoreDbContext _coreDbContext=new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                //解析token
                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                string state_idStr = RouteData.Values["stateId"].ToString();
                long state_id = long.Parse(state_idStr);

                //State对象
                State state = _coreDbContext.Set<State>().Find(state_id);
                if (state == null) return new Code(404, "没有记录", false);
                if(state.disable == 1) return new Code(404,"状态已被删除",false);

                //StateText对象
                List<StateText> stateList = _coreDbContext.Set<StateText>().Where(d => d.state_id == state_id).ToList();
                StateText stateText = stateList.First();
                
                int likenum = _coreDbContext.Set<Like>().Count(d => d.state_id == state_id && d.disable == 0);

                //User对象
                User user = _coreDbContext.Set<User>().Find(state.user_id);

                bool like;
                int userlike = _coreDbContext.Set<Like>().Count(d => d.state_id == state_id && d.user_id == id && d.disable == 0);
                if (userlike == 0) like = false;
                else like = true;

                //StateRet对象
                StateRet stateRet = new StateRet(state, stateText, user, likenum, like);

                return new Code(200, "成功", stateRet);
            }
        }

        /// <summary>
        /// 查询用户的所有状态
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("selectAll")]
        public Code getAllStates(int page = 0, int pageSize = 10)
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                //解析token
                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                int total = _coreDbContext.Set<State>().Count();
                int pages = total / pageSize;
                if (total % pageSize != 0) pages += 1;

                if (page > ((pages - 1) > 0 ? (pages - 1) : 0)) return new Code(400, "页码超过记录数", null);

                //只返回当前用户未被删除的状态
                List<State> stateList = _coreDbContext.Set<State>().Where(d => d.user_id == id&& d.disable == 0).Skip(page * pageSize).Take(pageSize).ToList();
 
                List<StateRet> stateRetList = new List<StateRet>();
                int likenum, userlike;
                bool like;

                foreach (State state in stateList)
                {
                    StateText stateText = _coreDbContext.Set<StateText>().Where(d => d.state_id == state.id).ToList().First();
                    User user = _coreDbContext.Set<User>().Find(state.user_id);
                    likenum = _coreDbContext.Set<Like>().Count(d => d.state_id == state.id && d.disable == 0);
                    userlike = _coreDbContext.Set<Like>().Count(d => d.state_id == state.id && d.user_id == id && d.disable == 0);
                    if (userlike == 0) like = false;
                    else like = true;

                    StateRet stateRet = new StateRet(state, stateText, user, likenum, like);
                    stateRetList.Add(stateRet);
                }

                return new Code(200, "成功", new { total = pages, items = stateRetList });

            }
        }

        /// <summary>
        /// 条件查询状态 未测试
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="title"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("selectCondition")]
        public Code getStateByCondition(long userId,string userName,string title,int page,int pageSize)
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                var queryResult = _coreDbContext.Set<State>().Select(d => d);
                if(userId.ToString()!=null) queryResult = queryResult.Where(d => d.user_id == userId);
                //if (userName != null) queryResult = queryResult.Where(d => d.name.Contains(name) || d.name.StartsWith(name) || d.name.EndsWith(name));
                if (title != null) queryResult = queryResult.Where(d => d.title.Contains(title) || d.title.StartsWith(title) || d.title.EndsWith(title));
                List<State> queryUser = queryResult.ToList();

                int total = queryUser.Count();
                int pages = total / pageSize;
                if (total % pageSize != 0) pages += 1;
                if (page > ((pages - 1) > 0 ? (pages - 1) : 0)) return new Code(400, "页码超过记录数", null);

                List<StateRet> userRetList = new List<StateRet>();
                int likenum, userlike;
                bool like;

                List<StateRet> stateRetList = new List<StateRet>();
                for (int i = page * pageSize; i < total; i++)
                {
                    State state = _coreDbContext.Set<State>().Find(queryUser[i].id);
                    StateText stateText = _coreDbContext.Set<StateText>().Where(d => d.state_id == state.id).ToList().First();
                    User user = _coreDbContext.Set<User>().Find(state.user_id);
                    likenum = _coreDbContext.Set<Like>().Count(d => d.state_id == state.id && d.disable == 0);
                    userlike = _coreDbContext.Set<Like>().Count(d => d.state_id == state.id && d.user_id == state.user_id && d.disable == 0);
                    if (userlike == 0) like = false;
                    else like = true;

                    StateRet stateRet = new StateRet(state, stateText, user, likenum, like);
                    stateRetList.Add(stateRet);
                }

                return new Code(200, "成功", new { total = pages, items = stateRetList });
            }   
        }

        

        /// <summary>
        /// 主页推荐的状态 未测试
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("recommend")]
        public Code getRecommendState(int page = 0, int pageSize = 10)
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                //解析token
                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                //List<LikeGroup> likeCount = _coreDbContext.Set<Like>().Where(d => d.disable == 0).GroupBy(d => d.state_id).Select(d => new LikeGroup(d.Key, d.Count())).OrderByDescending(d => d.count).ToList();
                List<LikeGroup> likeCount = _coreDbContext.Set<Like>().Where(d => d.disable == 0).GroupBy(d => d.state_id).Select(d => new LikeGroup(d.Key, d.Count())).ToList();


                List<long> stateIds = new List<long>();

                List<Likes> likesList = new List<Likes>();
                foreach(LikeGroup likeGroup in likeCount)
                {
                    State state = _coreDbContext.Set<State>().Find(likeGroup.state_id);
                    Likes likes = new Likes(likeGroup.state_id, likeGroup.count, state.gmt_create);
                    likesList.Add(likes);
                }

                List<long> recommentIds = new List<long>();

                likesList.OrderBy(d => d.count).ThenByDescending(d => d.gmt_create);

                foreach (Likes likes in likesList) recommentIds.Add(likes.stateId);

                List<Follow> followings = _coreDbContext.Set<Follow>().Where(d => d.follower_id == id).ToList();
                
                foreach(Follow follow in followings)
                {
                    List<State> stateList = _coreDbContext.Set<State>().Where(d => d.user_id == follow.user_id).ToList();
                    foreach(State state in stateList)
                    {
                        if (!recommentIds.Contains(state.id)) recommentIds.Add(state.id);
                    }
                }

                int total = recommentIds.Count()/2;
                if(total == 0)
                {
                    List<State> existStateList = _coreDbContext.Set<State>().ToList();
                    foreach (State state in existStateList) recommentIds.Add(state.id);
                    total = recommentIds.Count();
                }
    
                int pages = total / pageSize;
                if (total % pageSize != 0) pages += 1;
                if (page > ((pages - 1) > 0 ? (pages - 1) : 0)) return new Code(400, "页码超过记录数", null);

                List<StateRet> stateRetList = new List<StateRet>();
                int likenum, userlike;
                bool like;
                for (int i = page * pageSize; i < total; i++)
                {
                    State state = _coreDbContext.Set<State>().Find(recommentIds[i]);
                    StateText stateText = _coreDbContext.Set<StateText>().Where(d => d.state_id == state.id).ToList().First();
                    User user = _coreDbContext.Set<User>().Find(state.user_id);
                    likenum = _coreDbContext.Set<Like>().Count(d => d.state_id == state.id && d.disable == 0);
                    userlike = _coreDbContext.Set<Like>().Count(d => d.state_id == state.id && d.user_id == state.user_id && d.disable == 0);
                    if (userlike == 0) like = false;
                    else like = true;

                    StateRet stateRet = new StateRet(state, stateText, user, likenum, like);
                    stateRetList.Add(stateRet);
                }

                return new Code(200, "成功", stateRetList);
            }
        }


        /// <summary>
        /// 点赞状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("like/{stateId}")]
        public Code likeState()
        {
            using(CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                //解析token
                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                string state_idStr = RouteData.Values["stateId"].ToString();
                long state_id = long.Parse(state_idStr);

                //请求的state_id是否存在
                State state = _coreDbContext.Set<State>().Find(state_id);
                if (state == null) return new Code(404, "没有记录", false);
                if (state.user_id == id) return new Code(400, "不可以给自己的状态点赞", false);

                //分为两种状态 在数据库中已经存在记录；在数据库中没有记录
                int likeCount = _coreDbContext.Set<Like>().Count(d => d.user_id == id && d.state_id == state_id);
                if (likeCount==0)
                {
                    Like like = new Like();
                    like.state_id = state_id;
                    like.user_id = id;
                    _coreDbContext.Set<Like>().Add(like);
                    _coreDbContext.SaveChanges();
                    return new Code(200, "成功", true);
                }

                List<Like> likeList = _coreDbContext.Set<Like>().Where(d => d.state_id == state_id&&d.user_id == id).ToList();
                Like existLike = likeList.First();

                if (existLike.disable == 0) return new Code(400, "不可重复点赞", false);

                existLike.disable = 0;
                _coreDbContext.Set<Like>().Update(existLike);
                _coreDbContext.SaveChanges();
                return new Code(200, "成功", true);
            }
        }

        /// <summary>
        /// 取消点赞状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("unlike/{stateId}")]
        public Code unlikeState()
        {
            using(CoreDbContext _coreDbContext= new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                //解析token
                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                string state_idStr = RouteData.Values["stateId"].ToString();
                long state_id = long.Parse(state_idStr);

                State state = _coreDbContext.Set<State>().Find(state_id);
                if (state == null) return new Code(404, "没有记录", false);

                int count = _coreDbContext.Set<Like>().Count(d => d.state_id == state_id && d.user_id == id);
                if (count == 0) return new Code(404, "没有记录", false);

                List<Like> likeList = _coreDbContext.Set<Like>().Where(d => d.state_id == state_id && d.user_id == id).ToList();
                Like existLike = likeList.First();

                if (existLike.disable == 1) return new Code(400, "不可重复取消", false);

                existLike.disable = 1;
                _coreDbContext.Set<Like>().Update(existLike);
                _coreDbContext.SaveChanges();
                return new Code(200, "成功", true);
            }
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

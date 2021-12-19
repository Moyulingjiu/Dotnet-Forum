using CampusForum.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace CampusForum.Controllers
{
    [Route("/comment")]
    [ApiController]
    public class CommentController:ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;

        public CommentController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }


        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="token"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost("insert")]
        public Code insertComment(string token,Comment comment)
        {
            using(CoreDbContext _coreDbContext = new CoreDbContext())
            {
                //解析token
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                string idStr;
                try
                {
                    JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
                    jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
                    idStr = jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
                }
                catch (Exception)
                {
                    return new Code(404, "token错误", false);
                }

                long id = long.Parse(idStr);

                State state = _coreDbContext.Set<State>().Find(comment.state_id);
                if (state == null) return new Code(404, "没有状态记录", false);

                comment.user_id = id;
                _coreDbContext.Set<Comment>().Add(comment);
                _coreDbContext.SaveChanges();
                int commentId = comment.id;
                return new Code(200, "成功", new { id = commentId });
            }
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("delete/{comment_id}")]
        public Code deleteComment(string token)
        {
            using(CoreDbContext _coreDbContext = new CoreDbContext())
            {
                //解析token
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                string idStr;
                try
                {
                    JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
                    jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
                    idStr = jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
                }
                catch (Exception)
                {
                    return new Code(404, "token错误", false);
                }

                long id = long.Parse(idStr);

                string comment_idStr = RouteData.Values["comment_id"].ToString();
                int comment_id = int.Parse(comment_idStr);

                Comment comment = _coreDbContext.Set<Comment>().Find(comment_id);
                if (comment == null) return new Code(404, "没有评论记录", false);

                if (comment.disable == 1) return new Code(400, "不能重复删除评论", false);

                if (comment.user_id != id) return new Code(403, "不能删除他人评论", false);

                if(comment.father_id == 0)
                {
                    List<Comment> commentList = _coreDbContext.Set<Comment>().Where(d => d.father_id == comment.id && d.disable == 0).ToList();
                    foreach(Comment subComment in commentList)
                    {
                        subComment.disable = 1;
                        _coreDbContext.Set<Comment>().Update(subComment);
                        _coreDbContext.SaveChanges();
                    }
                }

                comment.disable = 1;
                _coreDbContext.Set<Comment>().Update(comment);
                _coreDbContext.SaveChanges();
                return new Code(200, "成功", true);
            }
        }

        /// <summary>
        /// 根据id获取评论
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("select/{comment_id}")]
        public Code selectCommentById(string token)
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                //解析token
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                string idStr;
                try
                {
                    JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
                    jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
                    idStr = jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
                }
                catch (Exception)
                {
                    return new Code(404, "token错误", false);
                }

                long id = long.Parse(idStr);

                String comment_idStr = RouteData.Values["comment_id"].ToString();
                int comment_id = int.Parse(comment_idStr);

                Comment comment = _coreDbContext.Set<Comment>().Find(comment_id);
                if (comment == null || comment.disable == 1) return new Code(404, "评论不存在或已被删除", null);

                User user = _coreDbContext.Set<User>().Find(comment.user_id);

                CommentRet commentRet = new CommentRet(comment, user);

                return new Code(200, "成功", commentRet);
            }
        }

        /// <summary>
        /// 获取一个状态的一级评论
        /// </summary>
        /// <param name="token"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("selectAll/{state_id}")]
        public Code getAllComment(string token, int page = 0, int pageSize = 10)
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                //解析token
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                string idStr;
                try
                {
                    JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
                    jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
                    idStr = jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
                }
                catch (Exception)
                {
                    return new Code(404, "token错误", false);
                }

                long id = long.Parse(idStr);

                String state_idStr = RouteData.Values["state_id"].ToString();
                int state_id = int.Parse(state_idStr);

                State state = _coreDbContext.Set<State>().Find(state_id);
                if (state == null) return new Code(404, "没有状态记录", null);

                int total = _coreDbContext.Set<Comment>().Where(d => d.state_id == state_id && d.father_id == 0).Count();

                int pages = total / pageSize;
                if (total % pageSize != 0) pages += 1;

                if(page > pages-1) return new Code(400, "页码超过记录数", null);

                List<Comment> commentList = _coreDbContext.Set<Comment>().Where(d => d.state_id == state_id && d.father_id == 0 && d.disable == 0).Skip(page * pageSize).Take(pageSize).ToList();
                List<CommentRet> commentRetList = new List<CommentRet>();

                foreach(Comment comment in commentList)
                {
                    User user = _coreDbContext.Set<User>().Find(comment.user_id);

                    CommentRet commentRet = new CommentRet(comment, user);
                    commentRetList.Add(commentRet);
                }

                return new Code(200, "成功", new { total = pages, items = commentRetList });
            }
        }

        /// <summary>
        /// 获取一个评论下的所有子评论
        /// </summary>
        /// <param name="token"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("selectAllReply/{comment_id}")]
        public Code getAllReply(string token, int page = 0, int pageSize = 10)
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                //解析token
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                string idStr;
                try
                {
                    JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
                    jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
                    idStr = jwtSecurityToken.Payload.GetValueOrDefault("ID").ToString();
                }
                catch (Exception)
                {
                    return new Code(404, "token错误", false);
                }

                long id = long.Parse(idStr);

                String comment_idStr = RouteData.Values["comment_id"].ToString();
                int comment_id = int.Parse(comment_idStr);

                Comment comment = _coreDbContext.Set<Comment>().Find(comment_id);
                if (comment == null || comment.disable == 1) return new Code(404, "评论不存在或已被删除", null);

                int total = _coreDbContext.Set<Comment>().Where(d => d.father_id == comment_id).Count();
                int pages = total / pageSize;
                if (total % pageSize != 0) pages += 1;

                if (page > pages - 1) return new Code(400, "页码超过记录数", null);

                List<Comment> commentList = _coreDbContext.Set<Comment>().Where(d => d.father_id == comment_id && d.disable == 0).Skip(page * pageSize).Take(pageSize).ToList();
                List<CommentRet> commentRetList = new List<CommentRet>();

                foreach(Comment existComment in commentList)
                {
                    User user = _coreDbContext.Set<User>().Find(existComment.user_id);

                    CommentRet commentRet = new CommentRet(existComment, user);
                    commentRetList.Add(commentRet);
                }
                return new Code(200, "成功", new { total = pages, items = commentRetList });
            }
        }
    }
}

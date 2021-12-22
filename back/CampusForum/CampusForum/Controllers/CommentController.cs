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
        /// <param name="commentReq"></param>
        /// <returns></returns>
        [HttpPost("insert")]
        public Code insertComment(CommentReq commentReq)
        {
            using(CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                //解析token
                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                

                State state = _coreDbContext.Set<State>().Find(commentReq.stateId);
                if (state == null) return new Code(404, "没有状态记录", false);
                if(state.disable == 1) return new Code(404,"状态已被删除",false);

                Comment comment = new Comment(commentReq);
                comment.user_id = id;
                comment.gmt_create =DateTime.Now;
                comment.gmt_modified = DateTime.Now;

                _coreDbContext.Set<Comment>().Add(comment);
                _coreDbContext.SaveChanges();
                long commentId = comment.id;
                return new Code(200, "成功", new { id = commentId });
            }
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete/{commentId}")]
        public Code deleteComment()
        {
            using(CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                //解析token
                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                string comment_idStr = RouteData.Values["commentId"].ToString();
                long comment_id = long.Parse(comment_idStr);

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
        /// <returns></returns>
        [HttpGet("select/{commentId}")]
        public Code selectCommentById()
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                //解析token
                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                long comment_id; 
                try
                {
                    String comment_idStr = RouteData.Values["commentId"].ToString();
                    comment_id = long.Parse(comment_idStr);
                }
                catch(Exception)
                {
                    return new Code(400, "参数错误", false);
                }

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
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("selectAll/{stateId}")]
        public Code getAllComment(int page = 0, int pageSize = 10)
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                //解析token
                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                long state_id; 
                try
                {
                    String state_idStr = RouteData.Values["stateId"].ToString();
                    state_id = long.Parse(state_idStr);
                }
                catch(Exception)
                {
                    return new Code(400, "参数错误", false);
                }


                State state = _coreDbContext.Set<State>().Find(state_id);
                if (state == null) return new Code(404, "没有状态记录", null);

                int total = _coreDbContext.Set<Comment>().Where(d => d.state_id == state_id && d.father_id == 0).Count();

                int pages = total / pageSize;
                if (total % pageSize != 0) pages += 1;

                if (page > ((pages - 1) > 0 ? (pages - 1) : 0)) return new Code(400, "页码超过记录数", null);

                List<Comment> commentList = _coreDbContext.Set<Comment>().Where(d => d.state_id == state_id && d.father_id == 0 && d.disable == 0).Skip(page * pageSize).Take(pageSize).OrderByDescending(d => d.gmt_create).ToList();
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
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("selectAllReply/{commentId}")]
        public Code getAllReply(int page = 0, int pageSize = 10)
        {
            using (CoreDbContext _coreDbContext = new CoreDbContext())
            {
                string token = HttpContext.Request.Headers["token"];

                //解析token
                long id = JwtToid(token);
                if (id == 0) return new Code(404, "token错误", null);

                long comment_id;
                try
                {
                    String comment_idStr = RouteData.Values["commentId"].ToString();
                    comment_id = long.Parse(comment_idStr);
                }
                catch (Exception)
                {
                    return new Code(400, "参数错误", false);
                }

                Comment comment = _coreDbContext.Set<Comment>().Find(comment_id);
                if (comment == null || comment.disable == 1) return new Code(404, "评论不存在或已被删除", null);

                int total = _coreDbContext.Set<Comment>().Where(d => d.father_id == comment_id).Count();
                int pages = total / pageSize;
                if (total % pageSize != 0) pages += 1;

                if (page > ((pages - 1) > 0 ? (pages - 1) : 0)) return new Code(400, "页码超过记录数", null);

                List<Comment> commentList = _coreDbContext.Set<Comment>().Where(d => d.father_id == comment_id && d.disable == 0).Skip(page * pageSize).Take(pageSize).OrderByDescending(d => d.gmt_create).ToList();
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

            long id;
            try
            {
                long studentId = long.Parse(studentIdStr);
                id = _coreDbContext.Set<User>().Where(d => d.student_id == studentId).FirstOrDefault().id;
            }
            catch (Exception)
            {
                return 0;
            }

            return id;
        }
    }
}

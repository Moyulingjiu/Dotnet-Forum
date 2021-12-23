using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using CampusForum.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System;

namespace CampusForum.Controllers
{
    [Route("/album")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;



        public AlbumController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }


        [HttpPost("insert")]
        public Code InsertAlbum(AlbumReq albumReq)
        {
            string token = HttpContext.Request.Headers["token"];
            long user_id = JwtToid(token);
            if (user_id == 0) return new Code(404, "token错误", null);

            int cnt = _coreDbContext.Set<Album>().Where(b => b.user_id == user_id).Count();
            if (cnt > 20) return new Code(403, "创建相册已经达到20个的上限", null);
            Album newAlbum = new Album { user_id = user_id, name = albumReq.name, description = albumReq.description ,cover=albumReq.cover};
            newAlbum.gmt_create = DateTime.Now;
            newAlbum.gmt_modified = DateTime.Now;
            _coreDbContext.Set<Album>().Add(newAlbum);
            _coreDbContext.SaveChanges();
            return new Code(200, "成功", new { album_id = newAlbum.id });
        }

        [HttpPut("update/{album_id}")]
        public Code UpdateAlbumByAlbumId(AlbumReq albumReq)
        {
            string token = HttpContext.Request.Headers["token"];
            string album_idStr = RouteData.Values["album_id"].ToString();
            long album_id = long.Parse(album_idStr);
            long user_id = JwtToid(token);
            if (user_id == 0) return new Code(404, "token错误", null);

            Album album = _coreDbContext.Set<Album>().Single(b => b.id == album_id);
            if (album == null) return new Code(404, "没有这个相册", null);
            if (album.user_id != user_id) return new Code(403, "没有修改权限", null);
            if(albumReq.name!=null)
                album.name = albumReq.name;
            if (albumReq.description != null)
                album.description = albumReq.description;
            if (albumReq.cover != null)
                album.cover = albumReq.cover;
            album.gmt_modified = DateTime.Now;
            _coreDbContext.SaveChanges();
            return new Code(200, "成功", true);
        }

        [HttpDelete("delete/{album_id}")]
        public Code DeleteAlbumByAlbumId()
        {
            string token = HttpContext.Request.Headers["token"];
            string album_idStr = RouteData.Values["album_id"].ToString();
            long album_id = long.Parse(album_idStr);
            long user_id = JwtToid(token);
            if (user_id == 0) return new Code(404, "token错误", null);

            Album album = _coreDbContext.Set<Album>().Single(b => b.id == album_id);
            if (album == null) return new Code(404, "没有这个相册", null);
            if (album.user_id != user_id) return new Code(403, "没有修改权限", null);
            _coreDbContext.Set<Album>().Remove(album);
            _coreDbContext.SaveChanges();
            return new Code(200, "成功", true);
        }


        [HttpGet("select/{album_id}")]
        public Code SelectAlbumByAlbumId()
        {
            string token = HttpContext.Request.Headers["token"];
            string album_idStr = RouteData.Values["album_id"].ToString();
            long album_id;
            try
            {
                album_id = long.Parse(album_idStr);
            }
            catch (Exception)
            {
                return new Code(404, "没有这个相册", null);
            }
            long user_id = JwtToid(token);
            if (user_id == 0) return new Code(404, "token错误", null);

            Album album = _coreDbContext.Set<Album>().Single(b => b.id == album_id);
            if (album == null) return new Code(404, "没有这个相册", null);
            if (album.user_id != user_id) return new Code(403, "没有查看权限", null);
            return new Code(200, "成功", new { id = album.id, name = album.name, description = album.description, cover = album.cover });
        }


        [HttpGet("selectAll")]
        public Code SelectAllAlbum()
        {
            string token = HttpContext.Request.Headers["token"];
            long user_id = JwtToid(token);
            if (user_id == 0) return new Code(404, "token错误", null);

            var albums = _coreDbContext.Set<Album>().Where(b => b.user_id == user_id).Select(s => new { id = s.id, name = s.name, description = s.description, cover = s.cover });
            if (albums == null) return new Code(404, "不存在已创建的相册", null);
            return new Code(200, "成功", new { item = albums });
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

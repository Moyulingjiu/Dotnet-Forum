using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using CampusForum.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

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
        public Code InsertAlbum(string name,string description)
        {
            string token = HttpContext.Request.Headers["token"];
            long user_id = Code.TokenToId(token);
            int cnt = _coreDbContext.Album.Where(b => b.user_id == user_id).Count();
            if (cnt > 20) return new Code(403, "创建相册已经达到20个的上限", null);
            var newAlbum = new Album { user_id = user_id, name = name, description = description };
            _coreDbContext.Album.Add(newAlbum);
            _coreDbContext.SaveChanges();
            return new Code(200, "成功", new { album_id = newAlbum.id });
        }
        
        [HttpPut("update/{album_id}")]
        public Code UpdateAlbumByAlbumId(string name, string description,string cover)
        {
            string token = HttpContext.Request.Headers["token"];
            string album_idStr = RouteData.Values["album_id"].ToString();
            long album_id = int.Parse(album_idStr);
            long user_id = Code.TokenToId(token);
            var album = _coreDbContext.Album.Find(album_id);
            if (album == null) return new Code(404, "没有这个相册", null);
            if (album.user_id != user_id) return new Code(403, "没有修改权限", null);
            album.name = name;
            album.description = description;
            album.cover = cover;
            _coreDbContext.SaveChanges();
            return new Code(200, "成功", true);
        }
        
        [HttpDelete("delete/{album_id}")]
        public Code DeleteAlbumByAlbumId()
        {
            string token = HttpContext.Request.Headers["token"];
            string album_idStr = RouteData.Values["album_id"].ToString();
            long album_id = int.Parse(album_idStr);
            long user_id = Code.TokenToId(token);
            Album album = _coreDbContext.Set<Album>().Find(album_id);
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
            long album_id = int.Parse(album_idStr);
            long user_id = Code.TokenToId(token);
            var album = _coreDbContext.Album.Find(album_id);
            if (album == null) return new Code(404, "没有这个相册", null);
            if (album.user_id != user_id) return new Code(403, "没有查看权限", null);
            return new Code(200, "成功", new { id = album.id, name = album.name, description = album.description, cover = album.cover });
        }

        
        [HttpGet("selectAll")]
        public Code SelectAllAlbum()
        {
            string token = HttpContext.Request.Headers["token"];
            long user_id = Code.TokenToId(token);
            var albums = _coreDbContext.Album.Where(b => b.user_id == user_id).Select(s=>new { id=s.id,name=s.name,description=s.description,cover=s.cover});
            if (albums == null) return new Code(404, "不存在已创建的相册", null);
            return new Code(200, "成功", new { item = albums});
        }
        
    }
    
}

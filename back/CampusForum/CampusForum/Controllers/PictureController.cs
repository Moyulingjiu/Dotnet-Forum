using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using CampusForum.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace CampusForum.Controllers
{
    [Route("/picture")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly CoreDbContext _coreDbContext;

        public PictureController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }
        //需要注意，这里返回与存入数据库的url并不是可以直接调用的，需要在url字符串前加上端口
        //例如我给的url是\image\1.png，但你需要在这个url前加上localhost:xxxx才能直接调用图片
        [HttpPost("insert/{album_id}")]
        public Code InsertPictureByAlbumId([FromForm(Name ="file")] IFormFile photo)
        {
            string token = HttpContext.Request.Headers["token"];
            string album_idStr = RouteData.Values["album_id"].ToString();
            long album_id = long.Parse(album_idStr);
            long user_id = JwtToid(token);
            if (user_id == 0) return new Code(404, "token错误", null);

            Album album = _coreDbContext.Set<Album>().Find(album_id);
            if (album == null) return new Code(404, "没有这个相册", null);
            if (album.user_id != user_id) return new Code(403, "没有使用权限", null);

            string uuid = System.Guid.NewGuid().ToString();
            string url = @"\image" + "\\" + uuid + photo.FileName;
            var newPicture = new Album_picture { album_id = album_id, name = photo.FileName, url = url };
            newPicture.gmt_create = DateTime.Now;
            newPicture.gmt_modified = DateTime.Now;
            _coreDbContext.Set<Album_picture>().Add(newPicture);
            _coreDbContext.SaveChanges();


            string path = @"wwwroot" + url;
            using (var stream = System.IO.File.Create(path))
            {
                photo.CopyTo(stream);
            }
            return new Code(200, "成功", new { album_id = album_id, name = photo.FileName, url = url });
        }

        [HttpDelete("delete/{picture_id}")]
        public Code DeletePictureByPictureId()
        {
            string token = HttpContext.Request.Headers["token"];
            string picture_idStr = RouteData.Values["picture_id"].ToString();
            long picture_id = long.Parse(picture_idStr);
            long user_id = JwtToid(token);
            if (user_id == 0) return new Code(404, "token错误", null);

            Album_picture picture = _coreDbContext.Set<Album_picture>().Single(b => b.id == picture_id);
            if (picture == null) return new Code(404, "没有这张图片", null);
            long uid = _coreDbContext.Set<Album>().Single(b => b.id == picture.album_id).user_id;
            if (uid != user_id) return new Code(403, "没有删除权限", null);
            _coreDbContext.Set<Album_picture>().Remove(picture);
            _coreDbContext.SaveChanges();
            return new Code(200, "成功", null);
        }

        //返回该相册按pageSize分页后第page页的内所有图片，注意page应当从0开始，即第一页page=0
        [HttpGet("selectAll/{album_id}")]
        public Code SelectAllPictureByAlbumId(int page, int pageSize)
        {
            string token = HttpContext.Request.Headers["token"];
            string album_idStr = RouteData.Values["album_id"].ToString();
            long album_id = long.Parse(album_idStr);
            long user_id = JwtToid(token);
            if (user_id == 0) return new Code(404, "token错误", null);

            var album = _coreDbContext.Album.Find(album_id);
            if (album == null) return new Code(404, "没有这个相册", null);
            if (album.user_id != user_id) return new Code(403, "没有使用权限", null);
            var picture = _coreDbContext.Album_picture.Where(c => c.album_id == album_id).Skip(page * pageSize).Take(pageSize).Select(b => new { id = b.id, name = b.name, url = b.url });
            if (picture == null) return new Code(404, "这页没有图片", null);
            return new Code(200, "成功", new { item = picture });
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
        
        [HttpPost("head/insert")]
        public Code InsertHeadPicture([FromForm(Name = "file")] IFormFile photo)
        {
            string token = HttpContext.Request.Headers["token"];
            long user_id = JwtToid(token);
            if (user_id == 0) return new Code(404, "token错误", null);

            string uuid = System.Guid.NewGuid().ToString();
            string url = @"\head" + "\\" + uuid + photo.FileName;
            var newPicture = new Album_picture { name = photo.FileName, url = url };
            newPicture.gmt_create = DateTime.Now;
            newPicture.gmt_modified = DateTime.Now;
            _coreDbContext.Set<Album_picture>().Add(newPicture);
            _coreDbContext.SaveChanges();

            string path = @"wwwroot" + url;
            using (var stream = System.IO.File.Create(path))
            {
                photo.CopyTo(stream);
            }
            var user = _coreDbContext.Set<User>().Single(b => b.id == user_id);
            user.avater = url;
            _coreDbContext.SaveChanges();
            return new Code(200, "成功", new { name = photo.FileName, url = url });
        }
    }

}

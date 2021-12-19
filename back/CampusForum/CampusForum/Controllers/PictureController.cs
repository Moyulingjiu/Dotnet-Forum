using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using CampusForum.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

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
        [Route("/insert/{album_id}")]
        [HttpPost]
        public Code InsertPictureByAlbumId(IFormFile photo)
        {
            string token = HttpContext.Request.Headers["token"];
            string album_idStr = RouteData.Values["album_id"].ToString();
            long album_id = int.Parse(album_idStr);
            long user_id = Code.TokenToId(token);
            var album = _coreDbContext.Album.Find(album_id);
            if (album == null) return new Code(404, "没有这个相册", null);
            if (album.user_id != user_id) return new Code(403, "没有使用权限", null);
            string url = @"\image" + "\\" + photo.FileName;
            var newPicture = new Album_picture { album_id = album_id, name = photo.FileName, url = url };
            _coreDbContext.Album_picture.Add(newPicture);
            _coreDbContext.SaveChanges();
            string path = @"wwwroot\image"+"\\"+photo.FileName;
            using (var stream = System.IO.File.Create(path))
            {
                photo.CopyTo(stream);
            }
            return new Code(200, "成功", new { album_id = album_id, name = photo.FileName, url = url });
        }
        
        [Route("/deletepic/{picture_id}")]
        [HttpDelete]
        public Code DeletePictureByPictureId()
        {
            string token = HttpContext.Request.Headers["token"];
            string picture_idStr = RouteData.Values["picture_id"].ToString();
            long picture_id = int.Parse(picture_idStr);
            long user_id = Code.TokenToId(token);
            var picture = _coreDbContext.Album_picture.Single(b => b.id == picture_id);
            if (picture == null) return new Code(404, "没有这张图片", null);
            long uid = _coreDbContext.Album.Single(b => b.id == picture.album_id).user_id;
            if(uid!=user_id) return new Code(403, "没有删除权限", null);
            _coreDbContext.Set<Album_picture>().Remove(picture);
            _coreDbContext.SaveChanges();
            return new Code(200, "成功", null);
        }
        
        //返回该相册按pageSize分页后第page页的内所有图片，注意page应当从0开始，即第一页page=0
        [Route("/selectAll/{album_id}")]
        [HttpGet]
        public Code SelectAllPictureByAlbumId(int page,int pageSize)
        {
            string token = HttpContext.Request.Headers["token"];
            string album_idStr = RouteData.Values["album_id"].ToString();
            long album_id = int.Parse(album_idStr);
            long user_id = Code.TokenToId(token);
            var album = _coreDbContext.Album.Find(album_id);
            if (album == null) return new Code(404, "没有这个相册", null);
            if (album.user_id != user_id) return new Code(403, "没有使用权限", null);
            var picture = _coreDbContext.Album_picture.Skip(page * pageSize).Take(pageSize).Select(b => new { id = b.id, name = b.name, url = b.url });
            if (picture == null) return new Code(404, "这页没有图片", null);
            return new Code(200, "成功", new { item = picture });
        }
    }
    
}

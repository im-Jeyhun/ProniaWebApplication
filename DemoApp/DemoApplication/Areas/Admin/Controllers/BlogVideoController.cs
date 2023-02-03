using DemoApplication.Areas.Admin.ViewModels.BlogVideo;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/blog-video")]
    [Authorize(Roles = "admin")]
    public class BlogVideoController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;

        public BlogVideoController(
            DataContext dataContext,
            IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        [HttpGet("{blogId}/video/list", Name = "admin-blog-video-list")]
        public async Task<IActionResult> ListAsync([FromRoute] int blogId)
        {
            var blog = await _dataContext.Blogs.Include(b => b.BlogVideos)
                .FirstOrDefaultAsync(b => b.Id == blogId);

            if (blog == null) return NotFound();

            var model = new BlogVideViewModel
            {
                BlogName = blog.Title,
                BlogId = blog.Id,

            };

            model.Videos = blog.BlogVideos.Select(pv => new BlogVideViewModel.ListItem
            {
                Id = pv.Id,
                ViedeoUrl = _fileService.GetFileUrl(pv.VideoNameInFileSystem, UploadDirectory.Blog)
            }).ToList();

            return View(model);
        }

        [HttpGet("{blogId}/video/add", Name = "admin-blog-video-add")]
        public async Task<IActionResult> AddAsync()
        {
            return View(new AddViewModel());
        }

        [HttpPost("{blogId}/video/add", Name = "admin-blog-video-add")]
        public async Task<IActionResult> AddAsync([FromRoute] int blogId, AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var blog = await _dataContext.Blogs.Include(b => b.BlogVideos)
                           .FirstOrDefaultAsync(b => b.Id == blogId);

            if (blog is null) return NotFound();

            var blogVideoNameInFileSystem = await _fileService.UploadAsync(model.Video, UploadDirectory.Blog);

            await AddBlogVideo(model.Video.FileName, blogVideoNameInFileSystem);

            await _dataContext.SaveChangesAsync();


            async Task AddBlogVideo(string VideoName, string VideoNameInFileSystem)
            {
                var blogVideo = new BlogVideo
                {
                    VideoName = VideoName,
                    VideoNameInFileSystem = VideoNameInFileSystem,
                    Blog = blog,
                };

              await  _dataContext.BlogVideos.AddAsync(blogVideo);

            }

            return RedirectToRoute("admin-blog-video-list", new { blogId = blog.Id });
        }

        [HttpGet("{blogId}/blogVideo/{blogVideoId}/update", Name = "admin-blog-video-update")]
        public async Task<IActionResult> UpdateAsyn([FromRoute] int blogId, int blogVideoId)
        {
            var blogVideo = await _dataContext.BlogVideos
                .FirstOrDefaultAsync(bi => bi.Id == blogVideoId && bi.BlogId == blogId);

            if (blogVideo is null) return NotFound();

            var model = new UpdateViewModel
            {
                BlogId = blogVideo.BlogId,
                BlogtVideoId = blogVideo.Id,
                VideoUrl = _fileService.GetFileUrl(blogVideo.VideoNameInFileSystem, UploadDirectory.Blog),

            };

            return View(model);
        }

        [HttpPost("{blogId}/blogVideo/{blogVideoId}/update", Name = "admin-blog-video-update")]
        public async Task<IActionResult> UpdateAsyn([FromRoute] int blogId, int blogVideoId, UpdateViewModel model)
        {
            var blogVideo = await _dataContext.BlogVideos
                 .FirstOrDefaultAsync(bi => bi.Id == blogVideoId && bi.BlogId == blogId);

            if (blogVideo is null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.VideoUrl = _fileService.GetFileUrl(blogVideo.VideoNameInFileSystem, UploadDirectory.Blog);
                return View(model);
            }


            await _fileService.DeleteAsync(blogVideo.VideoNameInFileSystem, UploadDirectory.Blog);

            var blogVideoFileNameInSystem = await _fileService.UploadAsync(model.Video, UploadDirectory.Blog);

            await UpdateBlogVideoAsync(model.Video.FileName, blogVideoFileNameInSystem);

            async Task UpdateBlogVideoAsync(string VideoName, string VideoFileNameInSystem)
            {
                blogVideo.VideoName = VideoName;
                blogVideo.VideoNameInFileSystem = VideoFileNameInSystem;

                await _dataContext.SaveChangesAsync();

            };

            return RedirectToRoute("admin-blog-video-list", new { blogId = blogVideo.BlogId });

        }



        [HttpPost("{blogId}/blogVideo/{blogVideoId}/delete", Name = "admin-blog-video-delete")]
        public async Task<IActionResult> DeleteAsync(int blogId, int blogVideoId)
        {
            var blogVideo = await _dataContext.BlogVideos
               .FirstOrDefaultAsync(bi => bi.Id == blogVideoId && bi.BlogId == blogId);

            if (blogVideo is null) return NotFound();

            await _fileService.DeleteAsync(blogVideo.VideoNameInFileSystem, UploadDirectory.Blog);

            _dataContext.BlogVideos.Remove(blogVideo);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-blog-video-list", new { blogId = blogVideo.BlogId });
        }

    }
}

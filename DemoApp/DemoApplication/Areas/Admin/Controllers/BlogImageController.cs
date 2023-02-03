using DemoApplication.Areas.Admin.ViewModels.BlogImage;
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
    [Route("admin/blog-image")]
    [Authorize(Roles = "admin")]
    public class BlogImageController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;

        public BlogImageController(
            DataContext dataContext,
            IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        [HttpGet("{blogId}/image/list", Name = "admin-blog-image-list")]
        public async Task<IActionResult> ListAsync([FromRoute] int blogId)
        {
            var blog = await _dataContext.Blogs.Include(b => b.BlogImages)
                .FirstOrDefaultAsync(b => b.Id == blogId);

            if (blog == null) return NotFound();

            var model = new BlogImagesViewModel
            {
                BlogName = blog.Title,
                BlogId = blog.Id,

            };

            model.Images = blog.BlogImages.Select(pi => new BlogImagesViewModel.ListItem
            {
                Id = pi.Id,
                ImageUrL = _fileService.GetFileUrl(pi.ImageNameInFileSystem, UploadDirectory.Blog)
            }).ToList();

            return View(model);
        }

        [HttpGet("{blogId}/image/add", Name = "admin-blog-image-add")]
        public async Task<IActionResult> AddAsync()
        {
            return View(new AddViewModel());
        }

        [HttpPost("{blogId}/image/add", Name = "admin-blog-image-add")]
        public async Task<IActionResult> AddAsync([FromRoute] int blogId, AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var blog = await _dataContext.Blogs.Include(b => b.BlogImages)
                           .FirstOrDefaultAsync(b => b.Id == blogId);
            if (blog is null) return NotFound();

            var blogImageNameInFileSystem = await _fileService.UploadAsync(model.Image, UploadDirectory.Blog);

            await AddBlogImage(model.Image.FileName, blogImageNameInFileSystem);

            await _dataContext.SaveChangesAsync();


            async Task AddBlogImage(string ImageName, string ImageNameInFileSystem)
            {
                var blogImage = new BlogImage
                {
                    ImageName = ImageName,
                    ImageNameInFileSystem = ImageNameInFileSystem,
                    Blog = blog,
                };

               await _dataContext.BlogImages.AddAsync(blogImage);

            }

            return RedirectToRoute("admin-blog-image-list", new { blogId = blog.Id });
        }

        [HttpGet("{blogId}/image/{blogImageId}/update", Name = "admin-blog-image-update")]
        public async Task<IActionResult> UpdateAsyn([FromRoute] int blogId, int blogImageId)
        {
            var blogImage = await _dataContext.BlogImages
                .FirstOrDefaultAsync(bi => bi.Id == blogImageId && bi.BlogId == blogId);

            if (blogImage is null) return NotFound();

            var model = new UpdateViewModel
            {
                BlogId = blogImage.BlogId,
                BlogImageeId = blogImage.Id,
                ImageUrl = _fileService.GetFileUrl(blogImage.ImageName, UploadDirectory.Blog),

            };

            return View(model);
        }

        [HttpPost("{blogId}/image/{blogImageId}/update", Name = "admin-blog-image-update")]
        public async Task<IActionResult> UpdateAsyn([FromRoute] int blogId, int blogImageId, UpdateViewModel model)
        {
            var blogImage = await _dataContext.BlogImages
                 .FirstOrDefaultAsync(bi => bi.Id == blogImageId && bi.BlogId == blogId);

            if (blogImage is null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.ImageUrl = _fileService.GetFileUrl(blogImage.ImageName, UploadDirectory.Blog);
                return View(model);
            }
          
           
            await _fileService.DeleteAsync(blogImage.ImageNameInFileSystem, UploadDirectory.Blog);
            var blogImageFileNameInSystem = await _fileService.UploadAsync(model.Image, UploadDirectory.Plant);
            await UpdateBlogImageAsync(model.Image.FileName, blogImageFileNameInSystem);

            async Task UpdateBlogImageAsync(string ImageName, string ImageFileNameInSystem)
            {
                blogImage.ImageName = ImageName;
                blogImage.ImageNameInFileSystem = ImageFileNameInSystem;

                await _dataContext.SaveChangesAsync();

            };

            return RedirectToRoute("admin-blog-image-list", new { blogId = blogImage.BlogId });

        }



        [HttpPost("{blogId}/image/{blogImageId}/delete", Name = "admin-blog-image-delete")]
        public async Task<IActionResult> DeleteAsync(int blogId, int blogImageId)
        {
            var blogImage = await _dataContext.BlogImages
               .FirstOrDefaultAsync(bi => bi.Id == blogImageId && bi.BlogId == blogId);

            if (blogImage is null) return NotFound();

            await _fileService.DeleteAsync(blogImage.ImageNameInFileSystem, UploadDirectory.Blog);

            _dataContext.BlogImages.Remove(blogImage);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-blog-image-list", new { blogId = blogImage.BlogId });
        }

    }
}

using DemoApplication.Areas.Client.ViewComponents;
using DemoApplication.Areas.Client.ViewModels.Blog;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        public BlogController(IFileService folderService, DataContext dataContext = null)
        {
            _fileService = folderService;
            _dataContext = dataContext;
        }
        [HttpGet("details/{id}",Name ="client-blog-details")]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            var blog = await _dataContext.Blogs
                .Include(b => b.BlogTags)
                .Include(b => b.User)
                .Include(b => b.BlogVideos)
                .Include(b => b.BlogImages)
                .FirstOrDefaultAsync(b => b.Id == id);

            if(blog == null) return NotFound();

            var model = new BlogItemViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                CreatedAt = blog.CreatedAt,
                User = $"{blog.User.FirstName} {blog.User.LastName}",
                ImageUrl = blog!.BlogImages!.Take(1).FirstOrDefault() != null ?
                    _fileService.GetFileUrl(blog!.BlogImages!.Take(1)
                        .FirstOrDefault()!.ImageNameInFileSystem, UploadDirectory.Blog) : default,

                VideoUrl = blog!.BlogVideos!.Take(1).FirstOrDefault() != null ?
                    _fileService.GetFileUrl(blog!.BlogVideos!.Take(1)
                        .FirstOrDefault()!.VideoNameInFileSystem, UploadDirectory.Blog) : blog.VideoUrl,
                Tags = _dataContext.BlogTags
                .Include(bt => bt.BTag)
                .Where(bt => bt.BlogId == blog.Id)
                .Select(pt => new TagViewModel(pt.BTagId, pt.BTag.Name)).ToList(),
            };

            return View(model);
        }

        [HttpGet("list", Name = "client-blog-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = new BlogListItemViewModel
            {
                Categories = _dataContext.BlogCategories
                     .Select(bc => new CategoryViewModel(bc.Id, bc.Title)).ToList(),
                Tags = _dataContext.BlogTags
                     .Select(bt => new TagViewModel(bt.BTagId, bt.BTag.Name)).ToList()

            };

            return View(model);
        }

        [HttpGet("blog-filter", Name = "client-blog-filter")]
        public async Task<IActionResult> FilterAsync(BlogListItemViewModel model, string Search)
        {
            return ViewComponent(nameof(BlogItem), new { CategoryId = model.CategoryId, TagId = model.TagId, Search = Search});
        }


    }
}

using DemoApplication.Areas.Admin.ViewModels.Blog;
using DemoApplication.Areas.Admin.ViewModels.Blog.Add;
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
    [Route("admin/blog")]
    [Authorize(Roles = "admin")]

    public class BlogController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly ILogger<BlogController> _logger;

        public BlogController(DataContext dataContext, IFileService fileService, ILogger<BlogController> logger, IUserService userService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("list",Name ="admin-blog-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.Blogs
                .Include(p => p.BlogCategory)
                .Include(p => p.User)
                .Select(p => new ListItemViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = DateTime.Now,
                    UserFullName = $"{p.User.FirstName} {p.User.FirstName}",
                    Category = p.BlogCategory.Title
                    

                }).ToListAsync();

            return View(model);
        }

        [HttpGet("add-blog", Name = "admin-blog-add")]
        public async Task<IActionResult> AddAsync()
        {
            var model = new AddViewModel
            {
                BlogTags = _dataContext.BTags
                .Select(t => new TagItemViewModel(t.Id, t.Name)).ToList(),
                BlogCategiries = _dataContext.BlogCategories
                .Select(bc => new BlogCategorItemViewModel(bc.Id , bc.Title)).ToList()
            };

            return View(model);
        }

        [HttpPost("add-blog", Name = "admin-blog-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid) return GetView();

            if (!_dataContext.BlogCategories.Any(c => c.Id == model.BlogCategoryId))
            {
                ModelState.AddModelError(string.Empty, "Blog category is not found");
                return GetView();
            }


            foreach (var bTagId in model.BlogTagIds)
            {
                if (!_dataContext.BTags.Any(t => t.Id == bTagId))
                {
                    ModelState.AddModelError(string.Empty, "something went wrong");
                    _logger.LogWarning($"BlogTag with id({bTagId}) not found in db ");
                    return GetView();
                }

            }


            var blog = CreateBlog();

            await CreateBlogTags();


            await CreateBlogImage();

            if(model.BlogVideos is not null)
            {

            await CreateBlogVideo();
            }


            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-blog-list");
            Blog CreateBlog()
            {
                var blog = new Blog
                {
                    Title = model.Title,
                    Content = model.Content,
                    UserId = _userService.CurrentUser.Id,
                    BlogCategoryId = model.BlogCategoryId,
                    VideoUrl = model.VideoUrl != null ? model.VideoUrl : default
                    
                };

                _dataContext.Blogs.Add(blog);

                return blog;
            }

            async Task CreateBlogTags()
            {
                foreach (var tagId in model.BlogTagIds)
                {
                    var blogTag = new BlogBTag
                    {
                        Blog = blog,
                        BTagId = tagId
                    };

                    await _dataContext.BlogTags.AddAsync(blogTag);

                }
            }

          

            async Task CreateBlogImage()
            {
                foreach (IFormFile blogImage in model!.BlogImages)
                {
                    var imageNameInSystem = await _fileService.UploadAsync(blogImage, UploadDirectory.Blog);

                    var image = new BlogImage
                    {
                        
                        ImageName = blogImage.FileName,
                        ImageNameInFileSystem = imageNameInSystem,
                        Blog = blog
                    };

                   await _dataContext.BlogImages.AddAsync(image);
                }
            }

            async Task CreateBlogVideo()
            {
                foreach (IFormFile blogVideo in model!.BlogVideos)
                {
                    var videNameInSystem = await _fileService.UploadAsync(blogVideo, UploadDirectory.Blog);

                    var video = new BlogVideo
                    {

                        VideoName = blogVideo.FileName,
                        VideoNameInFileSystem = videNameInSystem,
                        Blog = blog
                    };

                    await _dataContext.BlogVideos.AddAsync(video);
                }
            }

            IActionResult GetView()
            {
                model.BlogTags = _dataContext.BTags
                 .Select(t => new TagItemViewModel(t.Id, t.Name)).ToList();
                model.BlogCategiries = _dataContext.BlogCategories
                .Select(bc => new BlogCategorItemViewModel(bc.Id, bc.Title)).ToList();
                return View(model);
            }
        }
        [HttpGet("uptade-blog/{id}", Name = "admin-blog-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id)
        {
            var blog = await _dataContext.Blogs
                .Include(b => b.BlogImages)
                .Include(b => b.BlogVideos)
                .Include(b => b.BlogTags)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog is null) return NotFound();

            var model = new UpdateViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                VideoUrl = blog.VideoUrl != null ? blog.VideoUrl : default,
                BlogCategoryId = blog.BlogCategoryId,
                BlogCategiries = _dataContext.BlogCategories
                .Select(c => new BlogCategorItemViewModel(c.Id, c.Title)).ToList(),
                TagIds = blog!.BlogTags.Select(bt => bt.BTagId).ToList(),
                Tags = _dataContext.BTags
                .Select(t => new TagItemViewModel(t.Id, t.Name)).ToList(),
            };

            return View(model);
        }
        [HttpPost("uptade-blog/{id}", Name = "admin-blog-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, UpdateViewModel model)
        {
            var blog = await _dataContext.Blogs
                .Include(b => b.BlogImages)
                .Include(b => b.BlogVideos)
                .Include(b => b.BlogTags)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (!ModelState.IsValid) return GetView();

            if (blog is null) return NotFound();

            if (!_dataContext.BlogCategories.Any(c => c.Id == model.BlogCategoryId))
            {
                ModelState.AddModelError(string.Empty, "Blog category is not found");
                return GetView();
            }


            foreach (var bTagId in model.TagIds)
            {
                if (!_dataContext.BTags.Any(bt => bt.Id == bTagId))
                {
                    ModelState.AddModelError(string.Empty, "something went wrong");
                    _logger.LogWarning($"BlogTag with id({bTagId}) not found in db ");
                    return GetView();
                }

            }

            await UpdateBlog();

            return RedirectToRoute("admin-blog-list");


            IActionResult GetView()
            {
                model.BlogCategiries = _dataContext.BlogCategories
                 .Select(c => new BlogCategorItemViewModel(c.Id, c.Title)).ToList();
                model.Tags = _dataContext.BTags
                .Select(t => new TagItemViewModel(t.Id, t.Name)).ToList();

                return View(model);

            }

            async Task UpdateBlog()
            {
                blog.Title = model.Title;
                blog.Content = model.Content;
                blog.BlogCategoryId = model.BlogCategoryId;
                blog.VideoUrl = blog.VideoUrl != null ? blog.VideoUrl : default;

                UpdateBlogTag();

                await _dataContext.SaveChangesAsync();

            }

            void UpdateBlogTag()
            {
                //databazada olan blogun taglarini getirmek
                var tagsInDb = blog!.BlogTags.Select(p => p.BTagId).ToList();

                //databazada bloga aid olan taglari ayirmaq modeldeki taglara
                var tagsToRemove = tagsInDb.Except(model.TagIds).ToList();

                //modelden gelen yeni taglari databasada bloga aid olan taglardan ayirmaq
                var tagsToAdd = model.TagIds.Except(tagsInDb).ToList();

                blog.BlogTags.RemoveAll(pt => tagsToRemove.Contains(pt.BTagId));

                foreach (var tagId in tagsToAdd)
                {
                    var blogTag = new BlogBTag
                    {
                        BTagId = tagId,
                        BlogId = blog.Id
                   
                    };

                    _dataContext.BlogTags.Add(blogTag);
                }
            }

           
        }

        [HttpPost("delete-blog/{id}", Name = "admin-blog-delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var blog = await _dataContext.Blogs
                .Include(b => b.BlogImages)
                .Include(b => b.BlogVideos)
                .Include(b => b.BlogTags)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog is null) return NotFound();

            _dataContext.Blogs.Remove(blog);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-add-blog-list");

        }
    }
}

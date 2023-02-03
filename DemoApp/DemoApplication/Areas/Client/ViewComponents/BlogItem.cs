using DemoApplication.Areas.Client.ViewModels.Product;
using DemoApplication.Areas.Client.ViewModels.PlantItem;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoApplication.Contracts.Blog;
using DemoApplication.Areas.Client.ViewModels.Blog;

namespace DemoApplication.Areas.Client.ViewComponents
{
    [ViewComponent(Name = "BlogItem")]

    public class BlogItem : ViewComponent
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;

        public BlogItem(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string showDirectory, 
            int? CategoryId = null, int? TagId = null , string Search = null)
        
        {
            
            var query = _dataContext
                .Blogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
                query = query.Where(b => b.Title.Contains(Search));

            if (CategoryId != null)
            {
                query = query.Where(p => p.BlogCategoryId == CategoryId);
            }
           
            if (TagId != null)
            {
                query = query.Where(b => b.BlogTags.Any(b => b.BTagId == TagId));
            }
           

            if (showDirectory == Contracts.Blog.Directory.New_Blog)
            {
                query = query.Take(3).OrderByDescending(p => p.CreatedAt);
            }

            var model = query
                    .Include(b => b.BlogImages)
                    .Include(b => b.BlogVideos)
                    .Select(b => new BlogItemViewModel
                    {
                        Id = b.Id,
                        User = $"{b.User.FirstName} {b.User.LastName}",
                        Title = b.Title,
                        Content = b.Content,
                        CreatedAt = b.CreatedAt,
                        ImageUrl = b!.BlogImages.Take(1).FirstOrDefault() != null ?
                    _fileService.GetFileUrl(b!.BlogImages.Take(1)
                        .FirstOrDefault().ImageNameInFileSystem, UploadDirectory.Blog) : default,
                        VideoUrl = b.BlogVideos.Take(1).FirstOrDefault() != null ?
                    _fileService.GetFileUrl(b.BlogVideos.Take(1)
                        .FirstOrDefault().VideoNameInFileSystem, UploadDirectory.Blog) : b.VideoUrl

                    }).ToList();





            return View(model);
        }
    }
}

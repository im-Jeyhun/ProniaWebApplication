using DemoApplication.Areas.Admin.ViewModels.BlogCategory;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/blog-category")]
    [Authorize(Roles = "admin")]

    public class BlogCategoryController : Controller
    {
        private readonly DataContext _dataContext;

        public BlogCategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list", Name = "admin-blog-category-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.BlogCategories.Select(c => new ListItemViewModel(c.Id , c.Title)).ToListAsync();

            return View(model);
        }
        [HttpGet("add-blog-category", Name = "admin-blog-category-add")]
        public async Task<IActionResult> AddAsync()
        {
            return View();
        }
        [HttpPost("add-blog-category", Name = "admin-blog-category-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            var blogCategory = new BlogCategory
            {
                Title = model.Title
            };

            _dataContext.BlogCategories.Add(blogCategory);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-blog-category-list");

        }

        [HttpGet("update-blog-category/{id}", Name = "admin-blog-category-update")]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var blogCategory = await _dataContext.BlogCategories.FirstOrDefaultAsync(c => c.Id == id);

            if (blogCategory is null) return NotFound();

            var model = new AddViewModel
            {
                Id = blogCategory.Id,
                Title = blogCategory.Title
            };

            return View(model);
        }

        [HttpPost("update-blog-category/{id}", Name = "admin-blog-category-update")]
        public async Task<IActionResult> UpdateAsync(int id , AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var blogCategory = await _dataContext.BlogCategories.FirstOrDefaultAsync(c => c.Id == id);

            if (blogCategory is null) return NotFound();

            blogCategory.Title = model.Title;

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-blog-category-list");
        }
        [HttpPost("delete-blog-category/{id}", Name = "admin-blog-category-delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var blogCategory = await _dataContext.BlogCategories.FirstOrDefaultAsync(c => c.Id == id);

            if (blogCategory is null) return NotFound();

            _dataContext.BlogCategories.Remove(blogCategory);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-blog-category-list");

        }
    }
}

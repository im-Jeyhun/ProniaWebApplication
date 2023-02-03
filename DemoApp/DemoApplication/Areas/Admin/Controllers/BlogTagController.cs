using DemoApplication.Areas.Admin.ViewModels.BlogTag;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/blog-tag")]
    [Authorize(Roles = "admin")]

    public class BlogTagController : Controller
    {
        private readonly DataContext _dataContext;

        public BlogTagController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list", Name = "admin-blog-tag-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.BTags.Select(bt => new ListItemViewModel(bt.Id , bt.Name)).ToListAsync();

            return View(model);
        }
        [HttpGet("add-blog-tag", Name = "admin-blog-tag-add")]
        public async Task<IActionResult> AddAsync()
        {
            return View();
        }
        [HttpPost("add-blog-tag", Name = "admin-blog-tag-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var blogTag = new BTag
            {
                Name = model.Name
            };

            _dataContext.BTags.Add(blogTag);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-blog-tag-list");

        }

        [HttpGet("update-blog-tag/{id}", Name = "admin-blog-tag-update")]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var blogTag = await _dataContext.BTags.FirstOrDefaultAsync(t => t.Id == id);

            if (blogTag is null) return NotFound();

            var model = new AddViewModel
            {
                Id = blogTag.Id,
                Name = blogTag.Name
            };

            return View(model);
        }

        [HttpPost("update-blog-tag/{id}", Name = "admin-blog-tag-update")]
        public async Task<IActionResult> UpdateAsync(int id , AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var blogTag = await _dataContext.BTags.FirstOrDefaultAsync(t => t.Id == id);

            if (blogTag is null) return NotFound();

            blogTag.Name = model.Name;

           await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-blog-tag-list");
        }
        [HttpPost("delete-blog-tag/{id}", Name = "admin-blog-tag-delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var blogTag = await _dataContext.BTags.FirstOrDefaultAsync(t => t.Id == id);

            if (blogTag is null) return NotFound();

            _dataContext.BTags.Remove(blogTag);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-blog-tag-list");

        }
    }
}

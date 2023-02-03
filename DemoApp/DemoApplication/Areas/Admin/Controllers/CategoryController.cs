using DemoApplication.Areas.Admin.ViewModels.Category;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/category")]
    [Authorize(Roles = "admin")]

    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;

        public CategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list", Name = "admin-category-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.Categories.Select(c => new ListItemViewModel(c.Id , c.Title)).ToListAsync();

            return View(model);
        }
        [HttpGet("add-category", Name = "admin-category-add")]
        public async Task<IActionResult> AddAsync()
        {
            return View();
        }
        [HttpPost("add-category", Name = "admin-category-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            var category = new Category
            {
                Title = model.Title
            };

            _dataContext.Categories.Add(category);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-category-list");

        }

        [HttpGet("update-category/{id}", Name = "admin-category-update")]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var category = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null) return NotFound();

            var model = new AddViewModel
            {
                Id = category.Id,
                Title = category.Title
            };

            return View(model);
        }

        [HttpPost("update-category/{id}", Name = "admin-category-update")]
        public async Task<IActionResult> UpdateAsync(int id , AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var category = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null) return NotFound();

            category.Title = model.Title;

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-category-list");
        }
        [HttpPost("delete-category/{id}", Name = "admin-category-delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var category = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null) return NotFound();

            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-category-list");

        }
    }
}

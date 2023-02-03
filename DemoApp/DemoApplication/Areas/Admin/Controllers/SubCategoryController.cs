using DemoApplication.Areas.Admin.ViewModels.SubCategory;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/subcategory")]
    public class SubCategoryController : Controller
    {
        private readonly DataContext _dataContext;

        public SubCategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list", Name = "admin-subcategory-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.SubCategories
                .Select(c => new ListItemViewModel(c.Id, c.Title , $"{c.Category.Title}"))
                .ToListAsync();

            return View(model);
        }

        [HttpGet("add-subcategory", Name = "admin-subcategory-add")]
        public async Task<IActionResult> AddAsync()
        {
            var model = new AddViewModel
            {
                Categories = await _dataContext.Categories
                 .Select(c => new AddViewModel.CategoryViewModel(c.Id, c.Title))
                 .ToListAsync()
            };

            return View(model);
        }

        [HttpPost("add-subcategory", Name = "admin-subcategory-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _dataContext.Categories
                 .Select(c => new AddViewModel.CategoryViewModel(c.Id, c.Title))
                 .ToListAsync();

                return View(model);
            }

            var subCategory = new SubCategory
            {
                Title = model.Title,
                CategoryId = model.CategoryId
            };


            _dataContext.SubCategories.Add(subCategory);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-subcategory-list");

        }

        [HttpGet("update-subcategory/{id}", Name = "admin-subcategory-update")]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var subcategory = await _dataContext.SubCategories.FirstOrDefaultAsync(sc => sc.Id == id);

            if (subcategory is null) return NotFound();


            var model = new UpdateVeiwModel
            {
                Title = subcategory.Title,
                Categories = await _dataContext.Categories
                .Select(c => new UpdateVeiwModel.CategoryViewModel(c.Id , c.Title)).ToListAsync()
            };

            return View(model);
        }


        [HttpPost("update-subcategory/{id}", Name = "admin-subcategory-update")]
        public async Task<IActionResult> UpdateAsync(int id, UpdateVeiwModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _dataContext.Categories
                 .Select(c => new UpdateVeiwModel.CategoryViewModel(c.Id, c.Title))
                 .ToListAsync();

                return View(model);
            }

            var subcategory = await _dataContext.SubCategories.FirstOrDefaultAsync(sc => sc.Id == id);

            if (subcategory is null) return NotFound();

            subcategory.Title = model.Title;
            subcategory.CategoryId = model.CategoryId;

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-subcategory-list");
        }

        [HttpPost("delete-subcategory/{id}", Name = "admin-subcategory-delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var subcategory = await _dataContext.SubCategories.FirstOrDefaultAsync(sc => sc.Id == id);

            if (subcategory is null) return NotFound();

            _dataContext.SubCategories.Remove(subcategory);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-subcategory-list");

        }

    } 
}

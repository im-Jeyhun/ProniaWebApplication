using DemoApplication.Areas.Admin.ViewModels.Color;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/color")]
    [Authorize(Roles = "admin")]

    public class ColorController : Controller
    {
        private readonly DataContext _dataContext;

        public ColorController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list", Name = "admin-color-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.Colors.Select(t => new ListItemViewModel(t.Id, t.Name)).ToListAsync();

            return View(model);
        }
        [HttpGet("add-color", Name = "admin-color-add")]
        public async Task<IActionResult> AddAsync()
        {
            return View();
        }
        [HttpPost("add-color", Name = "admin-color-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var color = new Color
            {
                Name = model.Name
            };

            _dataContext.Colors.Add(color);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-color-list");

        }

        [HttpGet("update-color/{id}", Name = "admin-color-update")]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var color = await _dataContext.Colors.FirstOrDefaultAsync(t => t.Id == id);

            if (color is null) return NotFound();

            var model = new UpdateVeiwModel
            {
                Id = color.Id,
                Name = color.Name
            };

            return View(model);
        }

        [HttpPost("update-color/{id}", Name = "admin-color-update")]
        public async Task<IActionResult> UpdateAsync(int id , AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var color = await _dataContext.Colors.FirstOrDefaultAsync(t => t.Id == id);

            if (color is null) return NotFound();

            color.Name = model.Name;

           await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-color-list");
        }
        [HttpPost("delete-color/{id}", Name = "admin-color-delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var color = await _dataContext.Colors.FirstOrDefaultAsync(t => t.Id == id);

            if (color is null) return NotFound();

            _dataContext.Colors.Remove(color);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-color-list");

        }
    }
}

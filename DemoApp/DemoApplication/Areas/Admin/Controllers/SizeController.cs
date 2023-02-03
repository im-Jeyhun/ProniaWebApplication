using DemoApplication.Areas.Admin.ViewModels.Size;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/size")]
    public class SizeController : Controller
    {
        private readonly DataContext _dataContext;

        public SizeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list", Name = "admin-size-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.Sizes.Select(t => new ListItemViewModel(t.Id, t.Name)).ToListAsync();

            return View(model);
        }
        [HttpGet("add-size", Name = "admin-size-add")]
        public async Task<IActionResult> AddAsync()
        {
            return View();
        }
        [HttpPost("add-size", Name = "admin-size-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var color = new Size
            {
                Name = model.Name
            };

            _dataContext.Sizes.Add(color);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-size-list");

        }

        [HttpGet("update-size/{id}", Name = "admin-size-update")]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var size = await _dataContext.Sizes.FirstOrDefaultAsync(t => t.Id == id);

            if (size is null) return NotFound();

            var model = new UpdateVeiwModel
            {
                Id = size.Id,
                Name = size.Name
            };

            return View(model);
        }

        [HttpPost("update-size/{id}", Name = "admin-size-update")]
        public async Task<IActionResult> UpdateAsync(int id , AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var size = await _dataContext.Sizes.FirstOrDefaultAsync(t => t.Id == id);

            if (size is null) return NotFound();

            size.Name = model.Name;

           await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-size-list");
        }
        [HttpPost("delete-size/{id}", Name = "admin-size-delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var size = await _dataContext.Sizes.FirstOrDefaultAsync(t => t.Id == id);

            if (size is null) return NotFound();

            _dataContext.Sizes.Remove(size);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-size-list");

        }
    }
}

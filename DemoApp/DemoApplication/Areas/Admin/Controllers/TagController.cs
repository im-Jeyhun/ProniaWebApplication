using DemoApplication.Areas.Admin.ViewModels.Tag;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/tag")]
    [Authorize(Roles = "admin")]

    public class TagController : Controller
    {
        private readonly DataContext _dataContext;

        public TagController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list", Name = "admin-tag-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.Tags.Select(t => new ListItemViewModel(t.Id, t.Name)).ToListAsync();

            return View(model);
        }
        [HttpGet("add-tag", Name = "admin-tag-add")]
        public async Task<IActionResult> AddAsync()
        {

            return View();
        }
        [HttpPost("add-tag", Name = "admin-tag-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var tag = new Tag
            {
                Name = model.Name
            };

            _dataContext.Tags.Add(tag);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-tag-list");

        }

        [HttpGet("update-tag/{id}", Name = "admin-tag-update")]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var tag = await _dataContext.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (tag is null) return NotFound();

            var model = new AddViewModel
            {
                Id = tag.Id,
                Name = tag.Name
            };

            return View(model);
        }

        [HttpPost("update-tag/{id}", Name = "admin-tag-update")]
        public async Task<IActionResult> UpdateAsync(int id , AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var tag = await _dataContext.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (tag is null) return NotFound();

            tag.Name = model.Name;

           await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-tag-list");
        }
        [HttpPost("delete-tag/{id}", Name = "admin-tag-delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var tag = await _dataContext.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (tag is null) return NotFound();

            _dataContext.Tags.Remove(tag);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-tag-list");

        }
    }
}

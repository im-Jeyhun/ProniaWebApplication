using DemoApplication.Areas.Admin.ViewModels.About;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/about")]
    public class AboutController : Controller
    {
        private readonly DataContext _dataContext;

        public AboutController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list", Name = "admin-about-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.Abouts.Select(a => new ListItemViewModel(a.Id, a.Content)).ToListAsync();

            return View(model);
        }
        [HttpGet("add-about", Name = "admin-about-add")]
        public async Task<IActionResult> AddAsync()
        {
            return View();
        }
        [HttpPost("add-about", Name = "admin-about-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var about = new About
            {
                Content = model.Content
            };

            _dataContext.Abouts.Add(about);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-about-list");

        }

        [HttpGet("update-about/{id}", Name = "admin-about-update")]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var about = await _dataContext.Abouts.FirstOrDefaultAsync(a => a.Id == id);

            if (about is null) return NotFound();

            var model = new UpdateViewModel
            {
                Id = about.Id,
                Content = about.Content
            };

            return View(model);
        }

        [HttpPost("update-about/{id}", Name = "admin-about-update")]
        public async Task<IActionResult> UpdateAsync(int id , AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var about = await _dataContext.Abouts.FirstOrDefaultAsync(a => a.Id == id);

            if (about is null) return NotFound();

            about.Content = model.Content;

           await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-about-list");
        }
        [HttpPost("delete-about/{id}", Name = "admin-about-delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var about = await _dataContext.Abouts.FirstOrDefaultAsync(a => a.Id == id);

            if (about is null) return NotFound();

            _dataContext.Abouts.Remove(about);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-tag-list");

        }
    }
}

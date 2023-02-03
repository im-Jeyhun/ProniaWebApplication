using DemoApplication.Areas.Admin.ViewModels.Navbar;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/navbar")]
    [Authorize(Roles = "admin")]

    public class NavbarController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IActionDescriptorCollectionProvider _provider;

        public NavbarController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list",Name ="admin-navbar-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.Navbars.Select(n =>
            new ListItemViewModel(n.Id, n.Name, n.Url, n.RowNumber, n.IsHeader, n.IsFooter)).ToListAsync();

            return View(model);
        }

        [HttpGet("add-navbar", Name = "admin-add-navbar")]
        public async Task<IActionResult> AddAsync()
        {
            return View();
        }

        [HttpPost("add-navbar", Name = "admin-add-navbar")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var navbar = new Navbar
            {
                Name = model.Name,
                Url = model.Url,
                IsFooter = model.IsFooter,
                IsHeader = model.IsHeader,
                RowNumber = model.RowNumber,
            };

            await _dataContext.Navbars.AddAsync(navbar);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-navbar-list");
        }

        [HttpGet("update-navbar/{id}", Name = "admin-update-navbar")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id)
        {
            var navbar = await _dataContext.Navbars.FirstOrDefaultAsync(n => n.Id == id);

            if(navbar is null) return NotFound();

            var model = new UpdateViewModel
            {
                Id = navbar.Id,
                Name = navbar.Name,
                Url = navbar.Url,
                RowNumber = navbar.RowNumber,
                IsFooter = navbar.IsFooter,
                IsHeader = navbar.IsHeader
            };

            return View(model);
        }

        [HttpPost("update-navbar/{id}", Name = "admin-update-navbar")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id , UpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var navbar = await _dataContext.Navbars.FirstOrDefaultAsync(n => n.Id == id);

            if (navbar is null) return NotFound();

            await UpdateNavbar();

            return RedirectToRoute("admin-navbar-list");


            async Task UpdateNavbar()
            {
                navbar.Name = model.Name;
                navbar.Url = model.Url;
                navbar.RowNumber = model.RowNumber;
                navbar.IsHeader = model.IsHeader;
                navbar.IsFooter = model.IsFooter;

               await _dataContext.SaveChangesAsync();
            }
        }

        [HttpPost("delete-navbar/{id}", Name = "admin-delete-navbar")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var navbar = await _dataContext.Navbars.FirstOrDefaultAsync(n => n.Id == id);

            if (navbar is null) return NotFound();


            _dataContext.Navbars.Remove(navbar);


            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-navbar-list");

        }
    }
}

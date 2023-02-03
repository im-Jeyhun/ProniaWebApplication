using DemoApplication.Areas.Admin.ViewModels.SubNavbar;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/subnavbar")]
    [Authorize(Roles ="admin")]
    public class SubNavbarController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IActionDescriptorCollectionProvider _provider;
        public SubNavbarController(DataContext dataContext, IActionDescriptorCollectionProvider provider)
        {
            _dataContext = dataContext;
            _provider = provider;
        }

        #region List
        [HttpGet("list", Name = "admin-subnavbar-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.SubNavbars
                .Select(sn => new ListItemViewModel(sn.Id, sn.Name, sn.Url, $"{sn.Navbar.Name}", sn.RowNumber))
                .ToListAsync();

            return View(model);
        }
        #endregion

        #region Add
        [HttpGet("add-subnavbar", Name = "admin-subnavbar-add")]
        public async Task<IActionResult> AddAsync()
        {
            var model = new AddViewModel
            {
                Navbars = await _dataContext.Navbars
                    .Select(n => new AddViewModel.NavbarViewModel(n.Id, n.Name))
                    .ToListAsync(),
                Urls = _provider.ActionDescriptors.Items.Where(u => u.RouteValues["Area"] != "admin")
                .Select(u => new AddViewModel.UrlViewModel(u!.AttributeRouteInfo.Template)).ToList()

            };

            return View(model);
        }

        [HttpPost("add-subnavbar", Name = "admin-subnavbar-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Navbars = await _dataContext.Navbars
                   .Select(a => new AddViewModel.NavbarViewModel(a.Id, a.Name))
                   .ToListAsync();

                model.Urls = _provider.ActionDescriptors.Items.Where(u => u.RouteValues["Area"] != "admin")
                .Select(u => new AddViewModel.UrlViewModel(u!.AttributeRouteInfo.Template)).ToList();

                return View(model);
            }

            var subNavbar = new SubNavbar()
            {
                Name = model.Name,
                Url = model.Url,
                RowNumber = model.RowNumber,
                NavbarId = model.NavbarId,
            };

            await _dataContext.SubNavbars.AddAsync(subNavbar);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-subnavbar-list");
        }
        #endregion

        #region Update
        [HttpGet("update-subnavbar/{id}", Name = "admin-subnavbar-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id)
        {
            var subnavbar = await _dataContext.SubNavbars.FirstOrDefaultAsync(b => b.Id == id);
            if (subnavbar is null)
            {
                return NotFound();
            }


            var model = new UpdateViewModel
            {

                Name = subnavbar.Name,
                Url = subnavbar.Url,
                RowNumber = subnavbar.RowNumber,
                Navbars = await _dataContext.Navbars
                    .Select(n => new UpdateViewModel.NavbarViewModel(n.Id, n.Name))
                    .ToListAsync(),
                NavbarId = subnavbar.NavbarId,
                Urls = _provider.ActionDescriptors.Items.Where(u => u.RouteValues["Area"] != "admin")
                .Select(u => new UpdateViewModel.UrlViewModel(u!.AttributeRouteInfo.Template)).ToList()

            };

            return View(model);
        }

        [HttpPost("update-subnavbar/{id}", Name = "admin-subnavbar-update")]
        public async Task<IActionResult> UpdateAsync(int id, UpdateViewModel model)
        {
            var subnavbar = await _dataContext.SubNavbars.FirstOrDefaultAsync(b => b.Id == id);

            if (subnavbar is null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.Navbars = await _dataContext.Navbars.Where(n => n.Id == subnavbar.NavbarId)
                    .Select(n => new UpdateViewModel.NavbarViewModel(n.Id, n.Name)).ToListAsync();
                model.Urls = _provider.ActionDescriptors.Items.Where(u => u.RouteValues["Area"] != "admin")
                .Select(u => new UpdateViewModel.UrlViewModel(u!.AttributeRouteInfo.Template)).ToList();

                return View(model);
            }

            subnavbar.Name = model.Name;
            subnavbar.Url = model.Url;
            subnavbar.RowNumber = model.RowNumber;
            subnavbar.NavbarId = model.NavbarId;

            await _dataContext.SaveChangesAsync();
            return RedirectToRoute("admin-subnavbar-list");
        }
        #endregion

        #region Delete
        [HttpPost("delete/{id}", Name = "admin-subnavbar-delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var subNavbar = await _dataContext.SubNavbars.FirstOrDefaultAsync(b => b.Id == id);
            if (subNavbar is null) return NotFound();


            _dataContext.SubNavbars.Remove(subNavbar);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-subnavbar-list");
        }
        #endregion


    }

}

using DemoApplication.Areas.Client.ViewModels.Navbar;
using DemoApplication.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Client.ViewComponents
{
    [ViewComponent(Name = "NavbarHeader")]
    public class NavbarHeader : ViewComponent
    {
        private readonly DataContext _dataContext;

        public NavbarHeader(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var model = await _dataContext.Navbars.Include(n => n.SubNavbars.OrderBy(s => s.RowNumber)).
                Where(n => n.IsHeader).OrderBy(n => n.RowNumber).
                Select(n => new NavbarListItemViewModel(n.Name, n.Url, n.SubNavbars.Select(s => new NavbarListItemViewModel.SubnavbarViewModel(s.Name, s.Url)).ToList()
                )).ToListAsync();

            return View(model);
        }
    }
}

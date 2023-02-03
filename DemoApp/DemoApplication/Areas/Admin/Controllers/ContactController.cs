using DemoApplication.Areas.Admin.ViewModels.Contact;
using DemoApplication.Database;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/contact")]
    public class ContactController : Controller
    {
        private readonly DataContext _dataContext;

        public ContactController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list",Name ="admin-contact-list")]
        public async Task<IActionResult> List()
        {
            var model = _dataContext.Contacts
                .Select(c => new ListItemViewModel(c.Id, c.Name, c.LastName, c.Phone, c.Email,c.Message)).ToList();

            return View(model);
        }
    }
}

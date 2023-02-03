using DemoApplication.Areas.Client.ViewModels.About;
using DemoApplication.Database;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("about")]
    public class AboutController : Controller
    {
        private readonly DataContext _dataContext;

        public AboutController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("about", Name = "client-aboutl-page")]
        public async Task<IActionResult> Index()
        {
            var about = _dataContext.Abouts.First();

            if (about == null) return NotFound();

           
            return View(about);
        }
    }
}

using DemoApplication.Areas.Client.ViewModels.Home.Contact;
using DemoApplication.Areas.Client.ViewModels.Home.Index;
using DemoApplication.Areas.Client.ViewModels.Plant;
using DemoApplication.Contracts.File;
using DemoApplication.Contracts.User;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("home")]
    public class HomeController : Controller
    {
        private readonly DataContext _dbContext;
        private readonly IFileService _fileService;

        public HomeController(DataContext dbContext, IFileService fileService)
        {
            _dbContext = dbContext;
            _fileService = fileService;
        }

        [HttpGet("~/", Name = "client-home-index")]
        [HttpGet("index")]
        public async Task<IActionResult> IndexAsync()
        {
            List<string> links = new List<string>();
            var model = new IndexViewModel
            {
              

                Sliders = await _dbContext.Sliders.
                Select(s => new SliderListItemViewModel(s.Offer, s.Title, s.Content, s.BgImageName,
                _fileService.GetFileUrl(s.BgImageNameInFileSystem, UploadDirectory.Slider),
                s.ButtonName, s.BtnRedirectUrl, s.Order)).ToListAsync(),

                FeedBacks = await _dbContext.FeedBacks
                    .Select(fb => new FedBackListItemViewModel(fb.ProfilPhotoName,
                    _fileService.GetFileUrl(fb.ProfilPhotoNameInFileSystem,UploadDirectory.FeedBack),
                    $"{fb.Name} {fb.SurName}", GetRoleName.GetRoleNameByCode(fb.Role), fb.Content))
                    .ToListAsync(),
               

            };



            return View(model);
        }

        [HttpGet("send-contact", Name = "client-send-contact")]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost("send-contact", Name = "client-send-contact")]
        public ActionResult Contact([FromForm] AddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var contact = new Contact
            {
                Name = model.Name,
                LastName = model.LastName,
                Phone = model.PhoneNumber,
                Email = model.Email,
                Message = model.Message
            };

            _dbContext.Contacts.AddAsync(contact);

            _dbContext.SaveChangesAsync();

            return RedirectToRoute("client-home-index");
        }


        [HttpGet("plant-modal/{plantId}", Name = "client-plant-modal")]
        public async Task<IActionResult> GetPlantModal(int plantId)
        {
            var plant = await _dbContext.Plants
                .Include(p => p.PlantSizes)
                .Include(p => p.PlantColors)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == plantId);
            if (plant == null) return NotFound();

            var model = new PlantModalListItemViewModal
            {
                Id = plant.Id,
                Name = plant.Name,
                Price = plant.Price,
                Content = plant.Content,
                ImageUrl = plant.Images.Take(1).FirstOrDefault()! != null
                 ? _fileService.GetFileUrl(plant.Images!.Take(1)!.FirstOrDefault()!.ImageNameInFileSystem!, UploadDirectory.Plant) : String.Empty,
                Sizes = _dbContext.PlantSizes
                .Include(ps => ps.Size)
                .Where(ps => ps.PlantId == plant.Id)
                .Select(ps => new PlantModalListItemViewModal.SizeItemViewModel(ps.SizeId, ps.Size.Name))
                .ToList(),

                Colors = _dbContext.PlantColors
                .Include(pc => pc.Color)
                .Where(pc => pc.PlantId == plant.Id)
                .Select(pc => new PlantModalListItemViewModal.ColorItemViewModel(pc.ColorId, pc.Color.Name))
                .ToList()
            };


            return PartialView("~/Areas/Client/Views/Shared/Partials/_PlantModelPartial.cshtml", model);
        }

    }
}

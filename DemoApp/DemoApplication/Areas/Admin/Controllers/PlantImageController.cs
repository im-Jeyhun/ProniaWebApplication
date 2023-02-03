using DemoApplication.Areas.Admin.ViewModels.PlantImage;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/plant-image")]
    //[Authorize(Roles = "admin")]
    public class BookImageController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;

        public BookImageController(
            DataContext dataContext,
            IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        [HttpGet("{plantId}/image/list", Name = "admin-plant-image-list")]
        public async Task<IActionResult> ListAsync([FromRoute] int plantId)
        {
            var plant = await _dataContext.Plants.Include(b => b.Images)
                .FirstOrDefaultAsync(p => p.Id == plantId);

            if (plant == null) return NotFound();

            var model = new PlantImagesViewModel
            {
                PlantName = plant.Name,
                PlantId = plant.Id,

            };

            model.Images = plant.Images.Select(pi => new PlantImagesViewModel.ListItem
            {
                Id = pi.Id,
                ImageUrL = _fileService.GetFileUrl(pi.ImageNameInFileSystem, UploadDirectory.Book)
            }).ToList();

            return View(model);
        }

        [HttpGet("{plantId}/image/add", Name = "admin-plant-image-add")]
        public async Task<IActionResult> AddAsync()
        {
            return View(new AddViewModel());
        }

        [HttpPost("{plantId}/image/add", Name = "admin-plant-image-add")]
        public async Task<IActionResult> AddAsync([FromRoute] int plantId, AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var plant = await _dataContext.Plants.Include(b => b.Images)
                .FirstOrDefaultAsync(p => p.Id == plantId);

            if (plant is null) return NotFound();

            var plantImageNameInFileSystem = await _fileService.UploadAsync(model.Image, UploadDirectory.Plant);

            AddPlantImage(model.Image.FileName, plantImageNameInFileSystem);

            await _dataContext.SaveChangesAsync();


            void AddPlantImage(string bookImageName, string bookImageNameInFileSystem)
            {
                var plantImage = new Image
                {
                    ImageName = bookImageName,
                    ImageNameInFileSystem = bookImageNameInFileSystem,
                    Plant = plant,
                    Order = model.Order
                };

                _dataContext.Images.AddAsync(plantImage);

            }

            return RedirectToRoute("admin-plant-image-list", new { plantId = plant.Id });
        }

        [HttpGet("{plantId}/image/{plantImageId}/update", Name = "admin-plant-image-update")]
        public async Task<IActionResult> UpdateAsyn([FromRoute] int plantId, int plantImageId)
        {
            var plantImage = await _dataContext.Images
                .FirstOrDefaultAsync(bi => bi.Id == plantImageId && bi.PlantId == plantId);

            if (plantImage is null) return NotFound();

            var model = new UpdateViewModel
            {
                PlantId = plantImage.PlantId,
                PlantImageId = plantImage.Id,
                ImageUrl = _fileService.GetFileUrl(plantImage.ImageName, UploadDirectory.Plant),

            };

            return View(model);
        }

        [HttpPost("{plantId}/image/{plantImageId}/update", Name = "admin-plant-image-update")]
        public async Task<IActionResult> UpdateAsyn([FromRoute] int plantId, int plantImageId, UpdateViewModel model)
        {
            var plantImage = await _dataContext.Images
               .FirstOrDefaultAsync(bi => bi.Id == plantImageId && bi.PlantId == plantId);

            if (plantImage is null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.ImageUrl = _fileService.GetFileUrl(plantImage.ImageName, UploadDirectory.Plant);
                return View(model);
            }
          

            if (model.Image is not null)
            {
                await _fileService.DeleteAsync(plantImage.ImageNameInFileSystem, UploadDirectory.Plant);
                var plantImageFileNameInSystem = await _fileService.UploadAsync(model.Image, UploadDirectory.Plant);
                await UpdatePlantImageAsync(model.Image.FileName, plantImageFileNameInSystem);
            }

            UpdatePlantImage();


            async Task UpdatePlantImageAsync(string plantImageName, string plantImageFileNameInSystem)
            {
                plantImage.ImageName = plantImageName;
                plantImage.ImageNameInFileSystem = plantImageFileNameInSystem;
                plantImage.Order = model.Order;
                 await _dataContext.SaveChangesAsync();

            };

            void UpdatePlantImage()
            {
                plantImage.Order = model.Order;
                _dataContext.SaveChanges();
            }


            return RedirectToRoute("admin-plant-image-list", new { plantId = plantImage.PlantId });
        }



        [HttpPost("{plantId}/image/{plantImageId}/delete", Name = "admin-plant-image-delete")]
        public async Task<IActionResult> DeleteAsync(int plantId, int plantImageId)
        {
            var plantImage = await _dataContext.Images
               .FirstOrDefaultAsync(bi => bi.Id == plantImageId && bi.PlantId == plantId);

            if (plantImage is null) return NotFound();

            await _fileService.DeleteAsync(plantImage.ImageNameInFileSystem, UploadDirectory.Plant);

            _dataContext.Images.Remove(plantImage);
            await _dataContext.SaveChangesAsync();


            return RedirectToRoute("admin-plant-image-list", new { plantId = plantImage.PlantId });
        }

    }
}

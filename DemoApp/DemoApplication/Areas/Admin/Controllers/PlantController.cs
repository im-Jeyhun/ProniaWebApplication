using DemoApplication.Areas.Admin.ViewModels.Plant;
using DemoApplication.Areas.Admin.ViewModels.Plant.Add;
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
    [Route("admin/plant")]
    [Authorize(Roles = "admin")]

    public class PlantController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<PlantController> _logger;
        private readonly IFileService _fileService;
        private readonly IProductService _productService;

        public PlantController(DataContext dataContext, ILogger<PlantController> logger, IFileService fileService, IProductService productService)
        {
            _dataContext = dataContext;
            _logger = logger;
            _fileService = fileService;
            _productService = productService;
        }

        #region List

        [HttpGet("list", Name = "admin-plant-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.Plants
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .Select(p => new ListItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Sku = p.Sku,
                    Content = p.Content,
                    Category = p.Category.Title,
                    SubCategory = p.SubCategory.Title,
                    ImageUrl = p.Images.Take(1).FirstOrDefault()! != null
                     ? _fileService.GetFileUrl(p.Images!.Take(1)!.FirstOrDefault()!.ImageNameInFileSystem!, UploadDirectory.Plant) : String.Empty

                }).ToListAsync();

            return View(model);
        }

        #endregion
        [HttpGet("add-plant", Name = "admin-plant-add")]
        public async Task<IActionResult> AddAsync()
        {
            var model = new AddViewModel
            {
                Categiries = _dataContext.Categories
                .Select(c => new ViewModels.Category.ListItemViewModel(c.Id, c.Title)).ToList(),
                Colors = _dataContext.Colors
                .Select(c => new ColorListItemViewModel(c.Id, c.Name)).ToList(),
                Sizes = _dataContext.Sizes
                .Select(s => new SizeListItemViewModel(s.Id, s.Name)).ToList(),
                Tags = _dataContext.Tags
                .Select(t => new TagListItemViewModel(t.Id, t.Name)).ToList(),

            };

            return View(model);
        }

        [HttpPost("add-plant", Name = "admin-plant-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid) return GetView();

            if (!_dataContext.Categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(string.Empty, "Category is not found");
                return GetView();
            }

            if (!_dataContext.SubCategories.Any(sc => sc.Id == model.SubCategoryId))
            {
                ModelState.AddModelError(string.Empty, "Subcategory is not found");
                return GetView();
            }

            foreach (var colorId in model.ColorIds)
            {
                if (!_dataContext.Colors.Any(c => c.Id == colorId))
                {
                    ModelState.AddModelError(string.Empty, "something went wrong");
                    _logger.LogWarning($"Color with id({colorId}) not found in db ");
                    return GetView();
                }

            }

            foreach (var tagId in model.TagIds)
            {
                if (!_dataContext.Tags.Any(t => t.Id == tagId))
                {
                    ModelState.AddModelError(string.Empty, "something went wrong");
                    _logger.LogWarning($"Tag with id({tagId}) not found in db ");
                    return GetView();
                }

            }

            foreach (var sizeId in model.SizeIds)
            {
                if (!_dataContext.Sizes.Any(t => t.Id == sizeId))
                {
                    ModelState.AddModelError(string.Empty, "something went wrong");
                    _logger.LogWarning($"Size with id({sizeId}) not found in db ");
                    return GetView();
                }

            }

            var plant = CreatePlant();

           await CreatePlantTags();

            await CreatePlantColors();

           await CreatePlantSize();

           await CreatePlantImage();

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-plant-list");
            Plant CreatePlant()
            {
                var plant = new Plant()
                {
                    Sku = _productService.GenerateSku(),
                    Name = model.Name,
                    Price = model.Price,
                    Content = model.Content,
                    CategoryId = model.CategoryId,
                    SubCategoryId = model.SubCategoryId,
                };

                _dataContext.Plants.Add(plant);

                return plant;
            }

            async Task CreatePlantTags()
            {
                foreach (var tagId in model.TagIds)
                {
                    var plantTag = new PlantTag
                    {
                        Plant = plant,
                        TagId = tagId
                    };

                   await _dataContext.PlantTags.AddAsync(plantTag);

                }
            }

            async Task CreatePlantColors()
            {
                foreach (var colorId in model.ColorIds)
                {
                    var plantColor = new PlantColor
                    {
                        Plant = plant,
                        ColorId = colorId
                    };

                   await _dataContext.PlantColors.AddAsync(plantColor);
                }
            }


            async Task CreatePlantSize()
            {
                foreach (var sizeId in model.SizeIds)
                {
                    var plantSize = new PlantSize
                    {
                        Plant = plant,
                        SizeId = sizeId
                    };

                   await _dataContext.PlantSizes.AddAsync(plantSize);
                }
            }

            async Task CreatePlantImage()
            {
                foreach (IFormFile imageName in model.Images)
                {
                    var imageNameInSystem = await _fileService.UploadAsync(imageName, UploadDirectory.Plant);

                    var image = new Image
                    {
                        ImageName = imageName.FileName,
                        ImageNameInFileSystem = imageNameInSystem,
                        Plant = plant
                    };
                    await _dataContext.Images.AddAsync(image);
                }
            }


            IActionResult GetView()
            {
                model.Categiries = _dataContext.Categories
                .Select(c => new ViewModels.Category.ListItemViewModel(c.Id, c.Title)).ToList();
                model.Colors = _dataContext.Colors
                .Select(c => new ColorListItemViewModel(c.Id, c.Name)).ToList();
                model.Sizes = _dataContext.Sizes
                .Select(s => new SizeListItemViewModel(s.Id, s.Name)).ToList();
                model.Tags = _dataContext.Tags
                .Select(t => new TagListItemViewModel(t.Id, t.Name)).ToList();

                return View(model);
            }
        }

        [HttpGet("uptade-plant/{id}", Name = "admin-plant-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute]int id)
        {
            var plant = await _dataContext.Plants
                .Include(p => p.PlantColors)
                .Include(p => p.PlantTags)
                .Include(p => p.PlantSizes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plant is null) return NotFound();

            var model = new UpdateViewModel
            {
                Id = plant.Id,
                Name = plant.Name,
                Price = plant.Price,
                Content = plant.Content,
                CategoryId = plant.CategoryId,
                Categiries = _dataContext.Categories
                .Select(c => new ViewModels.Category.ListItemViewModel(c.Id, c.Title)).ToList(),
                SubCategoryId = plant.SubCategoryId,
                SubCategories = _dataContext.SubCategories
                .Where(sc => sc.CategoryId == plant.CategoryId)
                .Select(sc => new ViewModels.SubCategory.ListItemViewModel(sc.Id,sc.Title ,sc.Category.Title))
                .ToList(),
                ColorIds = plant!.PlantColors.Select(pc => pc.ColorId).ToList(),
                Colors = _dataContext.Colors
                .Select(c => new ColorListItemViewModel(c.Id, c.Name)).ToList(),
                SizeIds = plant.PlantSizes.Select(ps => ps.SizeId).ToList(),
                Sizes = _dataContext.Sizes
                .Select(s => new SizeListItemViewModel(s.Id, s.Name)).ToList(),
                TagIds = plant!.PlantTags.Select(ps => ps.TagId).ToList(),
                Tags = _dataContext.Tags
                .Select(t => new TagListItemViewModel(t.Id, t.Name)).ToList(),
            };

            return View(model);
        }

        [HttpPost("uptade-plant/{id}", Name = "admin-plant-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, UpdateViewModel model)
        {
            var plant = await _dataContext.Plants
                .Include(p => p.PlantColors)
                .Include(p => p.PlantTags)
                .Include(p => p.PlantSizes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (!ModelState.IsValid) return GetView();

            if (plant is null) return NotFound();

            if (!_dataContext.Categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(string.Empty, "Category is not found");
                return GetView();
            }

            if (!_dataContext.SubCategories.Any(sc => sc.Id == model.SubCategoryId))
            {
                ModelState.AddModelError(string.Empty, "Subcategory is not found");
                return GetView();
            }

            foreach (var colorId in model.ColorIds)
            {
                if (!_dataContext.Colors.Any(c => c.Id == colorId))
                {
                    ModelState.AddModelError(string.Empty, "something went wrong");
                    _logger.LogWarning($"Color with id({colorId}) not found in db ");
                    return GetView();
                }

            }

            foreach (var tagId in model.TagIds)
            {
                if (!_dataContext.Tags.Any(t => t.Id == tagId))
                {
                    ModelState.AddModelError(string.Empty, "something went wrong");
                    _logger.LogWarning($"Tag with id({tagId}) not found in db ");
                    return GetView();
                }

            }

            foreach (var sizeId in model.SizeIds)
            {
                if (!_dataContext.Sizes.Any(t => t.Id == sizeId))
                {
                    ModelState.AddModelError(string.Empty, "something went wrong");
                    _logger.LogWarning($"Size with id({sizeId}) not found in db ");
                    return GetView();
                }

            }

            await UpdatePlant();

            return RedirectToRoute("admin-plant-list");


            IActionResult GetView()
            {
                model.Categiries = _dataContext.Categories
                .Select(c => new ViewModels.Category.ListItemViewModel(c.Id, c.Title)).ToList();
                model.SubCategories = _dataContext.SubCategories
                .Where(sc => sc.CategoryId == plant.CategoryId)
                .Select(sc => new ViewModels.SubCategory.ListItemViewModel(sc.Id, sc.Title, sc.Category.Title))
                .ToList();
                model.Colors = _dataContext.Colors
                .Select(c => new ColorListItemViewModel(c.Id, c.Name)).ToList();
                model.Sizes = _dataContext.Sizes
                .Select(s => new SizeListItemViewModel(s.Id, s.Name)).ToList();
                model.Tags = _dataContext.Tags
                .Select(t => new TagListItemViewModel(t.Id, t.Name)).ToList();


                return View(model);

            }

            async Task UpdatePlant()
            {
                plant.Name = model.Name;
                plant.Price = model.Price;
                plant.Content = model.Content;
                plant.CategoryId = model.CategoryId;
                plant.SubCategoryId = model.SubCategoryId;

                UpdatePlantSize();

                UpdatePlantColor();

                UpdatePlantTag();

              await  _dataContext.SaveChangesAsync();


            }

            void UpdatePlantSize()
            {
                //databazada olan plantin olculerini getirmek
                var sizesInDb = plant!.PlantSizes.Select(p => p.SizeId).ToList();

                //databazada planta aid olan olculeri ayirmaq modeldeki olculerle
                var sizesToRemove = sizesInDb.Except(model.SizeIds).ToList();

                //modelden gelen yeni olculer databasada planta aid olan olculerden ayirmaq
                var sizesToAdd = model.SizeIds.Except(sizesInDb).ToList();

                plant.PlantSizes.RemoveAll(ps => sizesToRemove.Contains(ps.SizeId));

                foreach (var sizeId in sizesToAdd)
                {
                    var size = new PlantSize
                    {
                        SizeId = sizeId,
                        PlantId = plant.Id
                    };

                    _dataContext.PlantSizes.Add(size);
                }
            }

            void UpdatePlantColor()
            {
                //databazada olan plantin regnlerini getirmek
                var colorsInDb = plant!.PlantColors.Select(p => p.ColorId).ToList();

                //databazada planta aid olan renglero ayirmaq modeldeki olculerle
                var colorsToRemove = colorsInDb.Except(model.ColorIds).ToList();

                //modelden gelen yeni rengleri databasada planta aid olan olculerden ayirmaq
                var colorsToAdd = model.ColorIds.Except(colorsInDb).ToList();

                plant.PlantColors.RemoveAll(ps => colorsToRemove.Contains(ps.ColorId));

                foreach (var colorId in colorsToAdd)
                {
                    var color = new PlantColor
                    {
                        ColorId = colorId,
                        PlantId = plant.Id
                    };

                    _dataContext.PlantColors.Add(color);
                }
            }

            void UpdatePlantTag()
            {
                //databazada olan plantin taglarini getirmek
                var tagsInDb = plant!.PlantTags.Select(p => p.TagId).ToList();

                //databazada planta aid olan taglari ayirmaq modeldeki taglara
                var tagsToRemove = tagsInDb.Except(model.TagIds).ToList();

                //modelden gelen yeni taglari databasada planta aid olan taglardan ayirmaq
                var tagsToAdd = model.TagIds.Except(tagsInDb).ToList();

                plant.PlantTags.RemoveAll(pt => tagsToRemove.Contains(pt.TagId));

                foreach (var tagId in tagsToAdd)
                {
                    var tag = new PlantTag
                    {
                        TagId = tagId,
                        PlantId = plant.Id
                    };

                    _dataContext.PlantTags.Add(tag);
                }
            }
        }

        [HttpPost("delete-plant/{id}", Name = "admin-plant-delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var plant = await _dataContext.Plants
                .Include(p => p.PlantColors)
                .Include(p => p.PlantTags)
                .Include(p => p.PlantSizes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plant is null) return NotFound();

            _dataContext.Plants.Remove(plant);
            await _dataContext.SaveChangesAsync();
            return RedirectToRoute("admin-plant-list");

        }

        [HttpGet("get-current-subcategories/{id}", Name = "admin-get-subcategories")]
        public async Task<IActionResult> GettCurrentSubCategories(int id)
        {
            var model = new SubCategoryListItemViewModel
            {
                SubCategories = _dataContext.SubCategories.Where(sc => sc.CategoryId == id)
                .Select(sc => new SubCategoryListItemViewModel.SubCategorItemViewModel(sc.Id, sc.Title)).ToList()
            };

            return PartialView("~/Areas/Admin/Views/Shared/Partials/Plant/_SubCategoryListPartial.cshtml", model);
        }

    }
}

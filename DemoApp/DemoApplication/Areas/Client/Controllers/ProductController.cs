using DemoApplication.Areas.Client.ViewComponents;
using DemoApplication.Areas.Client.ViewModels.Plant;
using DemoApplication.Areas.Client.ViewModels.PlantItem;
using DemoApplication.Areas.Client.ViewModels.Product;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;

        public ProductController(DataContext dataContext, IUserService userService, IFileService fileService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _fileService = fileService;
        }

        [HttpGet("product-item/{id}", Name = "client-product-item")]
        public async Task<IActionResult> ItemAsync(int id)
        {
            var product = await _dataContext.Plants
                .Include(p => p.PlantColors)
                .Include(p => p.PlantSizes)
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .Include(p => p.PlantTags)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return NotFound();


            var model = new ItemViewModel
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                Price = product.Price,
                Content = product.Content,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Title,
                SubCategoryId = product.SubCategoryId,
                SubCategoryName = product.SubCategory.Title,
                Sizes = _dataContext.PlantSizes
                .Include(ps => ps.Size)
                .Where(ps => ps.PlantId == product.Id)
                .Select(ps => new ItemViewModel.SizeItemViewModel(ps.SizeId, ps.Size.Name))
                .ToList(),

                Colors = _dataContext.PlantColors
                .Include(pc => pc.Color)
                .Where(pc => pc.PlantId == product.Id)
                .Select(pc => new ItemViewModel.ColorItemViewModel(pc.ColorId, pc.Color.Name))
                .ToList(),
                Tags = _dataContext.PlantTags
                .Include(pt => pt.Tag)
                .Where(pt => pt.PlantId == product.Id)
                .Select(pt => new ItemViewModel.TagItemViewModel(pt.TagId, pt.Tag.Name)).ToList(),
                ImageUrls = product.Images.Select(pi => new ItemViewModel.ImageItemViewModel(pi.ImageName, _fileService.GetFileUrl(pi.ImageNameInFileSystem, UploadDirectory.Plant))).ToList(),



            };

            return View(model);
        }

        [HttpGet("product-search", Name = "client-product-search")]
        public async Task<IActionResult> PageAsync(string Search = null)
        
        {
            var model = new ListItemViewModel
            {
                //Products = _dataContext.Plants.Include(p => p.Images).Select(p => new ListItemVIewModel(p.Id, p.Name, p.Price,
                //     p.Images.Take(1).FirstOrDefault()! != null
                //     ? _fileService.GetFileUrl(p.Images!.Take(1)!.FirstOrDefault()!.ImageNameInFileSystem!, UploadDirectory.Plant) : String.Empty,
                //     p.Images.Take(1).FirstOrDefault()! != null
                //     ? _fileService.GetFileUrl(p.Images!.Skip(1).Take(1)!.FirstOrDefault()!.ImageNameInFileSystem!, UploadDirectory.Plant) : String.Empty
                //      )).ToList(),
                Colors = _dataContext.Colors
                .Select(c => new ListItemViewModel.ColorViewModel(c.Id , c.Name)).ToList(),
                Sizes = _dataContext.Sizes
                .Select(s => new ListItemViewModel.SizeViewModel(s.Id , s.Name)).ToList(),
                Categories = _dataContext.Categories
                .Select(c => new ListItemViewModel.CategoryViewModel(c.Id , c.Title )).ToList(),
                SubCategories = _dataContext.SubCategories
                .Select(sb => new ListItemViewModel.SubCategoryViewModel(sb.Id , sb.Title)).ToList(),
                Tags  = _dataContext.Tags
                .Select(t => new ListItemViewModel.TagViewModel(t.Id , t.Name)).ToList(),
                Search = Search

            };


            return View(model);
        }

        [HttpPost("product-searchby", Name = "client-product-searchby")]
        public async Task<IActionResult> SearchAsync(string Search)
        {

            return RedirectToRoute("client-product-search", new { Search = Search });
        }

        [HttpGet("product-filter", Name = "client-product-filter")]
        public async Task<IActionResult> FilterAsync(ListItemViewModel model , string Search)
        {

            return ViewComponent(nameof(FilteredProduct), new { CategoryId = model.CategoryId , ColorId = model.ColorId , TagId = model.TagId , Search = Search , MaxPrice = model.MaxPrice , MinPrice = model.MinPrice});
        }
    }
}

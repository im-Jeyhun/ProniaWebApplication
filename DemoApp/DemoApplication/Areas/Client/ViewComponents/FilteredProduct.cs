using DemoApplication.Areas.Client.ViewModels.Product;
using DemoApplication.Areas.Client.ViewModels.PlantItem;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Client.ViewComponents
{
    [ViewComponent(Name = "FilteredProduct")]

    public class FilteredProduct : ViewComponent
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;

        public FilteredProduct(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? CategoryId = null , int? ColorId = null , int? TagId=null , string Search = null, decimal? MaxPrice = null, decimal? MinPrice = null)
        
        {
            var query = _dataContext.Plants.AsQueryable();


            if (!string.IsNullOrWhiteSpace(Search))
                query = query.Where(p => p.Name.Contains(Search));

            if (CategoryId != null)
            {
                query = query.Where(p => p.CategoryId == CategoryId);
            }
            if(ColorId != null)
            {
                query = query.Where(p => p.PlantColors.Any(p => p.ColorId == ColorId));
            }
            if (TagId != null)
            {
                query = query.Where(p => p.PlantTags.Any(p => p.TagId == TagId));
            }
            if(MaxPrice != null || MinPrice != null)
            {
                query = query.Where(p => p.Price! >= MinPrice && p.Price <= MaxPrice);

            }

            var model = new ListItemViewModel
            {
                Products = query.Include(p => p.Images).Select(p => new ListItemVIewModel(p.Id, p.Name, p.Price,
                    p.Images.Take(1).FirstOrDefault()! != null
                    ? _fileService.GetFileUrl(p.Images!.Take(1)!.FirstOrDefault()!.ImageNameInFileSystem!, UploadDirectory.Plant) : String.Empty,
                    p.Images.Take(1).FirstOrDefault()! != null
                    ? _fileService.GetFileUrl(p.Images!.Skip(1).Take(1)!.FirstOrDefault()!.ImageNameInFileSystem!, UploadDirectory.Plant) : String.Empty
                     )).ToList()
            };

            return View(model);
        }
    }
}

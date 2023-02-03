using DemoApplication.Areas.Client.ViewModels.Product;
using DemoApplication.Areas.Client.ViewModels.PlantItem;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoApplication.Contracts.Plant;

namespace DemoApplication.Areas.Client.ViewComponents
{
    [ViewComponent(Name = "PlantItem")]

    public class PlantItem : ViewComponent
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;

        public PlantItem(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ShowDirectory showDirectory)
        {
            
            var query = _dataContext.Plants.AsQueryable();
            var model = new List<ListItemVIewModel>();

            if (showDirectory == ShowDirectory.NewPlants)
            {
                query = query.Take(4).OrderByDescending(p => p.CreatedAt);
            }
            else if (showDirectory == ShowDirectory.BestSeller)
            {
                query = query
                   .OrderByDescending(
                        p => p.OrderProducts.Where(
                            op => op.Order.Status == Database.Models.Enums.OrderStatus.Completed).Count())
                   .Take(5);

                var result = query.ToList();
            }
            

            model = await query.Take(8).Select(p => new ListItemVIewModel(p.Id, p.Name, p.Price,
                 p.Images.Take(1).FirstOrDefault()! != null
                 ? _fileService.GetFileUrl(p.Images!.Take(1)!.FirstOrDefault()!.ImageNameInFileSystem!, UploadDirectory.Plant) : String.Empty,
                p.Images.Take(1).FirstOrDefault()! != null
                 ? _fileService.GetFileUrl(p.Images!.Skip(1).Take(1)!.FirstOrDefault()!.ImageNameInFileSystem!, UploadDirectory.Plant) : String.Empty
                )).ToListAsync();

            return View(model);
        }
    }
}

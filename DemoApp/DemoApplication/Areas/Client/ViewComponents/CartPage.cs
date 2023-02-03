using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DemoApplication.Areas.Client.ViewComponents
{
    [ViewComponent(Name = "CartPage")]
    public class CartPage : ViewComponent
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;

        public CartPage(DataContext dataContext, IUserService userService, IFileService fileService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _fileService = fileService;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<ProductCookieViewModel> viewModel)
        {

            // Case 1 : Qeydiyyat kecilib, o zaman bazadan gotur
            if (_userService.IsAuthenticated)
            {
                var model = await _dataContext.BasketProducts
                    .Where(bp => bp.Basket.UserId == _userService.CurrentUser.Id).
                    Include(bp => bp.Plant).Include(bp => bp.Plant.PlantColors).Include(bp => bp.Plant.PlantSizes)
                    .Select(bp =>
                        new ProductCookieViewModel(
                            bp.PlantId,
                            bp.Plant!.Name,
                            bp!.Plant!.Images.Take(1).FirstOrDefault()! != null ? _fileService.GetFileUrl(bp.Plant.Images!.Take(1)!.FirstOrDefault()!.ImageNameInFileSystem!, UploadDirectory.Plant) : String.Empty,
                            bp.Quantity,
                            bp.Plant.Price,
                            bp.Plant.Price * bp.Quantity , bp.ColorId , bp.SizeId ,
                              _dataContext.PlantColors.Include(ps => ps.Color).Where(ps => ps.PlantId == bp.PlantId)
                            .Select(ps => new ProductCookieViewModel.ColorItemViewModel(ps.ColorId, ps.Color.Name)).ToList(),
                            
                            _dataContext.PlantSizes.Include(ps => ps.Size).Where(ps => ps.PlantId == bp.PlantId)
                            .Select(ps => new ProductCookieViewModel.SizeItemViewModel(ps.SizeId , ps.Size.Name)).ToList()
                          ))
                    .ToListAsync();

                

                ViewBag.AuthenticatedBasket = model.Count();

                return View(model);
            }

            if(viewModel is not null)
            {
                return View(viewModel);
            }


            //Case 2: Qeydiyyatdan kecmeyib cookiden gotur oxu
            var productsCookieValue = HttpContext.Request.Cookies["products"];
            var productsCookieViewModel = new List<ProductCookieViewModel>();
            if (productsCookieValue is not null)
            {
                productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productsCookieValue);
            }



            return View(productsCookieViewModel);
        }
    }
}

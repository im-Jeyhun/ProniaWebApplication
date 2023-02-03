using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Extensions;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DemoApplication.Areas.Client.ViewComponents
{
    public class ShopCart : ViewComponent
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;

        public ShopCart(DataContext dataContext, IUserService userService, IFileService fileService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _fileService = fileService;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<ProductCookieViewModel>? viewModels = null)
        {
            // Case 1 : Qeydiyyat kecilib, o zaman bazadan gotur
            if (_userService.IsAuthenticated)
            {

                var model = await _dataContext.BasketProducts
                    .Where(bp => bp.Basket.UserId == _userService.CurrentUser.Id).
                    Include(bp => bp.Plant)
                    .Select(bp =>
                        new ProductCookieViewModel(
                            bp.PlantId,
                            bp.Plant!.Name,
                            bp!.Plant!.Images.Take(1).FirstOrDefault()! != null ? _fileService.GetFileUrl(bp.Plant.Images!.Take(1)!.FirstOrDefault()!.ImageNameInFileSystem!, UploadDirectory.Plant) : String.Empty,
                            bp.Quantity,
                            bp.Plant.Price,
                            bp.Plant.Price * bp.Quantity))
                    .ToListAsync();

                ViewBag.AuthenticatedBasket = model.Count();

                return View(model);
            }

            //Case 2: Argument olaraq actiondan gonderilib
            if (viewModels is not null)
            {
                return View(viewModels);
            }

            //Case 3: Argument gonderilmeyib bu zaman cookiden oxu
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

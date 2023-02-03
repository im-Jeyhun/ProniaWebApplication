using DemoApplication.Areas.Client.ViewComponents;
using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Areas.Client.ViewModels.Plant;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        private readonly IBasketService _basketService;

        public CartController(DataContext dataContext, IUserService userService, IFileService fileService, IBasketService basketService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _fileService = fileService;
            _basketService = basketService;
        }
        [HttpGet("cart-list", Name = "client-cart-list")]
        public async Task<IActionResult> List()
        {

            return View();
        }

        [HttpGet("cart-delete/{plantId}", Name = "client-cart-delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int plantId)
        {
            if (!_userService.IsAuthenticated)
            {

                var product = await _dataContext.Plants.FirstOrDefaultAsync(p => p.Id == plantId);
                if (product is null)
                {
                    return NotFound();
                }

                var productCookieValue = HttpContext.Request.Cookies["products"];
                if (productCookieValue is null)
                {
                    return NotFound();
                }

                var productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue);
                productsCookieViewModel!.RemoveAll(pcvm => pcvm.Id == plantId);

                HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productsCookieViewModel));
                return ViewComponent(nameof(CartPage), productsCookieViewModel);
            }
            else
            {

                var basketProduct = await _dataContext.BasketProducts
                        .FirstOrDefaultAsync(bp => bp.Basket.UserId == _userService.CurrentUser.Id && bp.PlantId == plantId);

                if (basketProduct is null) return NotFound();

                _dataContext.BasketProducts.Remove(basketProduct);
            }

            await _dataContext.SaveChangesAsync();
            return ViewComponent(nameof(CartPage));

        }

        [HttpGet("cart-invidial-delete/{plantId}", Name = "client-cart-delete-invidual")]
        public async Task<IActionResult> DeleteInviduallAsync([FromRoute] int plantId)
        {
            if (!_userService.IsAuthenticated)
            {

                var product = await _dataContext.Plants.FirstOrDefaultAsync(p => p.Id == plantId);
                if (product is null)
                {
                    return NotFound();
                }

                var productCookieValue = HttpContext.Request.Cookies["products"];
                if (productCookieValue is null)
                {
                    return NotFound();
                }

                var productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue);
                foreach (var productInCokie in productsCookieViewModel)
                {
                    if (productInCokie.Id == product.Id)
                    {

                        if (productInCokie.Quantity > 1)
                        {
                            productInCokie.Quantity -= 1;
                            productInCokie.Total = productInCokie.Quantity * productInCokie.Price;
                        }
                        else
                        {
                            productsCookieViewModel.Remove(productInCokie);
                            break;
                        }
                    }

                }


                HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productsCookieViewModel));
                return ViewComponent(nameof(CartPage), productsCookieViewModel);
            }
            else
            {

                var basketProduct = await _dataContext.BasketProducts
                        .FirstOrDefaultAsync(bp => bp.Basket.UserId == _userService.CurrentUser.Id && bp.PlantId == plantId);

                if (basketProduct is null) return NotFound();

                if (basketProduct.Quantity >= 1)
                {

                    basketProduct.Quantity -= 1;

                }
                else
                {

                    _dataContext.BasketProducts.Remove(basketProduct);

                }

            }

            await _dataContext.SaveChangesAsync();
            return ViewComponent(nameof(CartPage));

        }

        [HttpGet("cart-list-async/{id}", Name = "client-cart-list-async")]
        public async Task<IActionResult> ListAsync(int id , PlantModalListItemViewModal model)
        {
            var product = await _dataContext.Plants.FirstOrDefaultAsync(p => p.Id == id);
            if (product is null)
            {
                return NotFound();
            }

            var productsCookieViewModel = await _basketService.AddBasketProductAsync(product , model);
            if (productsCookieViewModel.Any())
            {
                return ViewComponent(nameof(CartPage), productsCookieViewModel);
            }

            return ViewComponent(nameof(CartPage));

        }
        [HttpGet("cart-shop-update", Name = "client-shop-update")]
        public async Task<IActionResult> UpdateShop()
        {
            return ViewComponent(nameof(ShopCart));

        }
    }
}

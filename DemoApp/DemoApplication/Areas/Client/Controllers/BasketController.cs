using DemoApplication.Areas.Client.ViewComponents;
using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Areas.Client.ViewModels.Plant;
using DemoApplication.Database;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("basket")]
    public class BasketController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IBasketService _basketService;
        private readonly IUserService _userService;

        public BasketController(DataContext dataContext, IBasketService basketService, IUserService userService)
        {
            _dataContext = dataContext;
            _basketService = basketService;
            _userService = userService;
        }


        [HttpPost("add/{id}", Name = "client-basket-add")]
        public async Task<IActionResult> AddProductAsync([FromRoute] int id , PlantModalListItemViewModal model)
        {
            var product = await _dataContext.Plants.FirstOrDefaultAsync(p => p.Id == id);
            if (product is null)
            {
                return NotFound();
            }

            var productsCookieViewModel = await _basketService.AddBasketProductAsync(product , model);
            if (productsCookieViewModel.Any())
            {
                return ViewComponent(nameof(ShopCart), productsCookieViewModel);
            }

            return ViewComponent(nameof(ShopCart));
        }

        //[HttpPost("add/{id}", Name = "client-basket-add-bymodal")]
        //public async Task<IActionResult> AddProductByModalAsync([FromRoute] int id , PlantModalListItemViewModal model)
        //{
        //    var product = await _dataContext.Plants
        //        .Include(p => p.PlantSizes).Include(p => p.PlantColors).FirstOrDefaultAsync(p => p.Id == id);

        //    if (product is null)
        //    {
        //        return NotFound();
        //    }

        //    //databazada olan plantin olculerini getirmek
        //    var sizesInDb = product!.PlantSizes.Select(p => p.SizeId).ToList();

        //    //databazada planta aid olan olculeri ayirmaq modeldeki olculerle
        //    var sizesToRemove = sizesInDb.Except(model.SizeId);

        //    //modelden gelen yeni olculer databasada planta aid olan olculerden ayirmaq
        //    var sizesToAdd = model.SizeIds.Except(sizesInDb).ToList();

        //    var productsCookieViewModel = await _basketService.AddBasketProductAsync(product);
        //    if (productsCookieViewModel.Any())
        //    {
        //        return ViewComponent(nameof(ShopCart), productsCookieViewModel);
        //    }

        //    return ViewComponent(nameof(ShopCart));
        //}

        [HttpGet("delete/{plantId}", Name = "client-basket-delete")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] int plantId)
        {
            var product = await _dataContext.Plants.FirstOrDefaultAsync(b => b.Id == plantId);
            if (product is null)
            {
                return NotFound();
            }

            if (_userService.IsAuthenticated)
            {
                var basketProduct = await _dataContext.BasketProducts.FirstOrDefaultAsync(p => p.PlantId == product.Id);

                _dataContext.BasketProducts.Remove(basketProduct);
                await _dataContext.SaveChangesAsync();
                return ViewComponent(nameof(ShopCart));
            }
            var productCookieValue = HttpContext.Request.Cookies["products"];
            if (productCookieValue is null)
            {
                return NotFound();
            }

            var productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue);
            productsCookieViewModel!.RemoveAll(pcvm => pcvm.Id == product.Id);

            HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productsCookieViewModel));

            return ViewComponent(nameof(ShopCart), productsCookieViewModel);
        }
    }
}

using DemoApplication.Areas.Client.ViewModels.Checkout;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Database.Models.Enums;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("checkout")]
    //[Authorize]
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public CheckoutController(DataContext dataContext, IUserService userService, IOrderService orderService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _orderService = orderService;
        }
        [HttpGet("checkoutlist", Name = "client-checkout-checkoutlist")]
        public async Task<IActionResult> Checkoutlist()
        {
            var model = new ProductListItemViewModel
            {

                Products = await _dataContext.BasketProducts.Include(bp => bp.Plant).Include(bp => bp.Size).Include(bp => bp.Color)
                 .Where(bp => bp.Basket!.UserId == _userService.CurrentUser.Id)
                 .Select(bp => new ProductListItemViewModel.ListItem(bp.PlantId, bp.Plant.Name, bp.Quantity, bp.Plant.Price, bp.Plant.Price * bp.Quantity, $"{bp.Color.Name}", $"{bp.Size.Name}"))
                 .ToListAsync(),
                SumTotal = await _dataContext.BasketProducts
                .Where(bp => bp.Basket!.UserId == _userService.CurrentUser.Id)
                .SumAsync(bp => bp.Plant.Price * bp.Quantity)
            };

            if(model is null)
            {
                model = new ProductListItemViewModel();
            }

            return View(model);
        }

        [HttpPost("place-order", Name = "client-checkout-place-order")]
        public async Task<IActionResult> PlaceOrder()
        {
            var pasketProducts = _dataContext.BasketProducts
                .Include(bp => bp.Color)
                .Include(bp => bp.Size)
                .Include(bp => bp.Plant)
                .Select(bp => new ProductListItemViewModel.ListItem(bp.PlantId, bp.Plant.Name,
                 bp.Quantity, bp.Plant.Price, bp.Plant.Price * bp.Quantity, bp.Color.Name, bp.Size.Name))
                .ToList();

            if(pasketProducts is null)
            {
                return NotFound();
            }

            var createOrder = await CreateOrder();

            foreach (var basketProduct in pasketProducts)
            {
                var orderProduct = new OrderProduct
                {
                    PlantId = basketProduct.Id,
                    Quantity = basketProduct.Quantity,
                    OrderId = createOrder.Id,
                    Color = basketProduct.Color,
                    Size = basketProduct.Size /*!=null ? _dataContext.PlantSizes.Where(ps => ps.PlantId == basketProduct.Id).Select()*/
                    //Color = basketProduct.Color != null ? basketProduct.Color :
                    //_dataContext.Plants.Include(p => p.PlantColors).Where(p => p.Id == basketProduct.Id).First().PlantColors!.First().Color.Name,
                    //Size = basketProduct.Size != null ? basketProduct.Size : 
                    //_dataContext.Plants.Include(p => p.PlantSizes).Where(p => p.Id == basketProduct.Id).First().PlantSizes!.First().Size.Name
                };

                _dataContext.OrderProducts.Add(orderProduct);
            }

            await DeleteBasketProducts();

            _dataContext.SaveChanges();

            async Task<Order> CreateOrder()
            {
                var order = new Order
                {
                    Id = _orderService.GenerateId(),
                    UserId = _userService.CurrentUser.Id,
                    Status = OrderStatus.Created,
                    SumTotalPrice = _dataContext.BasketProducts.
                    Where(bp => bp.Basket.UserId == _userService.CurrentUser.Id).Sum(bp => bp.Plant.Price * bp.Quantity)

                };

                await _dataContext.Orders.AddAsync(order);

                return order;

            }

            async Task DeleteBasketProducts()
            {
                var removedBasketProducts = await _dataContext.BasketProducts
                       .Where(bp => bp.Basket.UserId == _userService.CurrentUser.Id).ToListAsync();

                removedBasketProducts.ForEach(bp => _dataContext.BasketProducts.Remove(bp));
            }

            return RedirectToRoute("client-checkout-checkoutlist");
        }

    }





}

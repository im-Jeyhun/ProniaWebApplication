using DemoApplication.Areas.Client.ViewModels.Account;
using DemoApplication.Database;
using DemoApplication.Database.Models.Enums;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("account")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;

        public AccountController(DataContext dataContext, IUserService userService, IFileService fileService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _fileService = fileService;
        }

        [HttpGet("dashboard", Name = "client-account-dashboard")]
        public IActionResult Dashboard()
        {
            var user = _userService.CurrentUser;
            var user2 = _userService.CurrentUser;

            return View();
        }

        [HttpGet("orders", Name = "client-account-orders")]
        public async Task<IActionResult> Orders()
        {
            var model = await _dataContext.Orders.Where(o => o.UserId == _userService.CurrentUser.Id).Select(o => new OrderListItemViewModel
            {
                Id = o.Id,
                OrderStatus = StatusStatusCode.GetStatusCode((OrderStatus)o.Status),
                OrderSum = o.SumTotalPrice,
                OrderTime = o.CreatedAt,
                OrderProducts = _dataContext.OrderProducts.Where(op => op.OrderId == o.Id).ToList()
            }).ToListAsync();

            return View(model);
        }

        [HttpGet("order-product/{orderId}", Name = "client-account-order-products")]
        public async Task<IActionResult> OrderProduct(string orderId)
        {
            var orderProductModel =  await _dataContext.OrderProducts.Where(op => op.OrderId == orderId)
                .Select(op => new PlanttListItemViewModel(
                    op.Plant.Images.Take(1).FirstOrDefault() != null ?
                    _fileService.GetFileUrl(op.Plant.Images.Take(1).FirstOrDefault().ImageNameInFileSystem,
                    Contracts.File.UploadDirectory.Plant) : String.Empty, op.Plant.Name, op.Plant.Price, op.Color,
                    op.Size, op.Quantity , op.OrderId , op.Total

                    )).ToListAsync();

            return View(orderProductModel);
        }


    }
}

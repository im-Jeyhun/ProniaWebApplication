using DemoApplication.Areas.Client.ViewModels.Authentication;
using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Areas.Client.ViewModels.Plant;
using DemoApplication.Contracts.File;
using DemoApplication.Contracts.Identity;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Exceptions;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace DemoApplication.Services.Concretes
{
    public class BasketService : IBasketService
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;

        public BasketService(DataContext dataContext, IUserService userService, IHttpContextAccessor httpContextAccessor, IFileService fileService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
        }


        public async Task<List<ProductCookieViewModel>> AddBasketProductAsync(Plant plant, PlantModalListItemViewModal model)
        {
            if (_userService.IsAuthenticated)
            {
                await AddToDatabaseAsync();

                return new List<ProductCookieViewModel>();
            }

            return AddToCookie();





            //Add product to database if user is authenticated
            async Task AddToDatabaseAsync()
            {
                var basketProduct = await _dataContext.BasketProducts
                    .FirstOrDefaultAsync(bp => bp.Basket.UserId == _userService.CurrentUser.Id && bp.PlantId == plant.Id);
                if (basketProduct is not null)
                {

                    basketProduct.SizeId = model.SizeId != null ? model.SizeId : _dataContext.PlantSizes.First().SizeId;
                    basketProduct.ColorId = model.ColorId != null ? model.ColorId : _dataContext.PlantColors.First().ColorId;
                    basketProduct.Quantity++;
                }
                else
                {
                    var basket = await _dataContext.Baskets.FirstAsync(b => b.UserId == _userService.CurrentUser.Id);

                    basketProduct = new BasketProduct
                    {
                        Quantity = model.Quantity != null ? model.Quantity : 1,
                        BasketId = basket.Id,
                        PlantId = plant.Id,
                        SizeId = model.SizeId != null ? model.SizeId : _dataContext.PlantSizes.First().SizeId,
                        ColorId = model.ColorId != null ? model.ColorId : _dataContext.PlantColors.First().ColorId

                    };

                    await _dataContext.BasketProducts.AddAsync(basketProduct);
                }

                await _dataContext.SaveChangesAsync();
            }


            //Add product to cookie if user is not authenticated 
            List<ProductCookieViewModel> AddToCookie()
            {

                plant = _dataContext.Plants.Include(p => p.Images).FirstOrDefault(p => p.Id == plant.Id);

                var productCookieValue = _httpContextAccessor.HttpContext.Request.Cookies["products"];
                var productsCookieViewModel = productCookieValue is not null
                    ? JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue)
                    : new List<ProductCookieViewModel> { };

                var productCookieViewModel = productsCookieViewModel!.FirstOrDefault(pcvm => pcvm.Id == plant.Id);
                if (productCookieViewModel is null)
                {
                    productsCookieViewModel
                    !.Add(new ProductCookieViewModel(plant.Id, plant.Name, plant.Images.Take(1)!.FirstOrDefault()! != null
                        ? _fileService.GetFileUrl(plant.Images!.Take(1)!.FirstOrDefault()!.ImageNameInFileSystem!, UploadDirectory.Plant) : string.Empty,
                        model.Quantity != null ? model.Quantity : 1, plant.Price,
                        model.Quantity != null ? model.Quantity * plant.Price : plant.Price,
                        model.ColorId != null ? model.ColorId : null, model.SizeId != null ? model.SizeId : null,
                        _dataContext.PlantColors.Include(ps => ps.Color).Where(ps => ps.PlantId == plant.Id)
                            .Select(ps => new ProductCookieViewModel.ColorItemViewModel(ps.ColorId, ps.Color.Name)).ToList(),
                        _dataContext.PlantSizes.Include(ps => ps.Size).Where(ps => ps.PlantId == plant.Id)
                            .Select(ps => new ProductCookieViewModel.SizeItemViewModel(ps.SizeId, ps.Size.Name)).ToList()
                        ));
                }
                else
                {



                    productCookieViewModel.Quantity = model.Quantity != null ? productCookieViewModel.Quantity += model.Quantity : productCookieViewModel.Quantity += 1;

                    productCookieViewModel.ColorId = model.ColorId != null ? model.ColorId : productCookieViewModel.ColorId;

                    productCookieViewModel.SizeId = model.SizeId != null ? model.SizeId : productCookieViewModel.SizeId;


                    productCookieViewModel.Total = productCookieViewModel.Quantity * productCookieViewModel.Price;
                }

                _httpContextAccessor.HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productsCookieViewModel));

                return productsCookieViewModel;
            }
        }
    }
}

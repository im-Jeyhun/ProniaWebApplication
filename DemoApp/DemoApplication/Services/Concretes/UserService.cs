﻿using DemoApplication.Areas.Client.ViewModels.Authentication;
using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Contracts.Identity;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Exceptions;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;
using DemoApplication.Contracts.Email;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DemoApplication.Services.Concretes
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private User _currentUser;
        private readonly IEmailService _emailService;
        private readonly IUrlHelper _urlHelper;
        private readonly IUserActivationService _userActivationService;


        public UserService(
            DataContext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService,
            IUrlHelper urlHelper,
            IUserActivationService userActivationService)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _urlHelper = urlHelper;
            _userActivationService = userActivationService;
        }

        public bool IsAuthenticated
        {
            get => _httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated;
        }

        public User CurrentUser
        {
            get
            {
                if (_currentUser is not null)
                {
                    return _currentUser;
                }

                var idClaim = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(C => C.Type == CustomClaimNames.ID);
                if (idClaim is null)
                    throw new IdentityCookieException("Identity cookie not found");

                _currentUser = _dataContext.Users.First(u => u.Id == Guid.Parse(idClaim.Value));

                return _currentUser;
            }
        }


        public string GetCurrentUserFullName()
        {
            return $"{CurrentUser.FirstName} {CurrentUser.LastName}";
        }

        public bool IsEmailConfirmed(string email)
        {
            var user = _dataContext.Users.FirstOrDefault(u => u.Email == email);

            return user.IsEmailConfirmed == true;
        }

        public async Task<bool> CheckPasswordAsync(string? email, string? password)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            //return await _dataContext.Users.AnyAsync(u => u.Email == email && u.Password == password);
            return user is not null && BC.Verify(password, user.Password);

        }

        public async Task SignInAsync(Guid id, string? role = null , bool rememberMe = default)
        {
            var claims = new List<Claim>
            {
                new Claim(CustomClaimNames.ID, id.ToString())
            };

            if (role is not null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(identity);

            var Remember = new AuthenticationProperties
            {
                IsPersistent = rememberMe
            };
            await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal , Remember);
        }

        public async Task SignInAsync(string? email, string? password, string? role = null, bool rememberMe = default)
        {
            var user = await _dataContext.Users.FirstAsync(u => u.Email == email);

            if (user is not null && BC.Verify(password, user.Password) && user.IsEmailConfirmed == true)
            {
                await SignInAsync(user.Id, role , rememberMe);
            }

        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task CreateAsync(RegisterViewModel model)
        {
            var user = await CreateUserAsync();
            var basket = await CreateBasketAsync();

            await CreteBasketProductsAsync();

            await _userActivationService.SendActivationUrlAsync(user);

            await _dataContext.SaveChangesAsync();

            async Task<User> CreateUserAsync()
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = BC.HashPassword(model.Password),

                };
                await _dataContext.Users.AddAsync(user);

                return user;
            }

          

            async Task<Basket> CreateBasketAsync()
            {
                //Create basket process
                var basket = new Basket
                {
                    User = user,
                  
                };
                await _dataContext.Baskets.AddAsync(basket);

                return basket;
            }

            async Task CreteBasketProductsAsync()
            {
                //Add products to basket if cookie exists
                var productCookieValue = _httpContextAccessor.HttpContext!.Request.Cookies["products"];
                if (productCookieValue is not null)
                {
                    var productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue);
                    foreach (var productCookieViewModel in productsCookieViewModel)
                    {
                        var plant = await _dataContext.Plants.FirstOrDefaultAsync(p => p.Id == productCookieViewModel.Id);
                        var basketProduct = new BasketProduct
                        {
                            Basket = basket,
                            PlantId = plant!.Id,
                            Quantity = productCookieViewModel.Quantity,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        await _dataContext.BasketProducts.AddAsync(basketProduct);
                    }
                }
            }
        }

       
    }
}

using DemoApplication.Areas.Admin.ViewModels.PaymentBenefit;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/pamyent-benefit")]
    public class PaymentBenefitController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        public PaymentBenefitController(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }
        [HttpGet("list", Name = "admin-payment-benefit-list")]
        public async Task<IActionResult> List()

        {
            var model = await _dataContext.PaymentBenefits
                .Select(p => new ListItemViewModel(p.Id, p.Name, p.ImageName,
                _fileService.GetFileUrl(p.ImageNameInFileSystem, UploadDirectory.PaymentBenefits), p.Content))
                .ToListAsync();

            return View(model);
        }
        [HttpGet("add-payment-benefit", Name = "admin-add-payment-benefit")]
        public async Task<IActionResult> AddAsync()
        {
            return View();
        }

        [HttpPost("add-payment-benefit", Name = "admin-add-payment-benefit")]

        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var paymentBenefitImageNameInFileSysteym = await _fileService.UploadAsync(model!.PbImage, UploadDirectory.PaymentBenefits);

            await AddPaymentBenefit(model.PbImage.FileName, paymentBenefitImageNameInFileSysteym);

            await _dataContext.SaveChangesAsync();

            async Task AddPaymentBenefit(string pbIgameName, string pbImageNameInFileSystem)
            {
                var paymentBenefit = new PaymentBenefit
                {
                    Name = model!.Name,
                    Content = model!.Content,
                    ImageName = pbIgameName,
                    ImageNameInFileSystem = pbImageNameInFileSystem,
                };

                await _dataContext!.PaymentBenefits!.AddAsync(paymentBenefit);
            }


            return RedirectToRoute("admin-payment-benefit-list");
        }

        [HttpGet("update-payment-benefit{id}", Name = "admin-update-payment-benefit")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id)
        {
            var paymentBenefit = await _dataContext.PaymentBenefits.FirstOrDefaultAsync(p => p.Id == id);

            if (paymentBenefit is null) return NotFound();

            var model = new UpdateViewModel
            {
                Name = paymentBenefit.Name,
                Content = paymentBenefit.Content,
                ImageUrl = _fileService.GetFileUrl(paymentBenefit.ImageNameInFileSystem, UploadDirectory.PaymentBenefits),
                ImageName = paymentBenefit.ImageName
            };

            return View(model);
        }

        [HttpPost("update-payment-benefit{id}", Name = "admin-update-payment-benefit")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, UpdateViewModel model)
        {
            var paymentBenefit = await _dataContext.PaymentBenefits.FirstOrDefaultAsync(p => p.Id == id);

            if (!ModelState.IsValid) GetView(model);

            if (paymentBenefit is null) return NotFound();


            if (model.PbImage is not null)
            {
                await _fileService.DeleteAsync(paymentBenefit.ImageNameInFileSystem, UploadDirectory.PaymentBenefits);
                var PmImageNameInFileSystem = await _fileService.UploadAsync(model.PbImage, UploadDirectory.PaymentBenefits);
                await UpdatePaymentBenefitAsync(model.PbImage.FileName, PmImageNameInFileSystem);
            }
            else
            {
                UpdatePaymentBenefit();
            }


            return RedirectToRoute("admin-payment-benefit-list");

            IActionResult GetView(UpdateViewModel model)
            {
                model.ImageUrl = _fileService.GetFileUrl(paymentBenefit.ImageNameInFileSystem, UploadDirectory.PaymentBenefits);
                return View(model);
            }

            void UpdatePaymentBenefit()
            {
                paymentBenefit.Name = model!.Name;
                paymentBenefit.Content = model!.Content;

                _dataContext.SaveChanges();

            };

            async Task UpdatePaymentBenefitAsync(string PmImageName, string PmImageNameInFileSystem)
            {

                paymentBenefit.Name = model.Name;
                paymentBenefit.Content = model.Content;
                paymentBenefit.ImageName = PmImageName;
                paymentBenefit.ImageNameInFileSystem = PmImageNameInFileSystem;
                await _dataContext.SaveChangesAsync();

            }


        }

        [HttpPost("delete-payment-benefit{id}", Name = "admin-delete-payment-benefit")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var paymentBenefit = await _dataContext.PaymentBenefits.FirstOrDefaultAsync(p => p.Id == id);

            if (paymentBenefit is null) return NotFound();

            await _fileService.DeleteAsync(paymentBenefit.ImageNameInFileSystem, UploadDirectory.PaymentBenefits);

            _dataContext.PaymentBenefits.Remove(paymentBenefit);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-payment-benefit-list");

        }
    }
}

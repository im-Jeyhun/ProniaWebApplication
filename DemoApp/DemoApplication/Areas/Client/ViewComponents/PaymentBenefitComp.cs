using DemoApplication.Areas.Client.ViewModels.Home.Index;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Client.ViewComponents
{
    [ViewComponent(Name = "PaymentBenefitComp")]

    public class PaymentBenefitComp : ViewComponent
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;

        public PaymentBenefitComp(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var model = await _dataContext.PaymentBenefits.Take(3).
                Select(pb => new IndexViewModel.PaymentBenefitsViewModel(pb.Name, pb.ImageName,
                _fileService.GetFileUrl(pb.ImageNameInFileSystem, UploadDirectory.PaymentBenefits), pb.Content)).ToListAsync();

            return View(model);
        }
    }
}

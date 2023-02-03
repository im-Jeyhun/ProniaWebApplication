using DemoApplication.Areas.Client.ViewModels.Reward;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Client.ViewComponents
{
    [ViewComponent(Name = "RewardItem")]
    public class RewardItem : ViewComponent
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;

        public RewardItem(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var model = await _dataContext.Rewards.Select(r => new ItemViewModel(r.ImageName,
               _fileService.GetFileUrl(r.ImageNameInFileSystem, UploadDirectory.Reward)
               )).ToListAsync();

            return View(model);
        }
    }
}

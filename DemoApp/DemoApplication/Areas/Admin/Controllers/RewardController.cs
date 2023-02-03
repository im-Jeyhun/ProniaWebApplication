using DemoApplication.Areas.Admin.ViewModels.Reward;
using DemoApplication.Contracts.File;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/reward")]
    [Authorize(Roles = "admin")]

    public class RewardController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        public RewardController(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }
        [HttpGet("list", Name = "admin-reward-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.Rewards.Select(r => new ListItemViewModel(r.Id,
            _fileService.GetFileUrl(r.ImageNameInFileSystem, UploadDirectory.Reward))).ToListAsync();

            return View(model);
        }

        [HttpGet("add-reward", Name = "admin-reward-add")]
        public async Task<IActionResult> AddAsync()
        {
            return View();
        }

        [HttpPost("add-reward", Name = "admin-reward-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var rewardImageNameInFileSystem = await _fileService.UploadAsync(model.RewardImage, UploadDirectory.Reward);

            var reward = new Reward
            {
                ImageName = model.RewardImage.FileName,
                ImageNameInFileSystem = rewardImageNameInFileSystem
            };

            await _dataContext.Rewards.AddAsync(reward);
            await _dataContext.SaveChangesAsync();


            return RedirectToRoute("admin-reward-list");
        }

        [HttpGet("update-reward/{id}", Name = "admin-reward-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id)
        {
            var reward = await _dataContext.Rewards.FirstOrDefaultAsync(r => r.Id == id);

            if (reward is null) return NotFound();

            var model = new UpdateViewModel
            {
                Id = reward.Id,
                ImageName = reward.ImageName,
                ImageUrl = _fileService.GetFileUrl(reward.ImageNameInFileSystem, UploadDirectory.Reward)
            };
            return View(model);
        }

        [HttpPost("update-reward/{id}", Name = "admin-reward-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, UpdateViewModel model)
        {
            var reward = await _dataContext.Rewards.FirstOrDefaultAsync(r => r.Id == id);

            if (!ModelState.IsValid)
            {
                model.ImageUrl = _fileService.GetFileUrl(reward.ImageNameInFileSystem, UploadDirectory.Reward);

            }

            if (reward is null) return NotFound();

            await _fileService.DeleteAsync(reward.ImageNameInFileSystem, UploadDirectory.Reward);
            var rewardImageNameInFileSystem = await _fileService.UploadAsync(model.RewardImage, UploadDirectory.Reward);

            reward.ImageName = model.RewardImage.FileName;
            reward.ImageNameInFileSystem = rewardImageNameInFileSystem;

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-reward-list");

        }

        [HttpPost("delete-reward/{id}", Name = "admin-reward-delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var reward = await _dataContext.Rewards.FirstOrDefaultAsync(r => r.Id == id);
            if (reward is null) return NotFound();
            await _fileService.DeleteAsync(reward.ImageNameInFileSystem, UploadDirectory.Reward);

            _dataContext.Rewards.Remove(reward);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-reward-list");
        }
    }
}

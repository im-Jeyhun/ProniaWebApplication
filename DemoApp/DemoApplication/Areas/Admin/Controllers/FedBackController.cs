using DemoApplication.Areas.Admin.ViewModels.FeedBack;
using DemoApplication.Contracts.File;
using DemoApplication.Contracts.User;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/fedback")]
    [Authorize(Roles = "admin")]

    public class FedBackController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        public FedBackController(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }
        [HttpGet("list", Name = "admin-fedback-list")]
        public async Task<IActionResult> ListAsync()
        {
            var model = await _dataContext.FeedBacks
                .Select(fb => new ListItemViewModel
                {
                    Id = fb.Id,
                    FullName = $"{fb.Name} {fb.SurName}",
                    Role = GetRoleName.GetRoleNameByCode(fb.Role),
                    CreatedAt = fb.CreatedAt,
                    UpdatedAt = fb.UpdatedAt,
                    ProfilePhotoUrl = _fileService.GetFileUrl(fb.ProfilPhotoNameInFileSystem, UploadDirectory.FeedBack)
                }).ToListAsync();
            return View(model);
        }
        [HttpGet("add-fedback", Name = "admin-fedback-add")]
        public async Task<IActionResult> AddAsync()
        {
            return View();
        }

        [HttpPost("add-fedback", Name = "admin-fedback-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var feedBackPpImageNameInFileSystem = await _fileService.UploadAsync(model.ProfilePhoto, UploadDirectory.FeedBack);

            var feedBack = new FeedBack
            {
                Name = model.Name,
                SurName = model.LastName,
                ProfilPhotoName = model.ProfilePhoto.FileName,
                ProfilPhotoNameInFileSystem = feedBackPpImageNameInFileSystem,
                Role = model.Role,
                Content = model.Content,
            };

            await _dataContext.FeedBacks.AddAsync(feedBack);
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-fedback-list");
        }

        [HttpGet("update-fedback/{id}", Name = "admin-fedback-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id)
        {
            var feedBack = await _dataContext.FeedBacks.FirstOrDefaultAsync(fb => fb.Id == id);

            if (feedBack is null) return NotFound();
            var model = new UpdateViewModel
            {
                Name = feedBack.Name,
                LastName = feedBack.SurName,
                Content = feedBack.Content,
                Role = feedBack.Role,
                ProfilePhotoUrl = _fileService.GetFileUrl(feedBack.ProfilPhotoNameInFileSystem, UploadDirectory.FeedBack)

            };

            return View(model);
        }

        [HttpPost("update-fedback/{id}", Name = "admin-fedback-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, UpdateViewModel model)
        {

            var feedBack = await _dataContext.FeedBacks.FirstOrDefaultAsync(fb => fb.Id == id);

            if (!ModelState.IsValid) return GetView();

            if (feedBack is null) return NotFound();

            if (model.ProfilePhoto is not null)
            {
                await _fileService.DeleteAsync(feedBack.ProfilPhotoNameInFileSystem, UploadDirectory.FeedBack);
                var feedBackPpImageNameInFileSystem = await _fileService.UploadAsync(model.ProfilePhoto, UploadDirectory.FeedBack);
                UpdateFeedBackImage(model.ProfilePhoto.FileName, feedBackPpImageNameInFileSystem);
            }

            UpdateFeedBack();

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-fedback-list");


            IActionResult GetView()
            {
                model.ProfilePhotoUrl = _fileService.GetFileUrl(feedBack.ProfilPhotoNameInFileSystem, UploadDirectory.FeedBack);

                return View(model);
            }

            void UpdateFeedBack()
            {
                feedBack.Name = model.Name;
                feedBack.SurName = model.LastName;
                feedBack.Content = model.Content;
                feedBack.Role = model.Role;

            }

            void UpdateFeedBackImage(string imageName, string imageNameInFileSystem)
            {
                feedBack.ProfilPhotoName = imageName;
                feedBack.ProfilPhotoNameInFileSystem = imageNameInFileSystem;
            }
        }

        [HttpPost("delete-fedback/{id}", Name = "admin-fedback-delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var feedBack = await _dataContext.FeedBacks.FirstOrDefaultAsync(fb => fb.Id == id);

            if (feedBack is null) return NotFound();

            await _fileService.DeleteAsync(feedBack.ProfilPhotoNameInFileSystem, UploadDirectory.FeedBack);

            _dataContext.FeedBacks.Remove(feedBack);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-fedback-list");

        }
    }
}

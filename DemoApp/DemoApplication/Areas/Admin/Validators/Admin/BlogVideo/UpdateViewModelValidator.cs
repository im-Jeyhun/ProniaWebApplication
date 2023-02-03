using DemoApplication.Areas.Admin.ViewModels.BlogVideo;
using DemoApplication.Contracts;
using DemoApplication.Contracts.FileSizes;
using DemoApplication.Validators;
using FluentValidation;

namespace DemoApplication.Areas.Admin.Validators.BlogVideo
{
    public class UpdateViewModelValidator : AbstractValidator<UpdateViewModel>
    {
        public UpdateViewModelValidator()
        {
            RuleFor(uvm => uvm.Video)
               .Cascade(CascadeMode.Stop)

               .NotNull()
               .WithMessage("Image can't be empty")

               .SetValidator(
                    new FileValidator(2, FileSizes.Mega,
                        FileExtensions.JPG.GetExtension(), FileExtensions.PNG.GetExtension())!);
        }
    }
}

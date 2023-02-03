﻿using DemoApplication.Areas.Admin.ViewModels.Slider;
using DemoApplication.Contracts;
using DemoApplication.Contracts.FileSizes;
using DemoApplication.Validators;
using FluentValidation;

namespace DemoApplication.Areas.Admin.Validators.SliderImage
{
    public class UpdateViewModelValidator : AbstractValidator<UpdateViewModel>
    {
        public UpdateViewModelValidator()
        {
            RuleFor(uvm => uvm.BgImage)
               .Cascade(CascadeMode.Stop)

               .NotNull()
               .WithMessage("Image can't be empty")

               .SetValidator(
                    new FileValidator(2, FileSizes.Mega,
                        FileExtensions.JPG.GetExtension(), FileExtensions.PNG.GetExtension())!);
        }
    }
}

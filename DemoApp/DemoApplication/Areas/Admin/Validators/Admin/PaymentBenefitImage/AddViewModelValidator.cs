using DemoApplication.Areas.Admin.ViewModels.PaymentBenefit;
using DemoApplication.Contracts;
using DemoApplication.Contracts.FileSizes;
using DemoApplication.Validators;
using FluentValidation;

namespace DemoApplication.Areas.Admin.Validators.PaymentBenefitImage
{
    public class AddViewModelValidator : AbstractValidator<AddViewModel>
    {
        public AddViewModelValidator()
        {
            RuleFor(avm => avm.PbImage)
               .Cascade(CascadeMode.Stop)

               .NotNull()
               .WithMessage("Image can't be empty")

               .SetValidator(
                    new FileValidator(2, FileSizes.Mega,
                        FileExtensions.JPG.GetExtension(), FileExtensions.PNG.GetExtension())!);
        }
    }
}

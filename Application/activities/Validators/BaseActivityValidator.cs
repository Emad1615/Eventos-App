using Application.activities.DTOS;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.activities.Validators
{
    public class BaseActivityValidator<T, TDO> : AbstractValidator<T> where TDO : BaseActivityDTO
    {
        public BaseActivityValidator(Func<T, TDO> selector)
        {
            RuleFor(x => selector(x).Title).NotEmpty()
                .WithMessage("Activity Title is required").MaximumLength(100)
                .WithMessage("Title character must be less than 100");
            RuleFor(x => selector(x).Category).NotEmpty()
                .WithMessage("Activity Category is required");
            RuleFor(x => selector(x).Description).NotEmpty()
                .WithMessage("Activity Description is required")
                .MaximumLength(700).WithMessage("Title Description must be less than 700");
            RuleFor(x => selector(x).City).NotEmpty()
                .WithMessage("Activity City is required");
            RuleFor(x => selector(x).Venue).NotEmpty()
                .WithMessage("Activity Venue is required");
            RuleFor(x => selector(x).Date).NotEmpty()
                .WithMessage("Activity Description is required")
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Date should be in the future");
            RuleFor(x => selector(x).Latitude).ExclusiveBetween(-90, 90)
                .WithMessage("Latitude coord  must be between  -90 to 90");
            RuleFor(x => selector(x).Longitude).ExclusiveBetween(-180, 180)
                .WithMessage("Longitude coord  must be between  -180 to 180");
        }
    }
}

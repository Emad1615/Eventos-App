using Application.activities.Commands;
using Application.activities.DTOS;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.activities.Validators
{
    public class EditActivityValidator:BaseActivityValidator<EditActivity.Command,EditActivityDTO>
    {
        public EditActivityValidator():base(x=>x.activityDTO)
        {
            RuleFor(x => x.activityDTO.ID).NotEmpty().WithMessage("Activity Id is required");
        }
    }
}

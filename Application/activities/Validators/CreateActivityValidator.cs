using Application.activities.Commands;
using Application.activities.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.activities.Validators
{
    public class CreateActivityValidator:BaseActivityValidator<CreateActivity.Command,CreateActivityDTO>
    {
        public CreateActivityValidator():base(x=>x.activityDTO)
        {
            
        }
    }
}

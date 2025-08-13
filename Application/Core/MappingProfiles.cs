using Application.activities.DTOS;
using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ActivityListDTO, Activity>().ReverseMap();
            CreateMap<CreateActivityDTO, Activity>().ReverseMap();
            CreateMap<EditActivityDTO, Activity>().ReverseMap();
        }
    }
}

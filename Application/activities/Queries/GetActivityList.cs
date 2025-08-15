using Application.activities.DTOS;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.activities.Queries
{
    public class GetActivityList
    {
        public class Query : IRequest<List<ActivityListDTO>>
        {

        }
        public class Handler(AppDbContext context, ILogger<GetActivityList> logger, IMapper mapper) : IRequestHandler<Query, List<ActivityListDTO>>
        {
            public async Task<List<ActivityListDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await context.activities.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync(cancellationToken);
                return mapper.Map<List<ActivityListDTO>>(activities);
            }
        }
    }
}

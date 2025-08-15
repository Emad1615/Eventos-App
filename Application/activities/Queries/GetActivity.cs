using Application.activities.DTOS;
using Application.Core;
using AutoMapper;
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
    public class GetActivity
    {
        public class Query : IRequest<Result<ActivityListDTO>>
        {
            public string? ID { get; set; }
        }
        public class Handler(AppDbContext context, ILogger<GetActivity> logger, IMapper mapper) : IRequestHandler<Query, Result<ActivityListDTO>>
        {
            public async Task<Result<ActivityListDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await context.activities.FindAsync([request.ID], cancellationToken);
                if (activity == null) {
                    logger.LogError(message: "Actvity not found or not activity matches with this id");
                    return Result<ActivityListDTO>.Failure(404, "Activity not found");
                } 
                return Result<ActivityListDTO>.Success(mapper.Map<ActivityListDTO>(activity));
            }
        }
    }
}

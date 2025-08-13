using Application.activities.DTOS;
using MediatR;
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
        public class Query : IRequest<ActivityListDTO> {
            public string? ID { get; set; }
        }
        public class handler(AppDbContext context,ILogger<GetActivity> logger) : IRequestHandler<Query, ActivityListDTO>
        {
            public async Task<ActivityListDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}

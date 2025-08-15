using Application.activities.DTOS;
using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.activities.Commands
{
    public class CreateActivity
    {
        public class Command : IRequest<Result<string>>
        {
            public required CreateActivityDTO activityDTO { get; set; }
        }
        public class Handler(AppDbContext context, ILogger<EditActivity> logger, IMapper mapper) : IRequestHandler<Command, Result<string>>
        {
            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var dto = mapper.Map<Activity>(request.activityDTO);
                await context.activities.AddAsync(dto);
                var result = await context.SaveChangesAsync(cancellationToken);
                if (result > 0) return Result<string>.Success(dto.ID);
                return Result<string>.Failure(400, "Activity not created please try again or call technical support");

            }
        }
    }
}

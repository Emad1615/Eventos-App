using Application.activities.DTOS;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.activities.Commands
{
    public class CreateActivity
    {
        public class Command : IRequest<string> {
            public required CreateActivityDTO activityDTO { get; set; }
        }
        public class Handler(AppDbContext context, ILogger<EditActivity> logger) : IRequestHandler<Command, string>
        {
            public Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}

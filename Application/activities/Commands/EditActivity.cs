using Application.activities.DTOS;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.activities.Commands
{
    public class EditActivity
    {
        public class Command : IRequest<Unit> {
            public required EditActivityDTO activityDTO { get; set; }
        }
        public class Handler(AppDbContext context,ILogger<EditActivity> logger) : IRequestHandler<Command, Unit>
        {
            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}

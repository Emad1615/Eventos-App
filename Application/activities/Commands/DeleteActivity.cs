using Application.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.activities.Commands
{
    public class DeleteActivity
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required string ID { get; set; }
            public required string DeletedUserID { get; set; }
        }
        public class Handler(AppDbContext context, ILogger<DeleteActivity> logger) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var activity = await context.activities.FindAsync([request.ID], cancellationToken);
                    if (activity == null) return Result<Unit>.Failure(404, "Activity not found");
                    activity.IsDeleted = true;
                    activity.DeletedDate = DateTime.UtcNow;
                    activity.DeletedUserID = request.DeletedUserID;
                    var result = await context.SaveChangesAsync(cancellationToken) > 0;
                    if (!result) return Result<Unit>.Failure(400, "Failed to delete selected activity please try again");
                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception ex)
                {
                    return Result<Unit>.Failure(500, ex.Message);
                }

            }
        }
    }
}

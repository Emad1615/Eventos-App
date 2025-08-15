using Application.activities.DTOS;
using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.activities.Commands
{
    public class EditActivity
    {
        public class Command : IRequest<Result<string>>
        {
            public required EditActivityDTO activityDTO { get; set; }
        }
        public class Handler(AppDbContext context, ILogger<EditActivity> logger, IMapper mapper) : IRequestHandler<Command, Result<string>>
        {
            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {

                var activity = await context.activities.FindAsync([request.activityDTO.ID], cancellationToken);
                if (activity == null) return Result<string>.Failure(404, "Activity not found");
                activity.Title = request.activityDTO.Title;
                activity.Description = request.activityDTO.Description;
                activity.Category = request.activityDTO.Category;
                activity.Date = request.activityDTO.Date;
                activity.City = request.activityDTO.City;
                activity.Venue = request.activityDTO.Venue;
                activity.Latitude = request.activityDTO.Latitude;
                activity.Longitude = request.activityDTO.Longitude;
                activity.IsUpdated = true;
                activity.UpdatedDateTime = DateTime.Now;
                activity.UpdatedUserID = request.activityDTO.UpdatedUserID;
                var result = await context.SaveChangesAsync(cancellationToken);
                if (result > 0) return Result<string>.Success(activity.ID);
                return Result<string>.Failure(400, "Activity data not be updated correctly please try again or call technical support");

            }
        }
    }
}

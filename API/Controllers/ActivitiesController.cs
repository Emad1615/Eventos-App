using Application.activities.Commands;
using Application.activities.DTOS;
using Application.activities.Queries;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet("activities")]
        public async Task<ActionResult<List<ActivityListDTO>>> GetActivities(CancellationToken cancellation)
        {
            return await Mediator.Send(new GetActivityList.Query { }, cancellation);
        }
        [HttpGet("activity")]
        public async Task<ActionResult<Result<ActivityListDTO>>> GetActivity(string id, CancellationToken cancellation)
        {
            return HandleResult(await Mediator.Send(new GetActivity.Query { ID = id }, cancellation));
        }
        [HttpPost("create-activity")]
        public async Task<ActionResult<Result<string>>> CreateActivity(CreateActivityDTO activityDTO, CancellationToken cancellation)
        {
            activityDTO.InsertionUserID = UserId;
            return HandleResult(await Mediator.Send(new CreateActivity.Command { activityDTO = activityDTO }, cancellation));
        }
        [HttpPut("update-activity")]
        public async Task<ActionResult<Result<string>>> UpdateActivity(EditActivityDTO activityDTO, CancellationToken cancellation)
        {
            activityDTO.UpdatedUserID = UserId;
            return HandleResult(await Mediator.Send(new EditActivity.Command { activityDTO = activityDTO }, cancellation));
        }
        [HttpDelete("delete-activity")]
        public async Task<ActionResult<Result<Unit>>> DeleteActivity(string id, CancellationToken cancellation)
        {
            return HandleResult(await Mediator.Send(new DeleteActivity.Command { ID = id, DeletedUserID = UserId }, cancellation));
        }
    }
}

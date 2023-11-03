using FlightDocsSystem.Models.Model;
using FlightDocsSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class GroupUserController : ControllerBase
    {
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly ILogger<GroupUserController> _logger;

        public GroupUserController(IGroupUserRepository groupUserRepository, ILogger<GroupUserController> logger)
        {
            _groupUserRepository = groupUserRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToGroup(GroupUserModel model)
        {
            try
            {
                var groupUser = new GroupUser()
                {
                    GroupId = model.GroupId,
                    UserId = model.UserId,
                };
                await _groupUserRepository.AddUserToGroupAsync(groupUser);

                string mgs = $"Add user {model.UserId} to group {model.GroupId} successfully";
                _logger.LogInformation(mgs);
                return Ok(mgs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when adding user {model.UserId} to group {model.GroupId}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when adding user {model.UserId} to group {model.GroupId}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUserFromGroup(GroupUserModel model)
        {
            try
            {
                var groupUser = new GroupUser()
                {
                    GroupId = model.GroupId,
                    UserId = model.UserId,
                };

                await _groupUserRepository.RemoveUserFromGroupAsync(groupUser);
                string mgs = $"Remove user {model.UserId} from group {model.GroupId} successfully";
                _logger.LogInformation(mgs);
                return Ok(mgs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when removing user {model.UserId} from group {model.GroupId}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when removing user {model.UserId} from group {model.GroupId}");
            }
        }
    }
}

using AutoMapper;
using FlightDocsSystem.Models.EditModel;
using FlightDocsSystem.Models.Model;
using FlightDocsSystem.Models.ViewModel;
using FlightDocsSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GroupController> _logger;

        public GroupController(IGroupRepository groupRepository, IGroupUserRepository groupUserRepository, IMapper mapper, ILogger<GroupController> logger)
        {
            _groupRepository = groupRepository;
            _groupUserRepository = groupUserRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            try
            {
                var data = await _groupRepository.GetAllGroupAsync();
                if (data == null) return NotFound();

                var dataVM = data.Select(x => _mapper.Map<GroupVM>(x));

                _logger.LogInformation("Get all groups successfully");
                return Ok(dataVM);
            } catch (Exception ex)
            {
                _logger.LogError($"Error when getting all groups: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when getting all groups");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            try
            {
                var data = await _groupRepository.GetGroupByIdAsync(id);
                if (data != null)
                {
                    var dataVM = _mapper.Map<GroupVM>(data);

                    _logger.LogInformation($"Get group with id {id} successfully");
                    return Ok(dataVM);
                } else
                {
                    return NotFound($"Group with id {id} does not exist");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when getting group with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when getting group with id {id}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(GroupModel group)
        {
            try
            {
                Group newGroup = new Group
                {
                    Name = group.Name,
                    CreatedDate = DateTime.UtcNow,
                    Note = group.Note,
                    CreatorId = group.CreatorId,
                };
                int groupId = await _groupRepository.CreateGroupAsync(newGroup);

                if (group.UserIds != null)
                {
                    foreach (var userId in group.UserIds)
                    {
                        await _groupUserRepository.AddUserToGroupAsync(new GroupUser()
                        {
                            GroupId = groupId,
                            UserId = userId
                        });
                    }
                }

                _logger.LogInformation($"Create new group with id {groupId} successfully");
                return Ok("Create group successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when creating new group: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when creating new group");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int id, GroupEM group)
        {
            try
            {
                await _groupRepository.UpdateGroupAsync(group);
                _logger.LogInformation($"Update group with id {id} successfully");
                return Ok($"Update group with id {id} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when updating group with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when updating group with id {id}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            try
            {
                await _groupRepository.DeleteGroupAsync(id);
                _logger.LogInformation($"Delete group with id {id} successfully");
                return Ok($"Delete group with id {id} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when deleting group with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when deleting group with id {id}");
            }
        }
    }
}

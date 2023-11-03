using FlightDocsSystem.Models.EditModel;

namespace FlightDocsSystem.Services
{
    public interface IGroupRepository
    {
        Task<ICollection<Group>?> GetAllGroupAsync();
        Task<Group?> GetGroupByIdAsync(int id);
        Task<int> CreateGroupAsync(Group group);
        Task UpdateGroupAsync(GroupEM group);
        Task DeleteGroupAsync(int id);
    }
}

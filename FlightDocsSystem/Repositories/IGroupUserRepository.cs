namespace FlightDocsSystem.Services
{
    public interface IGroupUserRepository
    {
        Task AddUserToGroupAsync(GroupUser groupUser);
        Task RemoveUserFromGroupAsync(GroupUser groupUser);
    }
}

namespace FlightDocsSystem.Services
{
    public class GroupUserRepository : IGroupUserRepository
    {
        private readonly DataContext _dataContext;

        public GroupUserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddUserToGroupAsync(GroupUser groupUser)
        {
            await _dataContext.GroupUsers.AddAsync(groupUser);
            await _dataContext.SaveChangesAsync();
        }

        public async Task RemoveUserFromGroupAsync(GroupUser groupUser)
        {
            _dataContext.GroupUsers.Remove(groupUser);
            await _dataContext.SaveChangesAsync();
        }
    }
}

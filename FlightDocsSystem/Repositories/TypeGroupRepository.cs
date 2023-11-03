namespace FlightDocsSystem.Repositories
{
    public class TypeGroupRepository : ITypeGroupRepository
    {
        private readonly DataContext _dataContext;

        public TypeGroupRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddGroupToTypeAsync(TypeGroup typeGroup)
        {
            await _dataContext.TypeGroups.AddAsync(typeGroup);
            await _dataContext.SaveChangesAsync();
        }

        public async Task RemoveGroupFromTypeAsync(TypeGroup typeGroup)
        {
            _dataContext.TypeGroups.Remove(typeGroup);
            await _dataContext.SaveChangesAsync();
        }
    }
}

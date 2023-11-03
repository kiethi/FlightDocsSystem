namespace FlightDocsSystem.Repositories
{
    public interface ITypeGroupRepository
    {
        Task AddGroupToTypeAsync(TypeGroup typeGroup);
        Task RemoveGroupFromTypeAsync(TypeGroup typeGroup);
    }
}

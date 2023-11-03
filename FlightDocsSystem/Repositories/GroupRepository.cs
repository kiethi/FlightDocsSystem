using AutoMapper;
using FlightDocsSystem.Models.EditModel;

namespace FlightDocsSystem.Services
{
    public class GroupRepository : IGroupRepository
    {
        private readonly DataContext _context;

        public GroupRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> CreateGroupAsync(Group group)
        {
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
            return group.GroupId;
        }

        public async Task DeleteGroupAsync(int id)
        {
            var group = await _context.Groups.SingleOrDefaultAsync(gr => gr.GroupId == id);
            if (group != null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<Group>?> GetAllGroupAsync()
        {
            var groups = await _context.Groups.ToListAsync();
            return groups;
        }
        public async Task<Group?> GetGroupByIdAsync(int id)
        {
            var group = await _context.Groups.SingleOrDefaultAsync(gr => gr.GroupId == id);
            if (group != null)
            {
                return group;
            }
            return null;
        }

        public async Task UpdateGroupAsync(GroupEM group)
        {
            var newGroup = await _context.Groups.SingleOrDefaultAsync(gr => gr.GroupId == group.GroupId);
            if (newGroup != null)
            {
                newGroup.Name = group.Name;
                newGroup.Note = group.Note;
            }

            await _context.SaveChangesAsync();  
        }
    }
}

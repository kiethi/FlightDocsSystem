using FlightDocsSystem.Helpers;
using System.Globalization;

namespace FlightDocsSystem.Repositories
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly DataContext _context;

        public DocumentTypeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> CreateDocumentTypeAsync(DocumentType type)
        {
            await _context.DocumentTypes.AddAsync(type);
            await _context.SaveChangesAsync();
            return type.DocumentTypeId;
        }

        public async Task DeleteDocumentTypeAsync(int id)
        {
            var type = await _context.DocumentTypes.SingleOrDefaultAsync(t => t.DocumentTypeId == id);
            if (type != null) { 
                _context.DocumentTypes.Remove(type);
                await _context.SaveChangesAsync();
            }
        }

        public ICollection<DocumentType>? GetAllDocumentTypes(string? search, DateTime? from, DateTime? to, int pageSize = 10, int page = 1)
        {
            var types = _context.DocumentTypes.AsQueryable();

            #region Filtering
            if (!string.IsNullOrEmpty(search))
            {
                types = types.Where(t => t.Name.Contains(search));
            }

            if (from.HasValue)
            {
                types = types.Where(t => t.CreatedDate >= from);
            }
            if (to.HasValue)
            {
                types = types.Where(t => t.CreatedDate <= to);
            }
            #endregion

            #region sorting
            //Default sort descending by date
            types = types.OrderBy(t => t.CreatedDate);
            #endregion

            var result = PaginatedList<DocumentType>.Create(types, page, pageSize);

            return result;
        }

        public async Task<DocumentType?> GetDocumentTypeByIdAsync(int id)
        {
            var type = await _context.DocumentTypes.SingleOrDefaultAsync(t => t.DocumentTypeId == id);
            if (type != null)
            {
                return type;
            }
            return null;
        }

        public async Task UpdateDocumentTypeAsync(DocumentType type)
        {
            var newType = await _context.DocumentTypes.SingleOrDefaultAsync(t => t.DocumentTypeId == type.DocumentTypeId);
            if (newType != null)
            {
                newType.Name = type.Name;
                newType.Note = type.Note;
            }
            await _context.SaveChangesAsync();
        }
    }
}

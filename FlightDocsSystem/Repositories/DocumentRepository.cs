using System.Collections.Generic;

namespace FlightDocsSystem.Services
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DataContext _context;

        public DocumentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> CreateDocumentAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
            int id = document.DocumentId;
            return id;
        }

        public async Task CreateDocumentHistoryAsync(DocumentHistory documentHistory)
        {
            await _context.DocumentHistories.AddAsync(documentHistory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDocumentAsync(int id)
        {
            var document = await _context.Documents.SingleOrDefaultAsync(d => d.DocumentId == id);
            if (document != null)
            {
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<Document>> GetAllDocumentsAsync()
        {
            var documents = await _context.Documents.ToListAsync();
            return documents;
        }

        public async Task<Document?> GetDocumentByIdAsync(int id)
        {
            var document = await _context.Documents.SingleOrDefaultAsync(d => d.DocumentId == id);
            if (document != null)
            {
                return document;
            }
            return null;
        }

        public async Task UpdateDocumentAsync(Document document)
        {
            var newDocument = await _context.Documents.SingleOrDefaultAsync(d => d.DocumentId == document.DocumentId);
            _context.Documents.Update(newDocument);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Document>> GetRecentDocumentsAsync(int row)
        {
            var documents = _context.Documents.OrderByDescending(x => x.DocumentId).Take(row);
            var result = await documents.ToListAsync();
            return result;
        }
    }
}

using FlightDocsSystem.Models.EditModel;

namespace FlightDocsSystem.Repositories
{
    public interface IDocumentTypeRepository
    {

        ICollection<DocumentType>? GetAllDocumentTypes(string? search, DateTime? from, DateTime? to, int pageSize = 10, int page = 1);
        Task<DocumentType?> GetDocumentTypeByIdAsync(int id);
        Task<int> CreateDocumentTypeAsync(DocumentType type);
        Task UpdateDocumentTypeAsync(DocumentType type);
        Task DeleteDocumentTypeAsync(int id);

    }
}

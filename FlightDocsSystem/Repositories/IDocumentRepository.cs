namespace FlightDocsSystem.Services
{
    public interface IDocumentRepository
    {
        Task<ICollection<Document>> GetAllDocumentsAsync();
        Task<Document?> GetDocumentByIdAsync(int id);

        Task<int> CreateDocumentAsync(Document document);
        Task CreateDocumentHistoryAsync(DocumentHistory documentHistory);

        Task UpdateDocumentAsync(Document document);

        Task DeleteDocumentAsync(int id);

        Task<ICollection<Document>> GetRecentDocumentsAsync(int row);

    }
}

using FlightDocsSystem.Helpers;
using FlightDocsSystem.Models.EditModel;
using FlightDocsSystem.Models.Model;
using FlightDocsSystem.Models.ViewModel;
using FlightDocsSystem.Repositories;
using FlightDocsSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static FlightDocsSystem.Models.ViewModel.DocumentVM;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly ITypeGroupRepository _typeGroupRepository;
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(IDocumentRepository documentRepository, ITypeGroupRepository typeGroupRepository, ILogger<DocumentController> logger)
        {
            _documentRepository = documentRepository;
            _typeGroupRepository = typeGroupRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            try
            {
                var data = await _documentRepository.GetAllDocumentsAsync();
                var result = data.Select(x =>
                {
                    var latestDocument = x.DocumentHistories.OrderByDescending(o => o.CreateDate).FirstOrDefault()!;

                    DbDocumentVM dbDocumentVM = new DbDocumentVM()
                    {
                        DocumentId = x.DocumentId,
                        DocumentName = x.DocumentName,
                        DocumentType = x.DocumentType.Name,
                        FlightNo = x.Flight.FlightNo,
                        DepartureDate = x.Flight.DepartureDate,
                        Creator = latestDocument.Creator.Name,
                        UpdatedDate = latestDocument.CreateDate
                    };
                    return dbDocumentVM;
                }
                );
                 _logger.LogInformation("Get all documents successfully");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when getting all documents: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when getting all documents");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            try
            {
                var data = await _documentRepository.GetDocumentByIdAsync(id);
                if (data != null)
                {
                    var latestDocument = data.DocumentHistories.OrderByDescending(x => x.CreateDate).FirstOrDefault()!;

                    DocumentVM result = new DocumentVM()
                    {
                        DocumentId = data.DocumentId,
                        File = latestDocument.File,
                        DocumentName = data.DocumentName,
                        DocumentType = data.DocumentType.Name,
                        CreatedDate = latestDocument.CreateDate,
                        LatestVersion = data.LatestVersion,
                        CreatorName = latestDocument.Creator.Name,
                        Note = data.Note,
                        FlightId = data.Flight.FlightId,
                        FlightNo = data.Flight.FlightNo,
                        Route = data.Flight.Route,
                        DepartureDate = data.Flight.DepartureDate,
                        DocumentHistoryVMs = data.DocumentHistories.Select(
                            x => new DocumentHistoryVM()
                            {
                                DocumentHistoryId = x.DocumentHistoryId,
                                File = x.File,
                                Version = x.Version,
                                CreateDate = x.CreateDate
                            }).ToList(),
                    };
                    _logger.LogInformation($"Get document with Id: {id}");
                    return Ok(result);
                }
                else
                {

                    return NotFound($"Document with Id {id} does not exist.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when getting document with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when getting document with id {id}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromForm] DocumentModel model)
        {
            try
            {
                string filePathAndName = string.Empty;
                string fileName = string.Empty;
                string filePath = "FileStorage\\Documents\\";
                if (model.File.Length > 0)
                {
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    // su dung guid de ten document ko bi trung
                    fileName = "Document_" + Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
                    using (FileStream fileStream = System.IO.File.Create(filePath + fileName))
                    {
                        model.File.CopyTo(fileStream);
                        fileStream.Flush();
                        filePathAndName = filePath + fileName;
                    }
                }
                else
                {
                    return BadRequest("File has not been uploaded");
                }


                Document document = new Document()
                {
                    DocumentName = fileName,
                    DocumentTypeId = model.DocumentTypeId,
                    LatestVersion = "1.0",
                    Note = model.Note,
                    FlightId = model.FlightId,
                };
                int documentId = await _documentRepository.CreateDocumentAsync(document);

                DocumentHistory history = new DocumentHistory()
                {
                    File = filePathAndName,
                    Version = "1.0",
                    CreateDate = DateTime.UtcNow,
                    DocumentId = documentId,
                    CreatorId = model.CreatorId,

                };

                await _documentRepository.CreateDocumentHistoryAsync(history);

                _logger.LogInformation($"Create document with Id {documentId} successfully");
                return Ok("Create document successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when creating new document: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when creating new document");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDocument([FromForm] DocumentEM model)
        {
            try
            {
                Document? document = await _documentRepository.GetDocumentByIdAsync(model.DocumentId);
                if (document != null)
                {
                    string filePathAndName = string.Empty;
                    string fileName = string.Empty;
                    string filePath = "FileStorage\\Documents\\";
                    string newVersion = Utils.GetNewVersion(document.LatestVersion);

                    if (model.File.Length > 0)
                    {
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        fileName = "Document_" + Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
                        using (FileStream fileStream = System.IO.File.Create(filePath + fileName))
                        {
                            model.File.CopyTo(fileStream);
                            fileStream.Flush();
                            filePathAndName = filePath + fileName;
                        }
                    }
                    else
                    {
                        return BadRequest("File has not been uploaded");
                    }


                    document.DocumentName = fileName;
                    document.LatestVersion = newVersion;

                    await _documentRepository.UpdateDocumentAsync(document);

                    DocumentHistory history = new DocumentHistory()
                    {
                        File = filePathAndName,
                        Version = newVersion,
                        CreateDate = DateTime.UtcNow,
                        DocumentId = model.DocumentId,
                        CreatorId = model.CreatorId,

                    };

                    await _documentRepository.CreateDocumentHistoryAsync(history);

                    _logger.LogInformation($"Update document with Id {model.DocumentId} successfully");
                    return Ok("Update document successfully");
                }
                else
                {
                    return BadRequest("Document does not exist");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when updating document with id {model.DocumentId}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when updating document with id {model.DocumentId}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDocument(int id)
        {
            try
            {
                await _documentRepository.DeleteDocumentAsync(id);
                _logger.LogInformation($"Delete document with Id {id} successfully");
                return Ok("Delete document successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when deleting document with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when deleting document with id {id}");
            }
        }
    }
}

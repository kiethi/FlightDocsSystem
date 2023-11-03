using FlightDocsSystem.Models.EditModel;
using FlightDocsSystem.Models.Model;
using FlightDocsSystem.Models.ViewModel;
using FlightDocsSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly ITypeGroupRepository _typeGroupRepository;
        private readonly ILogger<DocumentTypeController> _logger;

        public DocumentTypeController(IDocumentTypeRepository documentTypeRepository, ITypeGroupRepository typeGroupRepository, ILogger<DocumentTypeController> logger)
        {
            _documentTypeRepository = documentTypeRepository;
            _typeGroupRepository = typeGroupRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllDocumentTypes(string? search, DateTime? from, DateTime? to, int pageSize = 10, int page = 1)
        {
            try
            {
                var data = _documentTypeRepository.GetAllDocumentTypes(search, from, to, pageSize, page);
                if (data != null) {
                    var result = data.Select(x => new DocumentTypeVM
                    {
                        DocumentTypeId = x.DocumentTypeId,
                        Name = x.Name,
                        CreatedDate = x.CreatedDate,
                        CreatorName = x.Creator.Name,
                        NoPermission = x.TypeGroups?.Count() ?? 0
                    });

                    _logger.LogInformation("Get all document types successfully");
                    return Ok(result);
                } else
                {
                    return NotFound("Cannot found any document types");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when getting document types: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when getting document types");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocumentType(DocumentTypeModel model)
        {
            try
            {
                DocumentType newType = new DocumentType()
                {
                    Name = model.Name,
                    CreatedDate = DateTime.UtcNow,
                    Note = model.Note,
                    CreatorId = model.CreatorId
                };

                int documentTypeId = await _documentTypeRepository.CreateDocumentTypeAsync(newType);

                if (model.GroupIds != null) { 
                    foreach (var groupId in model.GroupIds)
                    {
                        await _typeGroupRepository.AddGroupToTypeAsync(new TypeGroup()
                        {
                            GroupId = groupId,
                            DocumentTypeId = documentTypeId
                        });
                    }
                }

                _logger.LogInformation("create document type successfully");
                return Ok("Create document type successfully");
            } catch (Exception ex)
            {
                return BadRequest("Error when creating document type");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocumentType(int id, DocumentTypeEM type)
        {
            try
            {
                DocumentType newType = new DocumentType() { 
                    DocumentTypeId = id,
                    Name = type.Name,
                    Note = type.Note,
                };
                await _documentTypeRepository.UpdateDocumentTypeAsync(newType);
                _logger.LogInformation($"Update document type with id {id} successfully");
                return Ok($"Update document type with id {id} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when updating document type with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when updating document type with id {id}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDocumentType(int id)
        {
            try
            {
                await _documentTypeRepository.DeleteDocumentTypeAsync(id);
                return Ok("Delete document type successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when deleting document type with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when deleting document type");
            }
        }


    }
}

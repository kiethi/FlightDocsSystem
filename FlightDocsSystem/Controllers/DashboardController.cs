using FlightDocsSystem.Models.ViewModel;
using FlightDocsSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IDocumentRepository documentRepository, IFlightRepository flightRepository, ILogger<DashboardController> logger)
        {
            _documentRepository = documentRepository;
            _flightRepository = flightRepository;
            _logger = logger;
        }

        [HttpGet("RecentDocument/{row}")]
        public async Task<IActionResult> GetRecentDocuments(int row)
        {
            try
            {
                var data = await _documentRepository.GetRecentDocumentsAsync(row);
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
                _logger.LogInformation("Get recent " + row + " documents successfully");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error when getting recent documents: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when getting recent documents");
            }
        }

        [HttpGet("CurrentFlight")]
        public async Task<IActionResult> GetCurrentFlights()
        {
            try
            {
                var data = await _flightRepository.GetCurrentFlightsAsync();
                var result = data.Select(x => new DbFlightVM()
                {
                    FlightId = x.FlightId,
                    FlightNo = x.FlightNo,
                    Route = x.Route,
                    DepartureDate = x.DepartureDate
                });
                _logger.LogInformation("Get current flights successfully");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error when getting recent flights: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when getting recent flights");
            }
        }


    }
}

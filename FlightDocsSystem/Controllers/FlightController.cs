using AutoMapper;
using FlightDocsSystem.Data;
using FlightDocsSystem.Helpers;
using FlightDocsSystem.Models.EditModel;
using FlightDocsSystem.Models.Model;
using FlightDocsSystem.Models.ViewModel;
using FlightDocsSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class FlightController : ControllerBase
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FlightController> _logger;

        public FlightController(IFlightRepository flightRepository, IUserRepository userRepository, IMapper mapper, ILogger<FlightController> logger)
        {
            _flightRepository = flightRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFlights()
        {
            try
            {
                var flights = await _flightRepository.GetAllFlightsAsync();
                if (flights == null) return NotFound();
                var flightVMs = flights.Select(x => _mapper.Map<FlightVM>(x));

                _logger.LogInformation("Get all flights successfully");
                return Ok(flightVMs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when getting all flights: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when getting all flights");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlightById(int id)
        {
            try
            {
                var flight = await _flightRepository.GetFlightByIdAsync(id);
                if (flight == null) return NotFound();
                var flightVM = _mapper.Map<FlightVM>(flight);

                _logger.LogInformation($"Get flight with id {id} successfully");
                return Ok(flightVM);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when getting flight by id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when getting flight by id");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlight(FlightModel request)
        {
            try
            {
                if (request.FlightNo.IsNullOrEmpty())
                {
                    return BadRequest("Flight No must not be empty");
                }

                if (request.Loading.IsNullOrEmpty())
                {
                    return BadRequest("Point of Loading must not be empty");
                }

                if (request.Unloading.IsNullOrEmpty())
                {
                    return BadRequest("Point of Unloading must not be empty");
                }

                User? creator;
                try
                {
                    creator = await _userRepository.GetUserByIdAsync(request.CreatorId);
                }
                catch
                {
                    return BadRequest("Creator not exist");
                }

                var flight = new Flight
                {
                    FlightNo = request.FlightNo,
                    Route = Utils.TwoPointToRoute(request.Loading, request.Unloading),
                    DepartureDate = request.DepartureDate,
                    CreatorId = request.CreatorId
                };
                await _flightRepository.CreateFlightAsync(flight);

                _logger.LogInformation("Create new flight successfully");
                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when creating new flight: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when creating new flight");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlight(int id, FlightEM request)
        {

            try
            {
                if (request.FlightNo.IsNullOrEmpty())
                {
                    return BadRequest("Flight No must not be empty");
                }

                if (request.Loading.IsNullOrEmpty())
                {
                    return BadRequest("Point of Loading must not be empty");
                }

                if (request.Unloading.IsNullOrEmpty())
                {
                    return BadRequest("Point of Unloading must not be empty");
                }

                var flight = await _flightRepository.GetFlightByIdAsync(id);
                if (flight == null)
                {
                    return BadRequest("ID of flight not exist");
                }

                var newFlight = new Flight
                {
                    FlightId = id,
                    FlightNo = request.FlightNo,
                    Route = Utils.TwoPointToRoute(request.Loading, request.Unloading),
                    DepartureDate = request.DepartureDate
                };

                await _flightRepository.UpdateFlightAsync(newFlight);
                _logger.LogInformation($"Update flight with id {id} successfully");
                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when updating flight with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when updating flight with id {id}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            try
            {
                await _flightRepository.DeleteFlightAsync(id);

                _logger.LogInformation($"Delete flight with id {id} successfully");
                return Ok("Delete flight successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when deleting flight with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when deleting flight with id {id}");
            }
        }
    }
}

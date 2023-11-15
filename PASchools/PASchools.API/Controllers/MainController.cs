using Microsoft.AspNetCore.Mvc;
using PASchools.API.Models;
using PASchools.Application.DTOs;
using PASchools.Application.Interfaces;
using PASchools.Google.Connector.Models.Requests;

namespace PASchools.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ISchoolService _schoolService;
        public MainController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }
        [HttpGet]
        [Route("UpdateSchoolDatabase")]
        public async Task<IActionResult> Get(int rowsLimit)
        {
            try
            {
                var count = await _schoolService.UpdateSchoolDatabase(rowsLimit);
                return Ok($"{count} rows updated/added");
            }
            catch (Exception e)
            {
                //log
                return this.StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
        [HttpGet]
        [Route("SchoolsOrdered")]
        public async Task<IActionResult> GetSchoolsOrderByAddress(double lat, double lng)
        {
            try
            {
                List<SchoolDTO> addresses = await _schoolService.FindSchoolsByAddressOrderByDistance(new Coordinate() { Latitude = lat, Longitude = lng});
                if (addresses != null)
                    return Ok(addresses);

                return BadRequest("N�o foi poss�vel realizar a consulta no momento. Tente novamente em breve.");
            }
            catch (Exception e)
            {
                //log
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno na aplica��o." + e.Message);
            }
        }

        [HttpPost]
        [Route("Route")]
        public async Task<IActionResult> GetRouteByCoordinates(RouteRequest request)
        {
            try
            {
                RouteDTO route = await _schoolService.GenerateRoute(request.Origin, request.Destination);
                if (route != null)
                    return Ok(route);

                return BadRequest("N�o foi poss�vel realizar a consulta da rota no momento. Tente novamente em breve.");
            }
            catch (Exception e)
            {
                //log
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno na aplica��o." + e.Message);
            }
        }

        [HttpPost]
        [Route("Coordinates")]
        public async Task<IActionResult> GetCoordinates([FromBody] AddressDTO addressDTO)
        {
            try
            {
                Coordinate coordinates = await _schoolService.GetCoordinatesAsync(addressDTO);
                if (coordinates != null)
                    return Ok(coordinates);

                return BadRequest("N�o foi poss�vel realizar a consulta no momento. Tente novamente em breve.");
            }
            catch (Exception e)
            {
                //log
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno na aplica��o." + e.Message);
            }
        }
    }
}
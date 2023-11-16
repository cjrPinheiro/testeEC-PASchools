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
                var total = await _schoolService.UpdateSchoolDatabase(rowsLimit);

                return Ok(new
                {
                    total
                });
            }
            catch (Exception e)
            {
                //log
                return this.StatusCode(StatusCodes.Status500InternalServerError, e);
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

                return BadRequest("Não foi possível realizar a consulta no momento. Tente novamente em breve.");
            }
            catch (Exception e)
            {
                //log
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno na aplicação." + e.Message);
            }
        }
        [HttpGet]
        [Route("SchoolsOrdered")]
        public async Task<IActionResult> GetSchoolsOrderByAddress(double lat, double lng)
        {
            try
            {
                List<SchoolDTO> list = await _schoolService.FindSchoolsByAddressOrderByDistance(new Coordinate() { Lat = lat, Lng = lng });
                if (list != null)
                    return Ok(list);

                return BadRequest("Não foi possível realizar a consulta no momento. Tente novamente em breve.");
            }
            catch (Exception e)
            {
                //log
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno na aplicação." + e.Message);
            }
        }
        [HttpGet]
        [Route("PagedSchoolsOrdered")]
        public async Task<IActionResult> GetPagedSchoolsOrderByAddress(double lat, double lng, short pageIndex, short pageSize)
        {
            try
            {
                PagedList<SchoolDTO> list = await _schoolService.FindPagedSchoolsByAddressOrderByDistance(new Coordinate() { Lat = lat, Lng = lng }, pageIndex, pageSize);
                if (list != null)
                    return Ok(list);

                return BadRequest("Não foi possível realizar a consulta no momento. Tente novamente em breve.");
            }
            catch (Exception e)
            {
                //log
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno na aplicação." + e.Message);
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

                return BadRequest("Não foi possível realizar a consulta da rota no momento. Tente novamente em breve.");
            }
            catch (Exception e)
            {
                //log
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno na aplicação." + e.Message);
            }
        }
    }
}
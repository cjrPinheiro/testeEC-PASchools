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
        public async Task<IActionResult> Get()
        {
            try
            {
                await _schoolService.UpdateSchoolDatabase();
                return Ok("Sucess");
            }
            catch (Exception e)
            {
                //log
                return this.StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
        [HttpPost]
        [Route("Schools")]
        public async Task<IActionResult> GetSchoolsOrderByAddress([FromBody] AddressDTO addressDTO)
        {
            try
            {
                List<SchoolDTO> addresses = await _schoolService.FindSchoolsByAddressOrderByDistance(addressDTO);
                if (addresses != null)
                    return Ok(addresses);

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
        public async Task<IActionResult> GetRouteByAddress(RouteRequest request)
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
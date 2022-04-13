using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Results;
using Abp.AspNetCore.Mvc.Results.Wrapping;
using Abp.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvManagement.Controllers;
using MvManagement.Extensions.Dto.PageFilter;
using MvManagement.Models.Generic;
using MvManagement.Vehicles;
using MvManagement.Vehicles.Dto;

namespace MvManagement.Web.Host.Controllers
{
    [ApiController]
    [AbpAuthorize]
    [Route("api/[controller]/[action]")]
    public class VehicleController : MvManagementControllerBase
    {
        private readonly IVehicleManagementAppService _vehicleManagementAppService;

        public VehicleController(IVehicleManagementAppService vehicleManagementAppService)
        {
            _vehicleManagementAppService = vehicleManagementAppService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<long>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> CreateNewVehicleAsync(VehicleCreateDto input)
        {
            try
            {
                var vehicleId = await _vehicleManagementAppService.CreateVehicleAsync(input);
                return Ok(vehicleId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<PagedResultDto<VehicleDto>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetCurrentUserPersonalVehiclesAsync([FromQuery]PagedSortedAndFilteredInputDto input)
        {
            try
            {
                var vehicleId = await _vehicleManagementAppService.GetCurrentUserPersonalVehiclesAsync(input);
                return Ok(vehicleId);
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<PagedResultDto<VehicleDto>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetTenantVehiclesAsync([FromQuery]PagedSortedAndFilteredInputDto input)
        {
            try
            {
                var vehicleId = await _vehicleManagementAppService.GetTenantVehiclesAsync(input);
                return Ok(vehicleId);
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> UpdateVehicleAsync(VehicleDto input)
        {
            try
            {
                await _vehicleManagementAppService.UpdateVehicleAsync(input);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> DeleteVehicleAsync(long idVehicle)
        {
            try
            {
                await _vehicleManagementAppService.DeleteVehicleAsync(idVehicle);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> DeleteRefillAsync([FromBody] EntityDto<long> input)
        {
            try
            {
                await _vehicleManagementAppService.DeleteVehicleAsync(input.Id);
                return Ok();
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

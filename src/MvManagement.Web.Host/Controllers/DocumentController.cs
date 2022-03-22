using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvManagement.Controllers;
using MvManagement.Documents.Insurance;
using MvManagement.Documents.Insurance.Dto;
using MvManagement.Extensions.Dto.PageFilter;
using MvManagement.Models.Generic;
using MvManagement.Vehicles.Dto;

namespace MvManagement.Web.Host.Controllers
{
    [ApiController]
    [AbpAuthorize]
    [Route("api/[controller]/[action]")]
    public class DocumentController : MvManagementControllerBase
    {
        private IInsuranceAppService _insuranceAppService;

        public DocumentController(IInsuranceAppService insuranceAppService)
        {
            _insuranceAppService = insuranceAppService;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<long>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> SaveInsuranceAsync(InsuranceDocumentDto input)
        {
            try
            {
                await _insuranceAppService.SaveInsuranceAsync(input);
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<PagedResultDto<InsuranceDocumentDto>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetCurrentUserPersonalVehiclesAsync([FromQuery] VehiclesPagedResultRequestDto input)
        {
            try
            {
                var documents = await _insuranceAppService.GetIsurancesForVehicleAsync(input);
                return Ok(documents);
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
using System;
using System.Drawing.Printing;
using System.Security.Authentication;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Castle.Logging.Log4Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvManagement.Controllers;
using MvManagement.Documents.Insurance;
using MvManagement.Documents.Insurance.Dto;
using MvManagement.Documents.UserDocuments;
using MvManagement.Documents.UserDocuments.Dto;
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
        private readonly IInsuranceAppService _insuranceAppService;
        private readonly IUserDocumentAppService _userDocumentAppService;

        public DocumentController(IInsuranceAppService insuranceAppService, IUserDocumentAppService userDocumentAppService)
        {
            _insuranceAppService = insuranceAppService;
            _userDocumentAppService = userDocumentAppService;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<InsuranceResultDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetInsuranceStatusAsync([FromQuery] long idVehicle)
        {
            try
            {
                var documents = await _insuranceAppService.GetInsurancesForVehicleAsync(idVehicle);
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<InsuranceIdsResultDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetInsuranceIdsAsync([FromQuery] long idVehicle)
        {
            try
            {
                var documents = await _insuranceAppService.GetInsuranceIdsForVehicleAsync(idVehicle);
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> DeleteInsuranceAsync([FromBody] EntityDto<long> input)
        {
            try
            {
                await _insuranceAppService.DeleteInsurance(input.Id);
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<long>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> UpdateInsuranceAsync(InsuranceDocumentDto input)
        {
            try
            {
                await _insuranceAppService.EditInsuranceAsync(input);
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<long>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> SaveUserDocumentAsync(UserDocumentInputDto input)
        {
            try
            {
                var documentId = await _userDocumentAppService.AddUserDocumentAsync(input);
                return Ok(documentId);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<long>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> UpdateUserDocumentAsync(UserDocumentDto input)
        {
            try
            {
                await _userDocumentAppService.UpdateUserDocumentAsync(input);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<ListResultDto<UserDocumentDto>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetUserDocumentsAsync()
        {
            try
            {
                var documents = await _userDocumentAppService.GetCurrentUserDocumentsAsync();
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AbpActionResultWrapper<UserDocumentDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetUserDocumentAsync(long idDocument)
        {   
            try
            {
                var document = await _userDocumentAppService.GetUserDocumentAsync(idDocument);
                return Ok(document);
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
        public async Task<IActionResult> DeleteUserDocumentAsync([FromBody] EntityDto<long> input)
        {
            try
            {
                await _userDocumentAppService.DeleteUserDocumentAsync(input.Id);
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
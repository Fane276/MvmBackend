using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Catalogue.Documents;
using Microsoft.EntityFrameworkCore;
using MvManagement.Documents.Dto;
using MvManagement.Documents.PeriodicalDocuments.Dto;
using MvManagement.Extensions;
using MvManagement.VehicleData;
using MvManagement.Vehicles;

namespace MvManagement.Documents.PeriodicalDocuments
{
    [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
    public class PeriodicalDocumentAppService : VehicleAppServiceBase, IPeriodicalDocumentAppService
    {
        private readonly IRepository<PeriodicalDocument, long> _periodicalDocumentRepository;
        private readonly IRepository<Vehicle, long> _vehicleRepository;
        private readonly IRepository<PeriodicalDocumentType> _periodicalDocumentTypeRepository;
        public PeriodicalDocumentAppService(
            IVehiclePermissionManager vehiclePermissionManager, 
            IRepository<PeriodicalDocument, long> periodicalDocumentRepository, 
            IRepository<PeriodicalDocumentType> periodicalDocumentTypeRepository, IRepository<Vehicle, long> vehicleRepository) : base(vehiclePermissionManager)
        {
            _periodicalDocumentRepository = periodicalDocumentRepository;
            _periodicalDocumentTypeRepository = periodicalDocumentTypeRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<ListResultDto<PeriodicalDocumentDto>> GetPeriodicalDocumentsAsync(long idVehicle)
        {
            var hasPermission = await VehiclePermissionManager.CheckCurrentUserPermissionAsync(idVehicle, VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View);

            if (!hasPermission)
            {
                throw new UserFriendlyException($"Not authorized, {VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View} is missing");
            }

            var listOfDocuments = await (from document in _periodicalDocumentRepository.GetAll()
                where document.IdVehicle == idVehicle
                join documentType in _periodicalDocumentTypeRepository.GetAll() on document.IdPeriodicalDocumentType equals documentType.Id into documentTypeObj
                from documentTypeOrDefault in documentTypeObj.DefaultIfEmpty()
                select new PeriodicalDocumentDto()
                {
                    Id = document.Id,
                    IdVehicle = document.IdVehicle,
                    IdPeriodicalDocumentType = document.IdPeriodicalDocumentType,
                    PeriodicalDocumentType = documentTypeOrDefault.Name,
                    ValidFrom = document.ValidFrom,
                    ValidTo = document.ValidTo
                })
                .ToListAsync();

            return new ListResultDto<PeriodicalDocumentDto>(listOfDocuments);
        }

        public async Task<long> AddPeriodicalDocumentAsync(PeriodicalDocumentInput input)
        {
            var hasPermission = await VehiclePermissionManager.CheckCurrentUserPermissionAsync(input.IdVehicle, VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit);

            if (!hasPermission)
            {
                throw new UserFriendlyException($"Not authorized, {VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit} is missing");
            }

            var entity = ObjectMapper.Map<PeriodicalDocument>(input);

            var idDocument = await _periodicalDocumentRepository.InsertAndGetIdAsync(entity);

            return idDocument;
        }

        public async Task DeletePeriodicalDocumentAsync(DeletePeriodicalDocumentInput input)
        {
            var hasPermission = await VehiclePermissionManager.CheckCurrentUserPermissionAsync(input.IdVehicle, VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit);

            if (!hasPermission)
            {
                throw new UserFriendlyException($"Not authorized, {VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit} is missing");
            }

            await _periodicalDocumentRepository.DeleteAsync(d => d.Id == input.IdDocument);
        }
        public async Task UpdatePeriodicalDocumentAsync(PeriodicalDocumentDto input)
        {
            var hasPermission = await VehiclePermissionManager.CheckCurrentUserPermissionAsync(input.IdVehicle, VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit);

            if (!hasPermission)
            {
                throw new UserFriendlyException($"Not authorized, {VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit} is missing");
            }

            var entity = ObjectMapper.Map<PeriodicalDocument>(input);

            await _periodicalDocumentRepository.UpdateAsync(entity);
        }

        public async Task<PeriodicalDocumentDto> GetPeriodicalDocumentAsync(long idDocument)
        {
            var foundDocument = await (from document in _periodicalDocumentRepository.GetAll()
                    where document.Id == idDocument
                    join documentType in _periodicalDocumentTypeRepository.GetAll() on document.IdPeriodicalDocumentType equals documentType.Id into documentTypeObj
                    from documentTypeOrDefault in documentTypeObj.DefaultIfEmpty()
                    select new PeriodicalDocumentDto()
                    {
                        Id = document.Id,
                        IdVehicle = document.IdVehicle,
                        IdPeriodicalDocumentType = document.IdPeriodicalDocumentType,
                        PeriodicalDocumentType = documentTypeOrDefault.Name,
                        ValidFrom = document.ValidFrom,
                        ValidTo = document.ValidTo
                    })
                .FirstOrDefaultAsync();

            return foundDocument;
        }

        public async Task<List<ExpiredDocumentDto>> GetExpiredPeriodicalDocumentsAllUserVehiclesAsync()
        {
            var currentUserId = AbpSession.UserId;

            if (currentUserId == null)
            {
                throw new AbpAuthorizationException("User not authenticated");
            }

            var vehcicleIds = await _vehicleRepository.GetAll()
                .Where(vehicle => vehicle.UserId == currentUserId)
                .Select(vehicle => vehicle.Id).ToListAsync();

            var listOfDocuments = await(from document in _periodicalDocumentRepository.GetAll()
                where vehcicleIds.Contains(document.IdVehicle) && document.ValidTo.CompareTo(DateTime.Today) < 0
                join vehicle in _vehicleRepository.GetAll() on document.IdVehicle equals vehicle.Id
                join documentType in _periodicalDocumentTypeRepository.GetAll() on document.IdPeriodicalDocumentType equals documentType.Id
                select new ExpiredDocumentDto
                {
                    Id = document.Id,
                    Name = documentType.Name,
                    VehicleTitle = vehicle.Title,
                    ValidTo = document.ValidTo,
                    DocumentType = DocumentType.Periodical
                })
                .ToListAsync();

            return listOfDocuments;
        }
    }
}
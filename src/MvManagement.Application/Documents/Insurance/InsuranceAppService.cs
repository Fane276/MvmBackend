using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using MvManagement.Documents.Insurance.Dto;
using MvManagement.Extensions.Dto.PageResult;
using MvManagement.VehicleData;
using MvManagement.Vehicles;
using MvManagement.Vehicles.Dto;

namespace MvManagement.Documents.Insurance
{
    [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
    public class InsuranceAppService : VehicleAppServiceBase, IInsuranceAppService
    {
        private readonly IRepository<InsuranceDocument, long> _insuranceDocumentRepository;

        public InsuranceAppService(
            IVehiclePermissionManager vehiclePermissionManager, 
            IRepository<InsuranceDocument, long> insuranceDocumentRepository) : base(vehiclePermissionManager)
        {
            _insuranceDocumentRepository = insuranceDocumentRepository;
        }
        public async Task SaveInsuranceAsync(InsuranceDocumentDto document)
        {
            var hasPermission = await VehiclePermissionManager.CheckCurrentUserPermissionAsync(document.IdVehicle, VehiclePermissionNames.VehicleDocuments.Insurance.Edit);

            if (!hasPermission)
            {
                throw new UserFriendlyException($"Not authorized, {VehiclePermissionNames.VehicleDocuments.Insurance.Edit} is missing");
            }

            var insurance = await _insuranceDocumentRepository.GetAll()
                .FirstOrDefaultAsync(d => d.IdVehicle == document.IdVehicle && d.InsuranceType == document.InsuranceType);
            if (insurance != null)
            {
                throw new UserFriendlyException($"Already has an insurance of type {document.InsuranceType}");
            }

            var entity = ObjectMapper.Map<InsuranceDocument>(document);

            await _insuranceDocumentRepository.InsertAsync(entity);
        }

        public async Task<PagedResultDto<InsuranceDocumentDto>> GetIsurancesForVehicleAsync(VehiclesPagedResultRequestDto input)
        {
            var hasPermission = await VehiclePermissionManager.CheckCurrentUserPermissionAsync(input.IdVehicle, VehiclePermissionNames.VehicleDocuments.Insurance.View);

            if (!hasPermission)
            {
                throw new UserFriendlyException($"Not authorized, {VehiclePermissionNames.VehicleDocuments.Insurance.View} is missing");
            }

            var insuranceList = await _insuranceDocumentRepository.GetAll()
                .Where(d => d.IdVehicle == input.IdVehicle)
                .Select(d => ObjectMapper.Map<InsuranceDocumentDto>(d))
                .ToListAsync();

            return new PagedResultEnumerableDto<InsuranceDocumentDto>(insuranceList, input).Get();
        }
    }
}
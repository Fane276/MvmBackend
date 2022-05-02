using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MvManagement.Chart;
using MvManagement.Extensions.Dto.PageResult;
using MvManagement.FuelManagement.Dto;
using MvManagement.VehicleData;
using MvManagement.VehicleData.VehicleAccess;
using MvManagement.Vehicles;
using MvManagement.Vehicles.Dto;

namespace MvManagement.FuelManagement
{
    [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
    public class FuelManagementAppService : VehicleAppServiceBase, IFuelManagementAppService
    {
        private readonly IRepository<FuelRefill, long> _fuelRefillRepository;
        private readonly IRepository<Vehicle, long> _vehicleRepository;
        private readonly IRepository<VehicleRoleUser, long> _vehicleRoleUserRepository;
        private readonly IRepository<VehiclePermission, long> _vehiclePermissionRepository;

        public FuelManagementAppService(
            IVehiclePermissionManager vehiclePermissionManager, 
            IRepository<FuelRefill, long> fuelRefillRepository, 
            IRepository<Vehicle, long> vehicleRepository, IRepository<VehicleRoleUser, long> vehicleRoleUserRepository, IRepository<VehiclePermission, long> vehiclePermissionRepository) : base(vehiclePermissionManager)
        {
            _fuelRefillRepository = fuelRefillRepository;
            _vehicleRepository = vehicleRepository;
            _vehicleRoleUserRepository = vehicleRoleUserRepository;
            _vehiclePermissionRepository = vehiclePermissionRepository;
        }

        public async Task<PagedResultDto<FuelRefillDto>> GetVehicleRefillsAsync(VehiclesPagedResultRequestDto input)
        {
            var currentUserId = AbpSession.UserId;

            if (currentUserId == null)
            {
                throw new AbpAuthorizationException("User not authenticated");
            }

            if (AbpSession.TenantId == null)
            {
                throw new AbpException("Tenant not specified");
            }

            var refills = await _fuelRefillRepository.GetAll()
                .Where(r => r.IdVehicle == input.IdVehicle)
                .Select(r=>ObjectMapper.Map<FuelRefillDto>(r))
                .ToListAsync();

            return new PagedResultEnumerableDto<FuelRefillDto>(refills, input).Get();
        }
        public async Task<long> InsertRefillAsync(InputRefillDto input)
        {
            var entity = ObjectMapper.Map<FuelRefill>(input);

            var result = await _fuelRefillRepository.InsertAndGetIdAsync(entity);

            return result;
        }

        public async Task DeleteRefillAsync(long idRefill)
        {
            await _fuelRefillRepository.DeleteAsync(x => x.Id == idRefill);
        }

        private async Task<List<long>> GetUserVehicleIdsAsync(int vehiclesNumber)
        {
            var currentUserId = AbpSession.UserId;

            if (currentUserId == null)
            {
                throw new AbpAuthorizationException("User not authenticated");
            }

            if (AbpSession.TenantId == null)
            {
                throw new AbpException("Tenant not specified");
            }

            var idsVehiclesUserHasAccess = await _vehicleRoleUserRepository.GetAll()
                .Join(
                    _vehiclePermissionRepository.GetAll(),
                    ur => ur.IdRole,
                    p => p.IdRole,
                    (ur, p) => new { userRole = ur, vehiclePermission = p }
                )
                .Where(o => (o.userRole.UserId == currentUserId || o.vehiclePermission.UserId == currentUserId) &&
                            o.vehiclePermission.Name.Equals(VehiclePermissionNames.VehicleInfo.View))
                .Select(o => o.vehiclePermission.IdVehicle)
                .ToListAsync();

            var listOfVehicleIds = await (from vehicle in _vehicleRepository.GetAll()
                    where vehicle.UserId == currentUserId && !idsVehiclesUserHasAccess.Contains(vehicle.Id)
                    select  vehicle.Id)
                .ToListAsync();
            if (listOfVehicleIds.Count > vehiclesNumber)
            {
                return listOfVehicleIds.GetRange(0, vehiclesNumber);
            }
            return listOfVehicleIds;
        }

        public async Task<ChartResult> GetPricePerLastWeekAsync(long idVehicle, int numberDays = 7)
        {
            var data = new ChartResult();

            var startDate = DateTime.Today.Subtract(TimeSpan.FromDays(numberDays));
            var dates = new List<DateTime>();
            for (int i = 1; i <= numberDays; i++)
            {
                dates.Add(startDate.AddDays(i));
            }


            var queryResult = await _fuelRefillRepository.GetAll()
                .Where(f => f.IdVehicle == idVehicle && f.RefillDate.CompareTo(startDate)>=0 && f.RefillDate.CompareTo(DateTime.Now) <= 0) 
                .Select(f=>new {date = new DateTime(f.RefillDate.Year, f.RefillDate.Month, f.RefillDate.Day), value = f.Price })
                .GroupBy(f=>f.date)
                .Select(f=>new
                {
                    Date = f.Key,
                    Value = f.Sum(r=>r.value)
                })
                .ToListAsync();

            foreach (var date in dates)
            {
                var item = queryResult.FirstOrDefault(x => x.Date.Equals(date));
                if (item != null)
                {
                    data.AddItem(new ChartItem() { Label = item.Date.ToString("dd.MM.yyyy"), Value = item.Value });
                }
                else
                {
                    data.AddItem(new ChartItem() { Label = date.ToString("dd.MM.yyyy"), Value = 0 });
                }
            }

            return data;
        }

        public async Task<ChartResult> GetCostPerVehicleAsync(int maxNumberVehicle = 10)
        {
            var data = new ChartResult();

            var userVehicles = await GetUserVehicleIdsAsync(maxNumberVehicle);
            var refills = await _fuelRefillRepository.GetAll()
                .Where(f=>userVehicles.Contains(f.IdVehicle))
                .GroupBy(x => x.IdVehicle)
                .Select(f => new
                {
                    IdVehicle = f.Key,
                    Value = f.Sum(r => r.Price)
                })
                .ToListAsync();

            foreach (var refill in refills)
            {
                var vehicle = await _vehicleRepository.FirstOrDefaultAsync(x => x.Id == refill.IdVehicle);
                data.AddItem(new ChartItem()
                {
                    Label = vehicle.Title,
                    Value = refill.Value
                });
            }

            return data;
        }
    }
}
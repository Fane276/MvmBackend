using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using MvManagement.Chart;
using MvManagement.Extensions.Dto.PageResult;
using MvManagement.FuelManagement.Dto;
using MvManagement.VehicleData;
using MvManagement.Vehicles;
using MvManagement.Vehicles.Dto;

namespace MvManagement.FuelManagement
{
    [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
    public class FuelManagementAppService : VehicleAppServiceBase, IFuelManagementAppService
    {
        private readonly IRepository<FuelRefill, long> _fuelRefillRepository;

        public FuelManagementAppService(
            IVehiclePermissionManager vehiclePermissionManager, 
            IRepository<FuelRefill, long> fuelRefillRepository
            ) : base(vehiclePermissionManager)
        {
            _fuelRefillRepository = fuelRefillRepository;
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

        public async Task<BarChart> GetPricePerLastWeekAsync(long idVehicle, int numberDays = 7)
        {
            var data = new BarChart();

            var startDate = DateTime.Today.Subtract(TimeSpan.FromDays(numberDays));
            var dates = new List<DateTime>();
            for (int i = 1; i <= numberDays; i++)
            {
                dates.Add(startDate.AddDays(i));
            }


            var queryResult = await _fuelRefillRepository.GetAll()
                .Where(f => f.IdVehicle == idVehicle && f.CreationTime.CompareTo(startDate)>=0 && f.CreationTime.CompareTo(DateTime.Now) <= 0) 
                .Select(f=>new {date = new DateTime(f.CreationTime.Year, f.CreationTime.Month, f.CreationTime.Day), value = f.Price })
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
                    data.AddItem(new BarChartItem() { Label = item.Date.ToString("dd.MM.yyyy"), Value = item.Value });
                }
                else
                {
                    data.AddItem(new BarChartItem() { Label = date.ToString("dd.MM.yyyy"), Value = 0 });
                }
            }

            return data;
        }
    }
}
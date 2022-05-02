using System;
using Abp.Application.Services.Dto;

namespace MvManagement.FuelManagement.Dto
{
    public class FuelRefillDto : EntityDto<long>
    {
        public FuelType FuelType { get; set; }
        public double FuelAmount { get; set; }
        public FuelUnit FuelUnit { get; set; }
        public double Price { get; set; }
        public long IdVehicle { get; set; }
        public DateTime RefillDate { get; set; }
    }
}
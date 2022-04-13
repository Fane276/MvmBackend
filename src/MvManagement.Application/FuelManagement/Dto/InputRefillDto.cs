using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using MvManagement.VehicleData;

namespace MvManagement.FuelManagement.Dto
{
    public class InputRefillDto
    {
        public FuelType FuelType { get; set; }
        public double FuelAmount { get; set; }
        public FuelUnit FuelUnit { get; set; }
        public double Price { get; set; }
        public long IdVehicle { get; set; }
    }
}
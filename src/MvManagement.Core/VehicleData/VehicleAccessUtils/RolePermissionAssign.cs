﻿using Abp.Application.Services.Dto;

namespace MvManagement.VehicleData.VehicleAccessUtils
{
    public class RolePermissionAssign : EntityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TenantId { get; set; }
        public int IdRole { get; set; }
    }
}
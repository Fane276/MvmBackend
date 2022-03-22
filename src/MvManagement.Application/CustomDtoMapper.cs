﻿using Abp.Auditing;
using Abp.EntityHistory;
using Abp.Organizations;
using AutoMapper;
using MvManagement.Authorization.Roles;
using MvManagement.Documents.Insurance;
using MvManagement.Documents.Insurance.Dto;
using MvManagement.Roles.Dto;
using MvManagement.VehicleData;
using MvManagement.Vehicles.Dto;

namespace MvManagement
{
    public class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Vehicle, VehicleDto>();
            configuration.CreateMap<VehicleDto, Vehicle>();

            configuration.CreateMap<InsuranceDocument, InsuranceDocumentDto>();
            configuration.CreateMap<InsuranceDocumentDto, InsuranceDocument>();
        }
    }
}
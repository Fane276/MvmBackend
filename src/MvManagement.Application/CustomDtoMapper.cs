﻿using Abp.Auditing;
using Abp.EntityHistory;
using Abp.Organizations;
using AutoMapper;
using MvManagement.Authorization.Roles;
using MvManagement.Authorization.Users;
using MvManagement.Documents.Insurance;
using MvManagement.Documents.Insurance.Dto;
using MvManagement.Documents.PeriodicalDocuments;
using MvManagement.Documents.PeriodicalDocuments.Dto;
using MvManagement.Documents.UserDocuments;
using MvManagement.Documents.UserDocuments.Dto;
using MvManagement.FuelManagement;
using MvManagement.FuelManagement.Dto;
using MvManagement.Roles.Dto;
using MvManagement.Users.Dto;
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
            configuration.CreateMap<VehicleCreateDto, Vehicle>();

            configuration.CreateMap<InsuranceDocument, InsuranceDocumentDto>();
            configuration.CreateMap<InsuranceDocumentDto, InsuranceDocument>();
            configuration.CreateMap<UserDocumentDto, UserDocument>();
            configuration.CreateMap<UserDocumentInputDto, UserDocument>();
            configuration.CreateMap<UserDocument, UserDocumentDto>();
            configuration.CreateMap<PeriodicalDocumentInput, PeriodicalDocument>();
            configuration.CreateMap<PeriodicalDocumentDto, PeriodicalDocument>();

            configuration.CreateMap<InputRefillDto, FuelRefill>();
            configuration.CreateMap<FuelRefill, FuelRefillDto>();
            configuration.CreateMap<FuelRefillDto, FuelRefill>();

            configuration.CreateMap<User, UserDto>();
        }
    }
}
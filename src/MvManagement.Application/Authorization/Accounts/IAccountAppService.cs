﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MvManagement.Authorization.Accounts.Dto;
using MvManagement.Roles.Dto;
using MvManagement.Users.Dto;

namespace MvManagement.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
        Task<ListResultDto<PermissionDto>> GetCurrentUserPermissionsAsync();
        Task<UserDto> GetCurrentUserInfoAsync();
        Task UserUpdateAsync(UpdateUserInput input);
    }
}

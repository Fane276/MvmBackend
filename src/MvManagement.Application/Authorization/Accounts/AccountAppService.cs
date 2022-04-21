using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Zero.Configuration;
using MvManagement.Authorization.Accounts.Dto;
using MvManagement.Authorization.Users;
using MvManagement.Roles.Dto;
using MvManagement.Users.Dto;

namespace MvManagement.Authorization.Accounts
{
    public class AccountAppService : MvManagementAppServiceBase, IAccountAppService
    {
        // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
        public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";

        private readonly UserRegistrationManager _userRegistrationManager;

        public AccountAppService(
            UserRegistrationManager userRegistrationManager)
        {
            _userRegistrationManager = userRegistrationManager;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
            );

            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }

        public async Task<ListResultDto<PermissionDto>> GetCurrentUserPermissionsAsync()
        {
            var userId = AbpSession.UserId;
            if (userId == null)
            {
                return new ListResultDto<PermissionDto>();
            }

            var user =await UserManager.GetUserAsync(new UserIdentifier(AbpSession.TenantId,(long)userId));

            var permissions = await UserManager.GetGrantedPermissionsAsync(user);


            return await Task.FromResult(new ListResultDto<PermissionDto>(
                ObjectMapper.Map<List<PermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList()
            ));
        }

        public async Task<UserDto> GetCurrentUserInfoAsync()
        {
            var userId = AbpSession.UserId;
            if (userId == null)
            {
                throw new AbpAuthorizationException("Not logged in");
            }

            var currentUser = await UserManager.GetUserAsync(new UserIdentifier(AbpSession.TenantId, (long) userId));

            return ObjectMapper.Map<UserDto>(currentUser);

        }

        public async Task UserUpdateAsync(UpdateUserInput input)
        {
            var userId = AbpSession.UserId;
            if (userId == null)
            {
                throw new AbpAuthorizationException("Not logged in");
            }

            if (userId != input.Id)
            {
                throw new AbpAuthorizationException("Not authorized!");
            }

            var currentUser = await UserManager.GetUserAsync(new UserIdentifier(AbpSession.TenantId, (long)userId));

            currentUser.Name = input.Name;
            currentUser.Surname = input.Surname;
            currentUser.EmailAddress = input.EmailAddress;
            currentUser.UserName = input.UserName;

            await UserManager.UpdateAsync(currentUser);
        }
    }
}

using Abp.Authorization;
using MvManagement.Authorization.Roles;
using MvManagement.Authorization.Users;

namespace MvManagement.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}

using Abp.Authorization;
using MvMangement.Authorization.Roles;
using MvMangement.Authorization.Users;

namespace MvMangement.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}

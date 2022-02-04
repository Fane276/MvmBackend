using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace MvManagement.Controllers
{
    public abstract class MvManagementControllerBase: AbpController
    {
        protected MvManagementControllerBase()
        {
            LocalizationSourceName = MvManagementConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}

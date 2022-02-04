using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace MvMangement.Controllers
{
    public abstract class MvMangementControllerBase: AbpController
    {
        protected MvMangementControllerBase()
        {
            LocalizationSourceName = MvMangementConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}

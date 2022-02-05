using Abp.AspNetCore.Mvc.ViewComponents;

namespace MvManagement.Web.Views
{
    public abstract class MvManagementViewComponent : AbpViewComponent
    {
        protected MvManagementViewComponent()
        {
            LocalizationSourceName = MvManagementConsts.LocalizationSourceName;
        }
    }
}

using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace MvManagement.Web.Views
{
    public abstract class MvManagementRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected MvManagementRazorPage()
        {
            LocalizationSourceName = MvManagementConsts.LocalizationSourceName;
        }
    }
}

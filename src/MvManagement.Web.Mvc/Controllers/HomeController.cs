using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using MvManagement.Controllers;

namespace MvManagement.Web.Controllers
{
    // [AbpMvcAuthorize]
    public class HomeController : MvManagementControllerBase
    {
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}

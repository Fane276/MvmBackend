using System.Threading.Tasks;
using MvManagement.Models.TokenAuth;
using MvManagement.Web.Controllers;
using Shouldly;
using Xunit;

namespace MvManagement.Web.Tests.Controllers
{
    public class HomeController_Tests: MvManagementWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}
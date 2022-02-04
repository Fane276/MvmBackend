using System.Threading.Tasks;
using MvMangement.Models.TokenAuth;
using MvMangement.Web.Controllers;
using Shouldly;
using Xunit;

namespace MvMangement.Web.Tests.Controllers
{
    public class HomeController_Tests: MvMangementWebTestBase
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
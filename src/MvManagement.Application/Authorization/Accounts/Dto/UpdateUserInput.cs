using Abp.Application.Services.Dto;

namespace MvManagement.Authorization.Accounts.Dto
{
    public class UpdateUserInput : EntityDto<long>
    {
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
    }
}
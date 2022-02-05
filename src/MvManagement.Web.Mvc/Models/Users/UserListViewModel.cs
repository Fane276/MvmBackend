using System.Collections.Generic;
using MvManagement.Roles.Dto;

namespace MvManagement.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}

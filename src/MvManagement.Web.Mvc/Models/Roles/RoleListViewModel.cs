using System.Collections.Generic;
using MvManagement.Roles.Dto;

namespace MvManagement.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}

using System.Collections.Generic;
using MvManagement.Roles.Dto;

namespace MvManagement.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}
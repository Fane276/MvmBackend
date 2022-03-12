using System.Collections.Generic;
using System.Threading.Tasks;
using MvManagement.VehicleData.VehicleAccess;
using MvManagement.VehicleData.VehicleAccessUtils;

namespace MvManagement.VehicleData
{
    public interface IVehiclePermissionManager
    {
        /// <summary>
        /// Method <c>CheckCurrentUserPermissionAsync</c> checks if current user have specified permission
        /// </summary>
        /// <exception cref="AuthenticationException">Thrown when user not logged in</exception>
        Task<bool> CheckCurrentUserPermissionAsync(long vehicleId, string vehiclePermission);

        /// <summary>
        /// Method <c>CheckPermissionAsync</c> checks if user have specified permission
        /// </summary>
        /// <exception cref="AuthenticationException">Thrown when user not logged in</exception>
        Task<bool> CheckPermissionAsync(long userId,long vehicleId, string vehiclePermission);
        /// <summary>
        /// Method <c>AsignPermissionAndGetIdAsync</c> assign a permission to a role or to a user for a specific vehicle
        /// </summary>
        /// <param name="input"> If is not null the permission will be applied to userId</param>
        ///
        /// <returns>The Id of the inserted permission</returns>
        /// 
        /// <exception cref="AbpException">Thrown when permission does not exist</exception>
        /// <exception cref="AbpException">Thrown when user already have this permission</exception>
        /// <exception cref="AbpException">Thrown when both UserId and IdRole are null</exception>
        Task<long> AsignPermissionAndGetIdAsync(PermissionAssign input);
        /// <summary>
        /// Method <c>CreateRoleAndGetIdAsync</c> create a new role with given permissions.
        /// </summary>
        ///
        /// <exception cref="AbpException">Thrown when one of the give permission is not defined</exception>
        /// <returns></returns>
        Task<int> CreateRoleAndGetIdAsync(VehicleRole vehicleRole, long idVehicle, List<string> vehicleRolePermissions);
        /// <summary>
        /// Method <c>DeletePermissionFromRoleAsync</c> delete permission from selected role on specified vehicle
        /// </summary>
        Task DeletePermissionFromRoleAsync(int idRole, long idVehicle, string permissionName);
        /// <summary>
        /// Method <c>DeletePermissionFromUserAsync</c> delete permission from selected user on specified vehicle
        /// </summary>
        Task DeletePermissionFromUserAsync(int idUser, long idVehicle, string permissionName);
        /// <summary>
        /// Method <c>GetCurrentUserPermissions</c> retrieves all the permissions for currentUser both on role and individual
        /// </summary>
        /// <param name="idVehicle">id of the vehicle on which permission are assigned</param>
        /// <returns></returns>
        Task<IEnumerable<VehiclePermission>> GetCurrentUserPermissions(long idVehicle);
        /// <summary>
        /// Method <c>GetRolePermissions</c> retrieves all the permissions for specified role
        /// </summary>
        /// <param name="idRole">id of the role to be checked</param>
        /// <param name="idVehicle">id of the vehicle on which permission are assigned</param>
        /// <returns></returns>
        Task<IEnumerable<VehiclePermission>> GetRolePermissions(int idRole, long idVehicle);
    }
}
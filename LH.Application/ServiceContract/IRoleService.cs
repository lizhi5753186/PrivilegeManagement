using LH.Application.Dtos;

namespace LH.Application.ServiceContract
{
    public interface IRoleService
    {
        GetResults<RoleDto> GetRoles(RoleInput input);

        CreateResult<int> CreateRole(RoleDto role);

        UpdateResult UpdateRole(RoleDto role);

        DeleteResult DeleteRole(int roleId);
    }
}
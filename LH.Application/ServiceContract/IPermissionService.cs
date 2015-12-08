using LH.Application.Dtos;

namespace LH.Application.ServiceContract
{
    public interface IPermissionService
    {
        GetResults<BaseEntityDto> GetPermissions(PageInput input);

        UpdateResult UpdatePermission(BaseEntityDto action);

        CreateResult<int> CreatePermission(BaseEntityDto action);

        DeleteResult DeletePermission(int actionId);
    }
}
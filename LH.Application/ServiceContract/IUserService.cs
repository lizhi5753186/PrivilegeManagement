using System.Security.Policy;
using LH.Application.Dtos;

namespace LH.Application.ServiceContract
{
    public interface IUserService
    {
        GetResults<UserDto> GetUsers(PageInput input);

        UpdateResult UpdateUser(UserDto user);

        CreateResult<int> AddUser(UserDto user);

        DeleteResult DeleteUser(int userId);

        UpdateResult UpdatePwd(UserDto user);

        GetResult<UserDto> GetUser(int userId);

        UpdateResult UpdateRoles(UserDto user);

        DeleteResult DeleteRole(int userId, int roleId);
    }
}
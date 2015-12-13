using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LH.Application.Dtos;
using LH.Application.ServiceContract;

namespace LH.Web.API.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("api/user/GetUsers")]
        public OutputBase GetUsers([FromUri]PageInput input)
        {
            return _userService.GetUsers(input);
        }

        [HttpGet]
        [Route("api/user/UserInfo")]
        public OutputBase GetUserInfo(int id)
        {
            return _userService.GetUser(id);
        }

        [HttpPost]
        [Route("api/user/AddUser")]
        public OutputBase CreateUser([FromBody] UserDto userDto)
        {
            return _userService.AddUser(userDto);
        }

        [HttpPost]
        [Route("api/user/UpdateUser")]
        public OutputBase UpdateUser([FromBody] UserDto userDto)
        {
            return _userService.UpdateUser(userDto);
        }

        [HttpPost]
        [Route("api/user/UpdateRoles")]
        public OutputBase UpdateRoles([FromBody] UserDto userDto)
        {
            return _userService.UpdateRoles(userDto);
        }

        [HttpPost]
        [Route("api/user/DeleteUser/{id}")]
        public OutputBase DeleteUser(int id)
        {
            return _userService.DeleteUser(id);
        }

        [HttpPost]
        [Route("api/user/DeleteRole/{id}/{roleId}")]
        public OutputBase DeleteRole(int id, int roleId)
        {
            return _userService.DeleteRole(id, roleId);
        }
    }
}

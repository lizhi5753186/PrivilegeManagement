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
        public readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ActionName("GetUsers")]
        public OutputBase GetUsers([FromUri] PageInput input)
        {
            return _userService.GetUsers(input);
        }

        [HttpGet]
        [ActionName("GetUser")]
        public OutputBase GetUser(int id)
        {
            return _userService.GetUser(id);
        }

        [HttpPost]
        [ActionName("AddUser")]
        public OutputBase CreateUser([FromBody] UserDto userDto)
        {
            return _userService.AddUser(userDto);
        }

        [HttpPost]
        [ActionName("UpdateUser")]
        public OutputBase UpdateUser([FromBody] UserDto userDto)
        {
            return _userService.UpdateUser(userDto);
        }

        [HttpPost]
        [ActionName("UpdateRoles")]
        public OutputBase UpdateRoles([FromBody] UserDto userDto)
        {
            return _userService.UpdateRoles(userDto);
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        public OutputBase DeleteUser(int id)
        {
            return _userService.DeleteUser(id);
        }

        [HttpPost]
        [ActionName("DeleteRole")]
        public OutputBase DeleteRole(int id, int roleId)
        {
            return _userService.DeleteRole(id, roleId);
        }
    }
}

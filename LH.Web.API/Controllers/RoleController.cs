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
    public class RoleController : ApiController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("api/role/GetRoles")]
        public OutputBase GetRoles([FromUri]RoleInput input)
        {
            return _roleService.GetRoles(input);
        }

        [HttpPost]
        [ActionName("AddRole")]
        [Route("api/role/AddRole")]
        public OutputBase CreateRole([FromBody]RoleDto roleDto)
        {
            return _roleService.CreateRole(roleDto);
        }

        [HttpPost]
        [Route("api/role/UpdateRole")]
        public OutputBase UpdateRole([FromBody]RoleDto roleDto)
        {
            return _roleService.UpdateRole(roleDto);
        }

        [HttpPost]
        [Route("api/role/DeleteRole/{id}")]
        public OutputBase DeleteRole(int id)
        {
            return _roleService.DeleteRole(id);
        }
    }
}

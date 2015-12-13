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
    public class PermissionController : ApiController
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        [Route("api/permission/GetPermissions")]
        public OutputBase GetPermissions([FromUri] PageInput pageInput)
        {
            return _permissionService.GetPermissions(pageInput);
        }

        [HttpPost]
        [Route("api/permission/UpdatePermission")]
        public OutputBase UpdatePermission([FromBody] BaseEntityDto pDto)
        {
            return _permissionService.UpdatePermission(pDto);
        }

        [HttpPost]
        [Route("api/permission/AddPermission")]
        public OutputBase CreatePermission([FromBody] BaseEntityDto pDto)
        {
            return _permissionService.CreatePermission(pDto);
        }

        [HttpPost]
        [Route("api/permission/RemovePermission/{id}")]
        public OutputBase DeletePermission(int id)
        {
            return _permissionService.DeletePermission(id);
        }
    }
}

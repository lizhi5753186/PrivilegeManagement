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
        [ActionName("GetPermissions")]
        public OutputBase GetPermission([FromUri] PageInput pageInput)
        {
            return _permissionService.GetPermissions(pageInput);
        }

        [HttpPost]
        [ActionName("UpdatePermission")]
        public OutputBase UpdatePermission([FromBody] BaseEntityDto pDto)
        {
            return _permissionService.UpdatePermission(pDto);
        }

        [HttpPost]
        [ActionName("AddPermission")]
        public OutputBase CreatePermission([FromBody] BaseEntityDto pDto)
        {
            return _permissionService.CreatePermission(pDto);
        }

        [HttpPost]
        [ActionName("RemovePermission")]
        public OutputBase DeletePermission(int id)
        {
            return _permissionService.DeletePermission(id);
        }
    }
}

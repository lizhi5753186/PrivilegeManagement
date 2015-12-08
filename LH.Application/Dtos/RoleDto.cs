using System.Collections.Generic;

namespace LH.Application.Dtos
{
    public class RoleDto : BaseEntityDto
    {
        public List<BaseEntityDto> Permissions { get; set; } 
    }
}
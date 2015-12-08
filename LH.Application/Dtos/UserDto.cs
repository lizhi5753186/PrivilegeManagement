using System;
using System.Collections.Generic;

namespace LH.Application.Dtos
{
    public class UserDto :BaseEntityDto
    {
        public string RealName { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
        public string Email { get; set; }
        public int State { get; set; }

        public List<BaseEntityDto> Roles { get; set; }

        public int TotalRole { get; set; }
    }
}
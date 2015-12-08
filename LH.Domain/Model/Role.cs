using System;
using System.Collections;
using System.Collections.Generic;
using LH.Domain.Auditing;

namespace LH.Domain.Model
{
    public class Role : BaseEntity, ICreationAudited
    {
        public string RoleName { get; set; }

        public int? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public Role()
        {
            RolePermissions = new HashSet<RolePermission>();
            UserRoles = new HashSet<UserRole>();
        }
    }
}
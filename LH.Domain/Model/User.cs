using System;
using System.Collections.Generic;
using LH.Domain.Auditing;

namespace LH.Domain.Model
{
    public class User : BaseEntity, ICreationAudited
    {
       
        public string Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string RealName { get; set; }

        public int? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public int State { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public User()
        {
            this.UserRoles = new HashSet<UserRole>();
        }

    }
}
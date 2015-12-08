using System;
using LH.Domain.Interface;

namespace LH.Domain.Auditing
{
    public interface IDeletionAudited : ISoftDelete
    {
        /// <summary>
        /// 删除实体的用户ID
        /// </summary>
        int? DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeletionTime { get; set; } 
    }
}
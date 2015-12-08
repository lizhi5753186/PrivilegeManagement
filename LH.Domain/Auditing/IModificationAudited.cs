using System;

namespace LH.Domain.Auditing
{
    public interface IModificationAudited
    {
        /// <summary>
        /// 最后修改实体时间
        /// </summary>
        DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 最后修改实体的用户ID
        /// </summary>
        int? LastModifierUserId { get; set; }
    }
}
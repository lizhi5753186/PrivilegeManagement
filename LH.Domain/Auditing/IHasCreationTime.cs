using System;

namespace LH.Domain.Auditing
{
    public interface IHasCreationTime
    {
        /// <summary>
        /// 创建实体时间
        /// </summary>
        DateTime CreationTime { get; set; } 
    }
}
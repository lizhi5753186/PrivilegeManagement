namespace LH.Domain.Auditing
{
    public interface ICreationAudited : IHasCreationTime
    {
        /// <summary>
        /// 创建实体的用户ID
        /// </summary>
        int? CreatorUserId { get; } 
    }
}
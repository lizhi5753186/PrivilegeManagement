using LH.Domain.Interface;

namespace LH.Domain
{
    public class BaseEntity : IEntity
    {
        // 用户ID
        public int Id { get; set; }
    }
}
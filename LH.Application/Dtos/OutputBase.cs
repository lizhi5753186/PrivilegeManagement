namespace LH.Application.Dtos
{
    public class OutputBase
    {
        /// <summary>
        /// 错误或者其他信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int StateCode { get; set; }
    }
}
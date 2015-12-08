using System.Collections.Generic;

namespace LH.Application.Dtos
{
    public class GetResults<T> : OutputBase
    {
        public int Total { get; set; }

        public List<T> Data { get; set; }
    }
}
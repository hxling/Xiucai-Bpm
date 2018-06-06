using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xiucai.Common.Data
{
    public interface IPageableInfo
    {
        int PageCount { get; set; }
        int PageIndex { get; set; }
    }

    public interface IPageable<T> : IPageableInfo
    {
        IEnumerable<T> Rows { get; set; }
    }

    public class Pageable<T> : IPageable<T>
    {
        public int PageCount { get; set; }

        public IEnumerable<T> Rows { get; set; }

        public int PageIndex { get; set; }
    }
}

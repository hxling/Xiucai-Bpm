using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.Common.Data;
using Xiucai.BPM.Core.Model;
using Xiucai.Common.Provider;
namespace Xiucai.BPM.Core.Dal
{
    public class DepartmentDal:BaseRepository<Department>
    {
        public static DepartmentDal Instance
        {
            get { return SingletonProvider<DepartmentDal>.Instance; }
        }

        public IEnumerable<Department> GetChildren(int parentid=0)
        {
            return GetAll().Where(d => d.ParentId == parentid);
        }
    }
}

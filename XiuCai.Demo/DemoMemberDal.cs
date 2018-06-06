using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Xiucai.Common;
using Xiucai.Common.Data;
using Xiucai.Common.Data.Filter;
using Xiucai.Common.Provider;

namespace XiuCai.Demo
{
    public class DemoMemberDal :BaseRepository<DemoMember>
    {
        public static DemoMemberDal Instance
        {
            get { return SingletonProvider<DemoMemberDal>.Instance; }
        }

       
    }
}

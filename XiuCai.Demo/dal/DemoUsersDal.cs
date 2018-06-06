using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xiucai.Common.Data;
using Xiucai.Common.Provider;

using Xiucai.Demo.Model;

namespace Xiucai.Demo.Dal
{
    public class DemoUsersDal : BaseRepository<DemoUsersModel>
    {
        public static DemoUsersDal Instance
        {
            get { return SingletonProvider<DemoUsersDal>.Instance; }
        }

        public string GetJson(int pageindex, int pagesize, string filterJson, string sort = "keyid",
                              string order = "asc")
        {
            return base.JsonDataForEasyUIdataGrid(TableConvention.Resolve(typeof(DemoUsersModel)), pageindex, pagesize, filterJson,
                                                  sort, order);
        }
    }
}
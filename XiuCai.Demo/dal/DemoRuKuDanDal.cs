using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xiucai.Common.Data;
using Xiucai.Common.Provider;

using Xiucai.Model;

namespace Xiucai.Dal
{
    public class DemoRuKuDanDal : BaseRepository<DemoRuKuDanModel>
    {
        public static DemoRuKuDanDal Instance
        {
            get { return SingletonProvider<DemoRuKuDanDal>.Instance; }
        }

        public string GetJson(int pageindex, int pagesize, string filterJson, string sort = "keyid",
                              string order = "asc")
        {
            return base.JsonDataForEasyUIdataGrid(TableConvention.Resolve(typeof(DemoRuKuDanModel)), pageindex, pagesize, filterJson,
                                                  sort, order);
        }
    }
}
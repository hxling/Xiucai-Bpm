using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xiucai.Common.Data;
using Xiucai.Common.Provider;

using Xiucai.Model;

namespace Xiucai.Dal
{
    public class DemoRuKuDanMingXiDal : BaseRepository<DemoRuKuDanMingXiModel>
    {
        public static DemoRuKuDanMingXiDal Instance
        {
            get { return SingletonProvider<DemoRuKuDanMingXiDal>.Instance; }
        }

        public string GetJson(int pageindex, int pagesize, string filterJson, string sort = "keyid",
                              string order = "asc")
        {
            return base.JsonDataForEasyUIdataGrid("V_DemoRuKuMingXi", pageindex, pagesize, filterJson,
                                                  sort, order);
        }
    }
}
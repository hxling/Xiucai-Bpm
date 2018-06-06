using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xiucai.Common.Data;
using Xiucai.Common.Provider;
using Xiucai.Demo.Model;
using Xiucai.Model;

namespace Xiucai.Demo.Dal
{
    public class DemoArticleDal : BaseRepository<DemoArticleModel>
    {
        public static DemoArticleDal Instance
        {
            get { return SingletonProvider<DemoArticleDal>.Instance; }
        }

        public string GetJson(int pageindex, int pagesize, string filterJson, string sort = "keyid",
                              string order = "asc")
        {
            return base.JsonDataForEasyUIdataGrid(TableConvention.Resolve(typeof(DemoArticleModel)), pageindex, pagesize, filterJson,
                                                  sort, order);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.Dal;
using Xiucai.Demo.Dal;
using Xiucai.Demo.Model;
using Xiucai.Model;
using Xiucai.Common.Provider;

namespace Xiucai.Demo.Bll
{
    public class DemoArticleBll
    {
        public static DemoArticleBll Instance
        {
            get { return SingletonProvider<DemoArticleBll>.Instance; }
        }

        public int Add(DemoArticleModel model)
        {
            return DemoArticleDal.Instance.Insert(model);
        }

        public int Update(DemoArticleModel model)
        {
            return DemoArticleDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return DemoArticleDal.Instance.Delete(keyid);
        }

        public string GetJson(int pageindex, int pagesize, string filterJson, string sort = "Keyid", string order = "asc")
        {
            return DemoArticleDal.Instance.GetJson(pageindex, pagesize, filterJson, sort, order);
        }
    }
}

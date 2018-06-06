using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.Demo.Dal;
using Xiucai.Demo.Model;
using Xiucai.Common.Provider;

namespace Xiucai.Demo.Bll
{
    public class DemoUsersBll
    {
        public static DemoUsersBll Instance
        {
            get { return SingletonProvider<DemoUsersBll>.Instance; }
        }

        public int Add(DemoUsersModel model)
        {
            return DemoUsersDal.Instance.Insert(model);
        }

        public int Update(DemoUsersModel model)
        {
            return DemoUsersDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return DemoUsersDal.Instance.Delete(keyid);
        }

        public string GetJson(int pageindex, int pagesize, string filterJson, string sort = "Keyid", string order = "asc")
        {
            return DemoUsersDal.Instance.GetJson(pageindex, pagesize, filterJson, sort, order);
        }
    }
}

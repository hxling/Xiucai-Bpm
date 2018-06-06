using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.Dal;
using Xiucai.Model;
using Xiucai.Common.Provider;

namespace Xiucai.Bll
{
    public class DemoRuKuDanBll
    {
        public static DemoRuKuDanBll Instance
        {
            get { return SingletonProvider<DemoRuKuDanBll>.Instance; }
        }

        public int Add(DemoRuKuDanModel model)
        {
            int rkdid = DemoRuKuDanDal.Instance.Insert(model);
            if (rkdid > 0)
            {
                //添加入库明细
                
                //循环添加明细

                if (model.products.Count > 0)
                {
                    foreach (var mx in model.products)
                    {
                        mx.rkdId = rkdid;
                        DemoRuKuDanMingXiBll.Instance.Add(mx);
                    }
                }

            }

            return rkdid;
        }

        public int Update(DemoRuKuDanModel model)
        {
            int rkdid = model.KeyId;

            DemoRuKuDanMingXiBll.Instance.deleteAll(rkdid);

            if (model.products.Count > 0)
            {
                foreach (var mx in model.products)
                {
                    mx.rkdId = rkdid;
                    DemoRuKuDanMingXiBll.Instance.Add(mx);
                }
            }


            return DemoRuKuDanDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return DemoRuKuDanDal.Instance.Delete(keyid);
        }

        public string GetJson(int pageindex, int pagesize, string filterJson, string sort = "Keyid", string order = "asc")
        {
            return DemoRuKuDanDal.Instance.GetJson(pageindex, pagesize, filterJson, sort, order);
        }
    }
}

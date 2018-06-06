using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.BPM.Core.Dal;
using Xiucai.BPM.Core.Model;
using Xiucai.Common;
using Xiucai.Common.Provider;

namespace Xiucai.BPM.Core.Bll
{
    public class DicBll
    {
        public static DicBll Instance
        {
            get { return SingletonProvider<DicBll>.Instance; } 
        }

        public string DicCategoryJson()
        {
            return JSONhelper.ToJson(DicCategoryDal.Instance.GetAll().ToList().OrderBy(n => n.Sortnum)
                                    .Select(n => new
                                     {
                                         id = n.KeyId, 
                                         text = n.Title +" ["+n.Code+"]", 
                                         iconCls = "icon-bullet_green", 
                                         attributes = new { n.Sortnum,n.Remark ,n.Code}
                                     })
                                 );
        }

        public Dic Get(string code)
        {
            return DicDal.Instance.Get(code);
        }

        public Dic Get(int keyid)
        {
            return DicDal.Instance.Get(keyid);
        }



        /// <summary>
        /// 根据类别编码获取字典列表
        /// </summary>
        /// <param name="categoryCode">类别编码</param>
        /// <returns></returns>
        public List<Dic> GetListBy(string categoryCode)
        {
             
            var cat = DicCategoryDal.Instance.GetWhere(new {Code = categoryCode});
            if(cat.Any())
                return DicDal.Instance.GetWhere(new { CategoryId = cat.First().KeyId }).ToList();
            else
            {
                return new List<Dic>();
            }

        }

        /// <summary>
        /// 根据类别编码获取字典列表 JSON
        /// </summary>
        /// <param name="catCode">字典类别编码</param>
        /// <returns></returns>
        public string GetDicListBy(string catCode)
        {
            var list = GetListBy(catCode);
            return JSONhelper.ToJson(list);
        }

        /// <summary>
        /// 根据类别ID 获取字典列表JSON
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns></returns>
        public string GetDicListBy(int categoryId)
        {
            var dicList = DicDal.Instance.GetListBy(categoryId);
            var list = dicList as List<Dic> ?? dicList.ToList();
            return list.Any() ? JSONhelper.ToJson(list.FindAll(n => n.ParentId == 0)) : "[]";
        }

        private bool HasDicCode(string dicCode,int dicid=0)
        {
            List<Dic> list = DicDal.Instance.GetAll().ToList();
            return list.Any(n => n.Code == dicCode && n.KeyId!=dicid);
        }

        public int Add(Dic d)
        {
            if(HasDicCode(d.Code))
            {
                return 0; //字典编码已存在
            }

            int i= DicDal.Instance.Insert(d);
            if (i > 0)
            {
                if (d.IsDefault == 1)
                {
                    DicDal.Instance.UpdateDefaultState(i);
                }

                LogBll<Dic> log = new LogBll<Dic>();
                d.KeyId = i;
                log.AddLog(d);
            }
            return i;
        }

        public int Edit(Dic d)
        {
            if (HasDicCode(d.Code,d.KeyId))
            {
                return 0; //字典编码已存在
            }

            Dic oldDic = DicDal.Instance.Get(d.KeyId);


            int i = DicDal.Instance.Update(d);
            if(i >0)
            {
                if (d.IsDefault == 1)
                {
                    DicDal.Instance.UpdateDefaultState(d.KeyId);
                }

                LogBll<Dic> log = new LogBll<Dic>();
                log.UpdateLog(oldDic,d);
            }
            return i;
        }

        public int Delete(int dicid)
        {
            Dic d = DicDal.Instance.Get(dicid);
            if(d!=null)
            {
                if(d.children.Any())
                {
                    return 2; //有子节点不能删除
                }
                
                int i =  DicDal.Instance.Delete(dicid);
                if(i >0)
                {
                    LogBll<Dic> log = new LogBll<Dic>();
                    log.DeleteLog(d);
                }
                return i;
            }
            return 0; //参数错误
        }
    }
}

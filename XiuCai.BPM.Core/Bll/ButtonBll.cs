using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.BPM.Core.Dal;
using Xiucai.BPM.Core.Model;
using Xiucai.Common.Provider;
using Xiucai.Common.Data;

namespace Xiucai.BPM.Core.Bll
{
    public class ButtonBll
    {
        public static ButtonBll Instance
        {
            get { return SingletonProvider<ButtonBll>.Instance; }
        }

        /// <summary>
        /// 判断按钮是否存在
        /// </summary>
        /// <param name="title">按钮名称</param>
        /// <param name="code">编码</param>
        /// <param name="keyid">按钮ID</param>
        /// <returns></returns>
        private bool HasButton(Button b)
        {
            var btns = ButtonDal.Instance.GetAll();

            var enumerable = btns as Button[] ?? btns.ToArray();
            return enumerable.Any(n => (n.ButtonText == b.ButtonText || n.ButtonTag == b.ButtonTag) && n.KeyId != b.KeyId);
        }

        public string AddButton(Button b)
        {
            if(HasButton(b))
                return new JsonMessage { Success = false, Data = "0", Message = "按钮名称或编码已存存！" }.ToString();

            int k = ButtonDal.Instance.Insert(b);
            var msg = "添加成功。";
            if (k <= 0)
                msg = "添加失败。";
            else
            {
                LogBll<Button> log = new LogBll<Button>();
                b.KeyId = k;
                log.AddLog(b);
            }
            return new JsonMessage {Success = true, Data = k.ToString(), Message = msg}.ToString();
        }

        public string EditButton(Button b)
        {
            if (HasButton(b))
                return new JsonMessage { Success = false, Data = "0", Message = "按钮名称或编码已存存！" }.ToString();

            var oldBtn = ButtonDal.Instance.Get(b.KeyId);
            int k = ButtonDal.Instance.Update(b);
            var msg = "修改成功。";
            if (k <= 0)
                msg = "修改失败。";
            else
            {
                LogBll<Button> log = new LogBll<Button>();
                log.UpdateLog(oldBtn, b);
            }
            return new JsonMessage { Success = true, Data = k.ToString(), Message = msg }.ToString();
        }

        public string DelButton(int btnId)
        {
            var btn = ButtonDal.Instance.Get(btnId);
            int k = ButtonDal.Instance.Delete(btnId);

            var msg = "删除成功。";
            if (k <= 0)
                msg = "删除失败。";
            else
            {
                LogBll<Button> log = new LogBll<Button>();
                log.DeleteLog(btn);
            }

            return new JsonMessage { Success = true, Data = k.ToString(), Message = msg }.ToString();
        }

    }
}

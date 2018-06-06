using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xiucai.Common.Data.Filter;
using Xiucai.BPM.Core.Bll;
using Xiucai.BPM.Core.Model;
using Xiucai.BPM.Core.Dal;
using XiuCai.Demo;
using System.Text;
using System.Web;
namespace Xiucai.CommonTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string jsonFilter = "{\"groupOp\":\"AND\",\"rules\":[{\"field\":\"name\",\"op\":\"eq\",\"data\":\"abc\"}]," +
                                "\"groups\":[{\"groupOp\":\"or\",\"rules\":[" +
                                "{\"field\":\"Nimi\",\"op\":\"cn\",\"data\":\"kk\"}," +
                                "{\"field\":\"Nimi\",\"op\":\"cn\",\"data\":\"kkk\"}],\"groups\":[]}]}";
            string jsonFilter2 = "{\"groupOp\":\"AND\",\"rules\":[{\"field\":\"a.id\",\"op\":\"eq\",\"data\":\"\"},{\"field\":\"a.amount\",\"op\":\"eq\",\"data\":\"\"}],\"groups\":[{\"groupOp\":\"OR\",\"rules\":[{\"field\":\"a.invdate\",\"op\":\"eq\",\"data\":\"\"},{\"field\":\"a.id\",\"op\":\"eq\",\"data\":\"\"}],\"groups\":[{\"groupOp\":\"AND\",\"rules\":[{\"field\":\"a.invdate\",\"op\":\"eq\",\"data\":\"\"}],\"groups\":[]}]}]}";
            Console.Write(FilterTranslator.ToSql(jsonFilter2));
        }
        [TestMethod]
        public  void TestRoleNavBtns()
        {
            var json = "{\"roleId\":3,\"menus\":[{\"navid\":1,\"buttons\":[\"broswer\"]},{\"navid\":2,\"buttons\":[\"broswer\",\"add\",\"edit\",\"delete\",\"search\"]},{\"navid\":10,\"buttons\":[\"broswer\",\"add\",\"edit\",\"delete\"]},{\"navid\":11,\"buttons\":[\"broswer\",\"add\",\"edit\",\"delete\"]},{\"navid\":12,\"buttons\":[\"broswer\",\"add\",\"edit\",\"delete\",\"search\",\"export\",\"set\"]},{\"navid\":13,\"buttons\":[\"broswer\",\"add\",\"edit\",\"delete\",\"export\"]},{\"navid\":14,\"buttons\":[\"broswer\",\"add\",\"edit\",\"delete\",\"inport\",\"export\"]},{\"navid\":15,\"buttons\":[\"broswer\",\"delete\",\"search\",\"export\"]},{\"navid\":16,\"buttons\":[\"broswer\",\"add\",\"edit\"]},{\"navid\":17,\"buttons\":[\"broswer\",\"add\",\"download\"]}]}";
            var json2 =
                "{\"roleId\":3,\"menus\":[{\"navid\":1,\"buttons\":[\"broswer\"]},{\"navid\":2,\"buttons\":[\"broswer\",\"edit\"]},{\"navid\":10,\"buttons\":[\"broswer\",\"add\",\"edit\",\"delete\"]},{\"navid\":11,\"buttons\":[\"broswer\",\"add\",\"edit\",\"delete\"]},{\"navid\":12,\"buttons\":[\"broswer\",\"search\"]},{\"navid\":13,\"buttons\":[\"broswer\",\"edit\",\"delete\"]},{\"navid\":14,\"buttons\":[\"broswer\",\"add\",\"delete\"]},{\"navid\":15,\"buttons\":[\"broswer\",\"delete\"]},{\"navid\":16,\"buttons\":[\"broswer\",\"edit\"]},{\"navid\":17,\"buttons\":[\"broswer\",\"add\"]}]}";
            Console.Write(RoleBll.Instance.RoleAuthorize(json2));
        }
        [TestMethod]
        public void TestInitRoleNavbtns()
        {
            string s = RoleBll.Instance.GetRoleNavBtns(3);
            //File.WriteAllText("d:\\test.txt",s);
            Console.Write(s);
        }

        [TestMethod]
        public void TestUserBtns()
        {
            Console.Write(UserBll.Instance.GetNavBtnsJson(1));
        }

        [TestMethod]
        public void TestUserMenus()
        {
            var username = "test";
            //Console.Write(UserBll.Instance.GetNavJson(username));
            var s = UserBll.Instance.GetNavJson(username);
            File.WriteAllText("d:\\test.txt", s);
        }

        [TestMethod]
        public void TestDepTree()
        {
            Console.Write(RoleDal.Instance.GetDepIDs(3));
        }

        
        [TestMethod]
        public void DemoMembers()
        {
            var list = new List<DemoMember>
                           {
                               new DemoMember {KeyID = 1, Name = "火枪手", Company = "某知名集团", Ownner = 10, DepID = 10},
                               new DemoMember {KeyID = 2, Name = "小黑", Company = "某知名集团", Ownner = 10, DepID = 12},
                               new DemoMember {KeyID = 3, Name = "剑圣", Company = "某知名集团", Ownner = 10, DepID = 10},
                               new DemoMember {KeyID = 4, Name = "白牛", Company = "某知名集团", Ownner = 10, DepID = 11},
                               new DemoMember {KeyID = 5, Name = "唐僧", Company = "某知名集团", Ownner = 10, DepID = 9},
                               new DemoMember {KeyID = 6, Name = "潮汐猎人", Company = "某知名集团", Ownner = 10, DepID = 8},
                               new DemoMember {KeyID = 7, Name = "食尸鬼", Company = "某知名集团", Ownner = 10, DepID = 7},
                               new DemoMember {KeyID = 8, Name = "黑鸟", Company = "某知名集团", Ownner = 10, DepID = 8},
                               new DemoMember {KeyID = 9, Name = "腹蛇", Company = "某知名集团", Ownner = 10, DepID = 10},
                               new DemoMember {KeyID = 10,Name = "斧王", Company = "某知名集团", Ownner = 1, DepID = 11},
                               new DemoMember {KeyID = 11,Name = "熊战", Company = "某知名集团", Ownner = 1, DepID = 12},
                               new DemoMember {KeyID = 12, Name = "火凤凰", Company = "某知名集团", Ownner = 1, DepID = 15},
                               new DemoMember {KeyID = 13, Name = "骷髅王", Company = "某知名集团", Ownner = 1, DepID = 15},
                               new DemoMember {KeyID = 14, Name = "蝙蝠侠", Company = "某知名集团", Ownner = 1, DepID = 15},
                               new DemoMember {KeyID = 15, Name = "钢铁侠", Company = "某知名集团", Ownner = 1, DepID = 15},
                               new DemoMember {KeyID = 16, Name = "超人", Company = "某知名集团", Ownner = 1, DepID = 15},
                               new DemoMember {KeyID = 17, Name = "绿巨人", Company = "某知名集团", Ownner = 1, DepID = 8},
                               new DemoMember {KeyID = 18, Name = "美国队长", Company = "某知名集团", Ownner = 1, DepID = 8},
                               new DemoMember {KeyID = 19, Name = "蜘蛛侠", Company = "某知名集团", Ownner = 1, DepID = 8},
                               new DemoMember {KeyID = 20, Name = "燕子李三", Company = "某知名集团", Ownner = 1, DepID = 8},
                               new DemoMember {KeyID = 21, Name = "军师", Company = "某知名集团", Ownner = 1, DepID = 9}
                           };
            
            StringBuilder sb = new StringBuilder();
            string temp = "insert into demo_users (name,company,ownner,depid) values('{0}','{1}','{2}','{3}')";

            foreach (DemoMember demoMember in list)
            {
                sb.AppendFormat(temp, demoMember.Name, demoMember.Company, demoMember.Ownner, demoMember.DepID);
                sb.AppendLine();
            }

            Console.Write(sb.ToString());
        } 
    }

    
}

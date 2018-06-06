using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.Common.Data;
using Xiucai.BPM.Core.Model;
namespace Xiucai.BPM.Core.Dal
{
    public class NavigationDal: BaseRepository<Navigation>
    {
        private NavigationDal() { }
        private static readonly object syncObject = new object();
        private static NavigationDal singleton;

        public static NavigationDal Instance
        {
            get
            {
                if (singleton == null)
                {
                    lock (syncObject)
                    {
                        if (singleton == null)
                        {
                            singleton = new NavigationDal();
                        }
                    }
                }
                return singleton;
            }
        }

        public IEnumerable<Navigation> GetList(int parentid=0)
        {
            return from n in base.GetAll()
                   where n.ParentID == parentid
                   orderby n.Sortnum
                   select n;
        }

        
    }
}

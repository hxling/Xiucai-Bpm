using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.BPM.Core.Model;
using Xiucai.Common.Data;
using Xiucai.Common.Provider;

namespace Xiucai.BPM.Core.Dal
{
    public class DicCategoryDal : BaseRepository<DicCategory>
    {
        public static DicCategoryDal Instance
        {
            get { return SingletonProvider<DicCategoryDal>.Instance; }
        }

        
    }

}

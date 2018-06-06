using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xiucai.BPM.Core.Model;
using Xiucai.Common.Data;
using Xiucai.Common.Provider;

namespace Xiucai.BPM.Core.Dal
{
    public class LogDetailDal:BaseRepository<LogDetailModel>
    {
        public static LogDetailDal Instance
        {
            get { return SingletonProvider<LogDetailDal>.Instance; }
        }

        public IEnumerable<LogDetailModel> GetBy(int logId)
        {
            return DbUtils.GetWhere<LogDetailModel>(new {LogId = logId});
        }

        public int DeleteBy(string logIds)
        {
            string s = "delete sys_logdetails where logid in (" + logIds + ")";
            return DbUtils.ExecuteNonQuery(s, null);
        }
    }
}

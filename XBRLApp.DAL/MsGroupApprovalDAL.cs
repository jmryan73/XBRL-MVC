using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBRLApp.DAL
{
    public class MsGroupApprovalDAL:IDisposable
    {
        public XbrlDbCsEntities db = new XbrlDbCsEntities();
        public IEnumerable<MsGroupApproval> GetAllData()
        {
            var model = db.MsGroupApproval.Select(x => x);
            return model;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TequaCreek.eProcClientWSLibrary
{
    public class ApprovalHistoryParameter
    {
        public int RequisitionID { get; set; }
        public string SupervisorToApproveUserID { get; set; }
        public string SupervisorApprovedUserID { get; set; }

    }
}

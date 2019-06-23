using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TequaCreek.eProcClientWSLibrary
{
    public class RequisitionApprovalHistoryEntry
    {

        public int RequisitionID { get; set; }
        public int ResubmitInstance { get; set; }
        public System.DateTime ApprovalDateTime { get; set; }
        public string SupervisorToApproveUserID { get; set; }
        public string SupervisorApprovedUserID { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TequaCreek.eProcClientWSLibrary
{
    public class ReturnedRequisitionEMailMessageParameter
    {

        public int RequisitionID { get; set; }
        public int ResubmitInstance { get; set; }
        public string ReturnToRequestorReason { get; set; }
        public string ReturnToRequestorSpecificReason { get; set; }

    }
}

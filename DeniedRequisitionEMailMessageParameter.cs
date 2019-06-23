using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TequaCreek.eProcClientWSLibrary
{
    public class DeniedRequisitionEMailMessageParameter
    {
        public int RequisitionID { get; set; }
        public bool CopyAssistantTA { get; set; }
        public bool CopyTribalAdmin { get; set; }
        public bool CopyTribalJudge { get; set; }
        public bool CopyExecCouncil { get; set; }    

    }
}

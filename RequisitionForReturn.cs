using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TequaCreek.eProcClientWSLibrary
{
    public class RequisitionForReturn
    {

        public int requisitionId { get; set; }
        public string requisitionTitle { get; set; }
        public string requestor { get; set; }
        public string department { get; set; }
        public string comments { get; set; }
        public string errorMessage { get; set; }
        public string actingUserName { get; set; }
        public string actingUserFriendlyName { get; set; }
        public string ipAddress { get; set; }
        public string targetDevice { get; set; }
        public bool allowSupervisorApproval { get; set; }
        public bool allowTribalAdministratorApproval { get; set; }
        public bool allowTribalJudgeApproval { get; set; }
        public bool allowExecutiveCouncilApproval { get; set; }
        public string comparatorHash { get; set; }

    }

}

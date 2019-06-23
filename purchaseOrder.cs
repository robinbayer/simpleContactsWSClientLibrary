using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TequaCreek.eProcClientWSLibrary
{
    public class purchaseOrder
    {
        public int callresult { get; set; }
        public DateTime purchaseOrderDate { get; set; }
        public string vendorNumber { get; set; }
        public string vendorName { get; set; }
        public int requisitionId { get; set; }
        public int subDepartmentId { get; set; }
        public int departmentId { get; set; }
        public string requestorUserName { get; set; }
        public string proxyUserName { get; set; }
    }
}

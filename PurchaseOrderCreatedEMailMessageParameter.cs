using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TequaCreek.eProcClientWSLibrary
{
    public class PurchaseOrderCreatedEMailMessageParameter
    {

        public int requisitionId { get; set; }

        // PROGRAMMER'S NOTE:  This parameter is only used when invoking a local call to a controller
        public string baseUrl { get; set; }

    }
}

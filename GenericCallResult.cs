using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TequaCreek.eProcClientWSLibrary
{
    public class GenericCallResult
    {

        public const int RESULT_CODE_SUCCESS = 0;
        public const int RESULT_CODE_INVALID_PARAMETER = 1;
        public const int RESULT_CODE_UNKNOWN_ERROR = 2;
        
        public int ResultCode { get; set; }
        public string AdditionalInformation { get; set; }
    }
}

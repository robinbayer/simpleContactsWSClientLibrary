using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Overthink.SimpleContactsWSClientLibrary
{
    public class APICallResult
    {
        public int resultCode { get; set; }
        public string successKeyValue { get; set; }
        public List<string> errors { get; set; }

    }
}

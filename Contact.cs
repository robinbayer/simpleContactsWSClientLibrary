using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Overthink.SimpleContactsWSClientLibrary
{
    public class Contact
    {
        public string contactId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string cellPhone { get; set; }
        public string homePhone { get; set; }
        public string workPhone { get; set; }
        public string emailAddress { get; set; }
        public DateTime birthDate { get; set; }
        public string locationAddress { get; set; }

    }
}

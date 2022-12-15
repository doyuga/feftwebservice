using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FEFTRest
{
    [DataContract]
    public class ReprintRequest
    {
        [DataMember]
        public String Bank { get; set; }
    }
}

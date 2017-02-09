using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZipCodeLookup02
{
    [DataContract]
    public class ZipInfo
    {
        [DataMember]
        internal string Longitude { get; set; }
        [DataMember]
        internal string Zipcode { get; set; }
        [DataMember]
        internal string ZipClass { get; set; }
        [DataMember]
        internal string County { get; set; }
        [DataMember]
        internal string State { get; set; }
        [DataMember]
        internal string Latitude { get; set; }
    }

}

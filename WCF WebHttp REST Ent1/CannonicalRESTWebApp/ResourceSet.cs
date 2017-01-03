using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CannonicalRESTWebApp
{
    [DataContract(Name = "ResourceSet", Namespace = "http://microsoft.com/samples/wcf/rest")]
    public class ResourceSet<TResource>
    {
        [DataMember]
        public int SetCount;

        [DataMember]
        public int TotalCount;

        [DataMember]
        public int Skip;

        [DataMember]
        public int Take;

        [DataMember]
        public IList<TResource> Resources;
    }
}

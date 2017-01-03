using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CannonicalRESTWebApp.Tests
{
    /// <summary>
    /// This type is duplicated in the test project because we may want to make it different
    /// than the server version
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
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

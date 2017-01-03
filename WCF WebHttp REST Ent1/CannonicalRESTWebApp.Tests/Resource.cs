using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CannonicalRESTWebApp.Tests
{
    /// <summary>
    /// Client view of the Resource class
    /// </summary>
    [DataContract(Namespace = "http://microsoft.com/samples/wcf/rest")]
    public class Resource
    {
        [DataMember]
        public string Data;

        [DataMember]
        public string ReadOnlyData;

        [DataMember]
        public int Key;

        [DataMember]
        public Guid Version;
    }
}
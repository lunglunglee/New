using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CannonicalRESTWebApp
{
    public class ResourceConflictComparer : IEqualityComparer<KeyValuePair<int, Resource>>
    {
        public bool Equals(KeyValuePair<int, Resource> x, KeyValuePair<int, Resource> y)
        {
            // Resources are considered equal if the data is equal 
            // for this sample
            return x.Value.Data == y.Value.Data;
        }

        public int GetHashCode(KeyValuePair<int, Resource> obj)
        {
            return obj.Value.Data.GetHashCode();
        }
    }
}
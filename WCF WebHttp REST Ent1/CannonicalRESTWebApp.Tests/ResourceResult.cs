using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Http.Headers;

namespace CannonicalRESTWebApp.Tests
{
    public class ResourceResult<TResource> : TestResult<TResource>
    {
        public TResource Resource { get; set; }

        public string LocationKey
        {
            get
            {
                if (ResponseHeaders != null)
                {
                    if (ResponseHeaders.Location != null)
                    {
                        string[] segments = ResponseHeaders.Location.PathAndQuery.Split('/');
                        return segments[segments.Length-1];
                    }
                }

                return null;
            }
        }
    }
}

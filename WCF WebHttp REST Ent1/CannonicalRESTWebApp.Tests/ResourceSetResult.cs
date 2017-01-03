using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Http.Headers;

namespace CannonicalRESTWebApp.Tests
{
    public class ResourceSetResult<TResource> : TestResult<TResource>
    {
        public ResourceSet<TResource> ResourceSet { get; set; }
    }
}

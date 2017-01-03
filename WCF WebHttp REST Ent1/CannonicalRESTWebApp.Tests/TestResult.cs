using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Http.Headers;

namespace CannonicalRESTWebApp.Tests
{
    public abstract class TestResult<TResource>
    {
        public HttpStatusCode Status { get; set; }

        public ResponseHeaders ResponseHeaders { get; set; }

        public string ResponseContent { get; set; }

        public string ErrorMessage { get; set; }

    }
}

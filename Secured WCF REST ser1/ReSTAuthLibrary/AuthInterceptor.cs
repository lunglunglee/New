
#region Imports
using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using Microsoft.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Channels;
#endregion 

namespace ReSTAuthLibrary
{
    /// <summary>
    /// Custom class to implement Request Interception for ReST
    /// </summary>
    public class AuthInterceptor : RequestInterceptor
    {
        /// <summary>
        /// Realm name 
        /// </summary>
        public string Realm
        {
            get { return "SomeRealm"; }
        }

        /// <summary>
        /// Pass isSynchronous perameter as false to base class
        /// </summary>
        public AuthInterceptor(): base (false){}

        /// <summary>
        /// Override ProcessRequest implements custom authentication 
        /// </summary>
        /// <param name="requestContext"></param>
        public override void ProcessRequest(ref RequestContext requestContext)
        {
            AppUser objAppUser = GetUserCredentials(requestContext.RequestMessage);
            if (objAppUser != null && AppUserHelper.ValidateUser(objAppUser))
            {
                // TODO: Initialize security context here. Home work for users :)
            }
            else
                // User is not authenticated, prompt Unauthorized access
                GenerateErrorResponse( requestContext, HttpStatusCode.Unauthorized, "Missing or invalid user key (supply via the Authorization header)");                             
            
        }

        /// <summary>
        /// Get user credentials from the request context
        /// </summary>
        /// <param name="requestMsg"></param>
        /// <returns></returns>
        protected AppUser GetUserCredentials(Message requestMsg)
        {
            AppUser objAppUser = null;
            HttpRequestMessageProperty msgPropery = (HttpRequestMessageProperty)requestMsg.Properties[HttpRequestMessageProperty.Name];
            
            // Check header for Authorization details
            string authHeader = msgPropery.Headers["Authorization"];
            
            // Authorization header is prefixed with the text 'Basic' followed by encoded UserName:Password
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                // Set encoding 
                Encoding encoding = Encoding.GetEncoding(28591);
                string authInfo = encoding.GetString(Convert.FromBase64String(authHeader.Substring(6).Trim()));
                int i = authInfo.IndexOf(':');

                // Return AppUser (Auth info)
                objAppUser = new AppUser(authInfo.Substring(0, i), authInfo.Substring(i+1));           
            }

            return objAppUser;
        }

        /// <summary>
        /// Generate error response
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="statusCode"></param>
        /// <param name="errorMessage"></param>
        public void GenerateErrorResponse(RequestContext requestContext, HttpStatusCode statusCode, string errorMessage)
        {
            Message reply = Message.CreateMessage(MessageVersion.None, null);
            HttpResponseMessageProperty responseProp = new HttpResponseMessageProperty()
            {
                StatusCode = statusCode
            };
           
            // Set response header
            responseProp.Headers.Add("WWW-Authenticate", String.Format("Basic realm=\"{0}\"", Realm));
            reply.Properties[HttpResponseMessageProperty.Name] = responseProp;
            requestContext.Reply(reply);

            // set the request context to null to terminate processing of this request
            requestContext = null;
        }
    }
}

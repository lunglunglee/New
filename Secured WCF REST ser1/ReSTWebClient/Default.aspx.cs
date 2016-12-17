
#region Imports
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using System.Net;
using Microsoft.Http;
using Microsoft.Http.Headers;
#endregion

namespace ReSTWebClient
{
    public partial class _Default : System.Web.UI.Page
    {
        // TODO: Change the base url 
        public string BaseURI
        {
            get { return "http://localhost/ReSTStringUtilService/ReSTStringUtilService.svc/count"; }            
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCallReSTService_Click(object sender, EventArgs e)
        {
            // Call the service
            using (HttpClient objClient = new HttpClient(this.BaseURI))
            {
                objClient.TransportSettings.Credentials = new NetworkCredential("kamal", "password");
                this.lblResult.Text = GetResponse(objClient,  string.Format("{0}/{1}",this.BaseURI, this.txtInput.Text));                 
            }
        }

        /// <summary>
        /// Call ReST service and get the response
        /// </summary>
        /// <param name="objClient"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected string GetResponse(HttpClient objClient, string uri)
        {
            string message = string.Empty;
            using (HttpResponseMessage response = objClient.Get(uri))
            {
                response.EnsureStatusIsSuccessful();
                message = response.Content.ReadAsString();
            }
            return message;
        }
    }
}

Introduction:

This sample demonstrates how to design secured WCF REST service. This service serves a common interface for both web and mobile platforms.
  
Building the Sample:
 
WCF REST service and web client
 •Visual Studio 2010 
 •WCF REST Starter Kit 

WCF REST WP7 client
 •Visual Studio 2010  
 •WP SDK (ver 7) 

WCF REST Android client 
 •Eclipse 3.7 
 •Android SDK (Solution tested on Android ver 2.3) 

Description:
 
The WCF REST Starter kit provides an easy way to implement request interception. For this sample we have used “request interception?to implement the authentication. 
 
In Microsoft.ServiceModel.Web assembly, there is an abstract class RequestInterceptor which provides an abstract method ProcessRequest, which is used to implement authentication in this sample. 
 
Request interception and authentication is implemented in separate class library name ReSTAuthLibrary. Following is the AuthInterceptor class which drives from RequestInterceptor 

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
 
Following is the custom ServiceHostFactory which injects the interceptor when WebServiceHost2 instance is created
 
#region Imports 
using System; 
using System.ServiceModel; 
using System.ServiceModel.Activation; 
using Microsoft.ServiceModel.Web; 
#endregion 
 
namespace ReSTAuthLibrary 
{ 
    /// <summary> 
    /// Custom ServiceHostFactory that injects the interceptor when the WebServiceHost2 instance is first created 
    /// </summary>   
    public class SecureWebServiceHostFactory : ServiceHostFactory 
    { 
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses) 
        { 
            WebServiceHost2 host = new WebServiceHost2(serviceType, true, baseAddresses); 
            host.Interceptors.Add(new AuthInterceptor()); 
            return host; 
        } 
    } 
} 
 
Next step is to configure our WCF service to call the custom ServiceHostFactory. After this change, our ReSTStringUtilService.svc file looks like following
 

<%@ ServiceHost Language="C#" Debug="true" Service="ReSTStringUtilService.ReSTStringUtilService"  
CodeBehind="ReSTStringUtilService.cs" Factory="ReSTAuthLibrary.SecureWebServiceHostFactory" %> 
 
 
The code behind implements a simple function, which returns string length 
 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.ServiceModel; 
using System.ServiceModel.Activation; 
using System.ServiceModel.Web; 
using System.Text; 
 
namespace ReSTStringUtilService 
{ 
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page 
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want 
    // a single instance of the service to process all calls.     
    [ServiceContract] 
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)] 
    // NOTE: If the service is renamed, remember to update the global.asax.cs file 
    public class ReSTStringUtilService 
    { 
        [WebGet(UriTemplate = @"count/{str}")] 
        public int Get(string str) 
        { 
            return str.Count(); 
        }  
    } 
} 
At this point, if we run the ReSTStringUtilService, we should get a login box. For this sample code use name and password is kamal/password.
 
  
Testing the service
 
Testing the service using Asp.Net web client
 
The ReSTWebClient web application can be used to test the REST service. Following is the code behind for web client
 
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
 
 Testing the service using WP7 client
 
Following is the code behind of MainPage.xaml.cs
 
#region Imports 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Net; 
using System.Windows; 
using System.Windows.Controls; 
using System.Windows.Documents; 
using System.Windows.Input; 
using System.Windows.Media; 
using System.Windows.Media.Animation; 
using System.Windows.Shapes; 
using Microsoft.Phone.Controls; 
using System.IO; 
using System.Xml.Linq; 
#endregion 
 
namespace ReSTWP7Client 
{ 
    public partial class MainPage : PhoneApplicationPage 
    { 
        #region Vars 
        WebClient request; 
 
        // TODO : Change the URL 
        String serviceAddress = "http://localhost/ReSTStringUtilService/ReSTStringUtilService.svc/count/"; 
        #endregion 
 
        #region Properties 
        public String ServiceAddress 
        { 
            get { return serviceAddress; }            
        } 
        #endregion 
 
        public MainPage() 
        { 
            InitializeComponent(); 
            request = new WebClient(); 
            request.Credentials = new NetworkCredential("kamal", "password"); 
            request.DownloadStringCompleted += new DownloadStringCompletedEventHandler(request_DownloadStringCompleted); 
            request.DownloadStringAsync(new Uri(ServiceAddress + txtInput.Text)); 
        } 
 
        void request_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e) 
        { 
            // Parse the xml and get the result 
            XDocument result = XDocument.Parse(e.Result); 
            txtOutput.Text = "Count: " + result.Root.Value; 
        } 
 
        private void Button_Click(object sender, RoutedEventArgs e) 
        { 
            // Call the ReST service 
            if (!String.IsNullOrEmpty(txtInput.Text)) 
            { 
                request.DownloadStringAsync(new Uri(serviceAddress + txtInput.Text)); 
            } 
        } 
    } 
} 
 
 
 
 
Testing the service using Android client
 
Following is the sample code for testing our service on Android
 

package com.ReSTAndroidTest; 
 
import java.io.IOException; 
import java.io.StringReader; 
 
import javax.xml.parsers.DocumentBuilder; 
import javax.xml.parsers.DocumentBuilderFactory; 
import javax.xml.parsers.ParserConfigurationException; 
 
import org.apache.http.auth.AuthScope; 
import org.apache.http.auth.UsernamePasswordCredentials; 
import org.apache.http.client.ClientProtocolException; 
import org.apache.http.client.CredentialsProvider; 
import org.apache.http.client.HttpClient; 
import org.apache.http.client.ResponseHandler; 
import org.apache.http.client.methods.HttpGet; 
import org.apache.http.impl.client.BasicCredentialsProvider; 
import org.apache.http.impl.client.BasicResponseHandler; 
import org.apache.http.impl.client.DefaultHttpClient; 
import org.w3c.dom.CharacterData; 
import org.w3c.dom.Document; 
import org.w3c.dom.Element; 
import org.w3c.dom.Node; 
import org.w3c.dom.NodeList; 
import org.xml.sax.InputSource; 
 
import android.app.Activity; 
import android.os.Bundle; 
import android.widget.TextView; 
 
public class ReSTAndroidTestActivity extends Activity { 
    /** Called when the activity is first created. */ 
    @Override 
    public void onCreate(Bundle savedInstanceState) { 
        super.onCreate(savedInstanceState); 
        TextView tview = new TextView(this); 
         
        // For REST Authentication 
        CredentialsProvider credProvider = new BasicCredentialsProvider(); 
        credProvider.setCredentials(new AuthScope(AuthScope.ANY_HOST, AuthScope.ANY_PORT), 
            new UsernamePasswordCredentials("kamal", "password")); 
         
        DefaultHttpClient httpclient = new DefaultHttpClient(); 
        httpclient.setCredentialsProvider(credProvider); 
 
        try 
        { 
             // Call the REST service 
             HttpGet request = new HttpGet("http://10.0.2.2:55786/ReSTStringUtilService.svc/count/asdf"); 
             ResponseHandler<String> handler = new BasicResponseHandler(); 
             String result = httpclient.execute(request, handler); 
              
             // Parse xml          
             try { 
                    DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance(); 
                    DocumentBuilder db = dbf.newDocumentBuilder(); 
                    InputSource is = new InputSource(); 
                    is.setCharacterStream(new StringReader(result)); 
                    Document doc = db.parse(is); 
                    NodeList nodes = doc.getChildNodes(); 
                    Element line = (Element) nodes.item(0); 
                                      
                    // Display the result 
                    tview.setText("String length: " + getCharacterData(line)); 
                    setContentView(tview);      
                } 
                catch (Exception e) { 
                    e.printStackTrace(); 
                }                      
        }  
        catch (ClientProtocolException e) { 
               e.printStackTrace();}  
        catch (IOException e) { 
               e.printStackTrace(); } 
    } 
     
     public static String getCharacterData(Element e) { 
        Node child = e.getFirstChild(); 
        if (child instanceof CharacterData) { 
           CharacterData cd = (CharacterData) child; 
           return cd.getData(); 
        } 
        return "?"; 
      } 
} 
  
More Information:
Introducing WCF WebHttp Services in .NET 4
A Developer's Guide to the WCF REST Starter Kit
WCF REST Starter Kit Preview 2
Consuming REST services with HttpClient


NOTE: Windows WP7 and Android client code is in ReSTWP7Client.zip and ReSTAndroidClient.zip

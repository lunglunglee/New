using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;

namespace CannonicalRESTWebApp
{
    // TIP: Use Visual Studio refactoring while modifying so that the references are updated
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceContract]
    public class CannonicalRESTService
    {
        // TODO: Provide a repository implementation
        // The sample repository is created with pre-populated data
        static IResourceRepository<int, Resource> resourceRepository = ResourceRepository.Create();

        // TODO: Define the maximum number of resources returned per request
        const int MaxResourcesPerRequest = 10;

        [AspNetCacheProfile("Resource")]
        // TIP: Use the Description attribute to show custom text in the help page
        [Description("Demonstrates how to do a service operation")]
        [WebGet(UriTemplate = "/Hello/{name}")]
        public string HelloWorld(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Hello!";
            else
                return string.Format("Hello {0}!", name);
        }

        // TIP: ResourceSet does not use ETags so it can use ASP.NET Caching
        [AspNetCacheProfile("ResourceSet")]
        [Description("Demonstrates how to GET a collection of resources")]
        // Tip: Allow the caller to specify skip / take to support paging
        [WebGet(UriTemplate = "/?skip={skip}&take={take}")]
        public ResourceSet<Resource> GetResources(int skip, int take)
        {
            // Tip: Validate arguments and return BadRequest for invalid arguments           
            if (skip < 0)
                throw new WebFaultException<string>(
                    string.Format("Invalid skip value {0}", skip), HttpStatusCode.BadRequest);

            if (take < 0)
                throw new WebFaultException<string>(
                    string.Format("Invalid take value {0}", take), HttpStatusCode.BadRequest);

            // Tip: Consider how many resources you are going to return
            if ((take == 0) || (take > MaxResourcesPerRequest))
                take = MaxResourcesPerRequest;

            // Tip: Return collection in a container with data to help
            // the caller to know their position in the resource set
            var result = new ResourceSet<Resource>() { Skip = skip, Take = take };

            // Tip: Use a repository class to manage access to data
            result.Resources = resourceRepository.GetResources(skip, take);
            result.SetCount = result.Resources.Count();
            result.TotalCount = resourceRepository.Resources.Count();

            return result;
        }

        // TIP: You cannot use ASP.NET Caching with ETags (it removes them)
        // Choose ASP.NET Caching or ETag support
        // [AspNetCacheProfile("Resource")]
        [Description("Demonstrates how to GET a resource")]
        [WebGet(UriTemplate = "/{key}")]
        public Resource GetResource(string key)
        {
            ValidateArguments(key);

            Resource resource = resourceRepository.Get(ParseResourceKey(key));

            // Tip: Return 404 - Not Found if you can't find the resource
            if (resource == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            // Tip: Provide ETag support
            // ETags provide a way to manage cached responses in HTTP
            // See http://en.wikipedia.org/wiki/HTTP_ETag

            // If the caller provided an ETag, this call will determine if the ETags match
            // If the tags match it will set the ETag on the response and throw a WebFaultException(HttpStatusCode.NotModified);
            Request.CheckConditionalRetrieve(resource.Version);

            // If the caller did not provide an ETag (or it was not a match)
            // Set the ETag on the response message
            Response.SetETag(resource.Version);

            return resource;
        }

        // Handles the case where there is a trailing slash
        [Description("Demonstrates how to GET a resource with a trailing slash")]
        [WebGet(UriTemplate = "/{key}/")]
        public Resource GetResourceSlash(string key)
        {
            return GetResource(key);
        }


        [AspNetCacheProfile("Resource")]
        [Description("Demonstrates how to GET a resource feed using RSS")]
        [WebGet(UriTemplate = "/rss")]
        public Rss20FeedFormatter GetResourcesRSS()
        {
            List<SyndicationItem> items = new List<SyndicationItem>();

            foreach (var resource in resourceRepository.Resources)
            {
                items.Add(new SyndicationItem()
                {
                    Title = new TextSyndicationContent(String.Format("{0} - {1}", resource.Key.ToString(), resource.Data)),
                    Content = new TextSyndicationContent(String.Format("Description for {0}", resource.Data))
                });
            }

            SyndicationFeed feed = new SyndicationFeed(items);
            return new Rss20FeedFormatter(feed);
        }

        [AspNetCacheProfile("Resource")]
        [Description("Demonstrates how to GET a resource feed using Atom")]
        [WebGet(UriTemplate = "/atom")]
        public Atom10FeedFormatter GetResourcesAtom()
        {
            List<SyndicationItem> items = new List<SyndicationItem>();

            foreach (var resource in resourceRepository.Resources)
            {
                string title = String.Format("{0} - {1}", resource.Key.ToString(), resource.Data);
                string content = String.Format("Description for {0}", resource.Data);
                Uri itemLink = CreateUri(resource.Key);
                items.Add(new SyndicationItem(title, content, itemLink));
            }
            SyndicationFeed feed = new SyndicationFeed(items);
            return new Atom10FeedFormatter(feed);
        }

        [WebInvoke(UriTemplate = "/", Method = "POST")]
        public Resource Post(Resource resourceToPost)
        {
            ValidateArguments(resourceToPost);

            var resource = resourceRepository.Post(resourceToPost);

            // Set the status code: "201 - Created" and the 
            // absolute URI of the new resource
            Response.SetStatusAsCreated(CreateUri(resource.Key));

            // Return with an ETag to enable client caching
            Response.SetETag(resource.Version);

            return resource;
        }

        // Demonstrates PUT which updates existing resources only
        [WebInvoke(UriTemplate = "/{key}", Method = "PUT")]
        public Resource PutResource(string key, Resource resourceToPut)
        {
            // Validate arguments
            ValidateArguments(key, resourceToPut);

            // Convert arguments
            int resourceKey = ParseResourceKey(key);

            // Get the old version of the resource for comparison
            var existingResource = resourceRepository.Get(resourceKey);

            // Can't find it
            if (existingResource == null)
                throw new WebFaultException(HttpStatusCode.NotFound);

            // Check the update based on etag
            // Will throw a WebFaultException(NotModified) if no update is needed
            if (Request.IfMatch != null)
                Request.CheckConditionalUpdate(existingResource.Version);

            var updatedResource = resourceRepository.Put(resourceKey, resourceToPut, existingResource);

            // Return with an ETag to enable client caching
            Response.SetETag(updatedResource.Version);

            return updatedResource;
        }

        // Using additonal path segment AddOrUpdate to disambiguate the PUT
        // in order to demonstrate the Add or Update style implementation of PUT
        [WebInvoke(UriTemplate = "/AddOrUpdate/{key}", Method = "PUT")]
        public Resource PutAddOrUpdateResource(string key, Resource resourceToPut)
        {
            // Validate arguments
            ValidateArguments(key, resourceToPut);

            var resource = resourceRepository.AddOrUpdate(
                ParseResourceKey(key),
                resourceToPut,
                // Delegate invoked when adding
                (resourceKey) => Response.SetStatusAsCreated(CreateUri(resourceKey)),
                // Delegate invoked when updating
                (version) =>
                {
                    // Check conditional update if If-Match header values provided
                    if (Request.IfMatch != null)
                        Request.CheckConditionalUpdate(version);
                }
                );

            // Return with an ETag to enable client caching
            Response.SetETag(resource.Version);

            return resource;
        }

        [WebInvoke(UriTemplate = "/{key}", Method = "DELETE")]
        public Resource DeleteResource(string key)
        {
            ValidateArguments(key);

            // Delete is an Idempotent method in HTTP
            // If you delete the same ID multiple times the result should be the same
            // Therefore if the item does not exist, do not throw an error
            // See http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html

            var resource = resourceRepository.Delete(
                ParseResourceKey(key),
                (r) =>
                {
                    if (Request.IfMatch != null)
                        Request.CheckConditionalUpdate(r.Version);
                });

            // If no resource was found (because it was previously deleted)
            if (resource == null)
                Response.StatusCode = HttpStatusCode.NoContent;

            return resource;
        }

#if DEBUG
        // Method to initialize the collection for testing
        [WebInvoke(UriTemplate = "/", Method = "TESTINIT")]
        public void Reset(string key)
        {
            resourceRepository = ResourceRepository.Create();
        }
#endif

        #region Helper methods

        public IncomingWebRequestContext Request
        {
            get { return WebOperationContext.Current.IncomingRequest; }
        }

        public OutgoingWebResponseContext Response
        {
            get { return WebOperationContext.Current.OutgoingResponse; }
        }

        private static Uri CreateUri(int key)
        {
            UriTemplate uriTemplate = new UriTemplate("/{key}");
            Uri baseUri = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri;
            Uri locationUri = uriTemplate.BindByPosition(baseUri, key.ToString());
            return locationUri;
        }

        private static void ValidateArguments(string key, Resource resource)
        {
            ValidateArguments(key);
            ValidateArguments(resource);
        }
        private static void ValidateArguments(Resource resource)
        {
            if (resource == null || !resource.IsValid())
                throw new WebFaultException<string>(
                    "Invalid Resource", System.Net.HttpStatusCode.BadRequest);
        }

        private static void ValidateArguments(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new WebFaultException<string>(
                    "Invalid key", System.Net.HttpStatusCode.BadRequest);
        }

        private static int ParseResourceKey(string key)
        {

            // If the key cannot be converted to an int
            // let the caller know it is a bad request
            // See http://en.wikipedia.org/wiki/HTTP_status_code#4xx_Client_Error

            int resourceKey;
            if (!int.TryParse(key, out resourceKey))
                throw new WebFaultException<string>(
                    string.Format("Resource ID '{0}' is invalid - it cannot be converted to a number", key),
                    System.Net.HttpStatusCode.BadRequest);

            return resourceKey;
        }

        #endregion
    }
}
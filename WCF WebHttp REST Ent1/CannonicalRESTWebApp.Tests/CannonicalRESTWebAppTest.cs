using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Microsoft.Http;
using Microsoft.Http.Headers;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Net;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading;
using System.Runtime.InteropServices;
using WCFTestHelper;

namespace CannonicalRESTWebApp.Tests
{
    [TestClass()]
    public class CannonicalRESTWebAppTest
    {
        #region Properties and Fields

        #region TestContext
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        public const string localhost = "localhost";
        public const string fiddlerLocalhost = "ipv4.fiddler";

        // Tip: Use this switch to control if you want to use fiddler for debugging
        private static bool useFiddler = true;

        private static int port = 1128;
        private const string baseUriFormat = "http://{0}:{1}/";

        // TIP: Use a property to implement fiddler debugging
        public static string BaseUri
        {
            get
            {
                if (useFiddler)
                    return string.Format(baseUriFormat, fiddlerLocalhost, port);
                else
                    return string.Format(baseUriFormat, localhost, port);
            }
        }

        // TIP: Add trailing "/" to avoid redirects from the service
        private static string servicePath = "CannonicalRESTService.svc/";

        // TIP: Use a property for your URI to simplify test code
        public static string ServiceUri
        {
            get
            {
                return BaseUri + servicePath;
            }
        }

        // When testing a service that supports both styles of PUT (Update/AddOrUpdate)
        // you have to add a path segment
        public static string AddOrUpdateServiceUri
        {
            get { return ServiceUri + "AddOrUpdate/"; }
        }

        #endregion

        #region Initialize and Cleanup

        [ClassCleanup]
        public static void CloseServers()
        {
            // TIP: Use helper classes to close servers required for testing
            WCFWebDevServer40.Close(port);

            // Could also close Fiddler
            // FiddlerDebugProxy.Close();
        }

        [TestInitialize]
        public void InitializeResourceCollection()
        {
            // TIP: When testing services with ASP.NET Compatibility Mode Required you must host with IIS or ASP.NET Development Server
            WCFWebDevServer40.EnsureIsRunning(port, WCFWebDevServer40.GetWebPathFromSolutionPath(testContextInstance));

            // TIP: Fiddler is a great tool for understanding HTTP traffice
            if (useFiddler)
                FiddlerDebugProxy.EnsureIsRunning();

            using (HttpClient client = new HttpClient())
            {
                // TIP: Initialize your service to a known state before each test
                Debug.WriteLine("Sending TESTINIT command to {0}", ServiceUri);
                using (HttpRequestMessage request = new HttpRequestMessage("TESTINIT", ServiceUri))
                {
                    client.Send(request);
                }
            }
        }

        #endregion

        #region GET Tests for template /{key}

        // HTTP GET Spec http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#sec9.3

        // GET MUST return a resource given a key if the resource with that key exists
        // GET MUST return a resource given a key if the resource with that key exists with a trailing slash
        // GET MUST return 400-BadRequest if the key is invalid
        // GET MUST return 404-NotFound if the key is not found
        // GET MUST return 304-NotModified if Conditional GET conditions are met using If-None-Match 
        // GET SHOULD return an ETag header

        /// <summary>
        /// GET MUST return a resource given a key if the resource with that key exists
        /// </summary>
        [TestMethod]
        public void GET_MUST_return_a_resource_given_a_key_if_the_resource_with_that_key_exists_Xml()
        {
            GET_MUST_return_a_resource_given_a_key_if_the_resource_with_that_key_exists(WebContentFormat.Xml);
        }

        [TestMethod]
        public void GET_MUST_return_a_resource_given_a_key_if_the_resource_with_that_key_exists_Json()
        {
            GET_MUST_return_a_resource_given_a_key_if_the_resource_with_that_key_exists(WebContentFormat.Json);
        }

        // TIP: Use this pattern when testing for both XML and Json
        public void GET_MUST_return_a_resource_given_a_key_if_the_resource_with_that_key_exists(WebContentFormat format)
        {
            // Arrange
            int expectedKey = 1;

            // TIP: HttpTestHelper makes it easy to test your service
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.GetResource(expectedKey, format: format);

            // Assert
            Assert.AreEqual(expectedKey, result.Resource.Key);
            Assert.AreEqual(HttpStatusCode.OK, result.Status);
        }

        /// <summary>
        /// GET MUST return a resource given a key if the resource with that key exists with a trailing slash
        /// </summary>
        [TestMethod]
        public void GET_MUST_return_a_resource_given_a_key_if_the_resource_with_that_key_exists_slash_Xml()
        {
            GET_MUST_return_a_resource_given_a_key_if_the_resource_with_that_key_exists_slash(WebContentFormat.Xml);
        }

        [TestMethod]
        public void GET_MUST_return_a_resource_given_a_key_if_the_resource_with_that_key_exists_slash_Json()
        {
            GET_MUST_return_a_resource_given_a_key_if_the_resource_with_that_key_exists_slash(WebContentFormat.Json);
        }

        public void GET_MUST_return_a_resource_given_a_key_if_the_resource_with_that_key_exists_slash(WebContentFormat format)
        {
            // Arrange
            int expectedKey = 1;

            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.GetResource(expectedKey, format: format, trailingSlash: true);

            // Assert
            Assert.AreEqual(expectedKey, result.Resource.Key);
            Assert.AreEqual(HttpStatusCode.OK, result.Status);
        }

        /// <summary>
        /// GET MUST return 400-BadRequest if the key is invalid
        /// </summary>
        /// <remarks>Test is the same for XML or Json</remarks>
        [TestMethod]
        public void GET_MUST_return_400_BadRequest_if_the_key_is_invalid()
        {
            // Arrange
            // TIP: To test an invalid URI with HttpTestHelper change the Key type to string
            var testHelper = new HttpTestHelper<string, Resource>(ServiceUri);

            // Act
            var result = testHelper.GetResource("badkey");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.Status);
        }

        /// <summary>
        /// GET MUST return 404-NotFound if the key is not found
        /// </summary>
        /// <remarks>Test is the same for XML or Json</remarks>
        [TestMethod]
        public void GET_MUST_return_404_NotFound_if_the_key_is_not_found()
        {
            // Arrange
            int expectedKey = -1;
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.GetResource(expectedKey);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, result.Status);
        }

        /// <summary>
        /// GET MUST return 304-NotModified if Conditional GET conditions are met using If-None-Match 
        /// </summary>
        /// <remarks>Test is the same for XML or Json</remarks>
        [TestMethod]
        public void GET_MUST_return_304_NotModified_if_Conditional_GET_conditions_are_met()
        {
            // Arrange
            int expectedKey = 1;
            EntityTag requestETag = null;

            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.GetResource(expectedKey);

            requestETag = result.ResponseHeaders.ETag;
            var result2 = testHelper.GetResource(expectedKey, requestETag);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotModified, result2.Status);
        }

        /// <summary>
        /// GET SHOULD return an ETag header
        /// </summary>
        /// <remarks>Test is the same for XML or Json</remarks>
        [TestMethod]
        public void GET_SHOULD_return_an_ETag_header()
        {
            // Arrange
            int expectedKey = 1;
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.GetResource(expectedKey);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.Status);
            Assert.IsNotNull(result.ResponseHeaders.ETag);
        }

        #endregion

        #region GET Tests for template /?skip={skip}&take={take}

        // GET MUST skip {skip} resources in the collection and return up to {take} resources.  
        // GET MUST return resources starting with the first one when {skip} is not defined
        // GET MUST return zero resources when {skip} is greater than the number of resources in the collection
        // GET MUST return 400-BadRequest if {skip} is < 0
        // GET MUST return zero or more resources when {take} is not provided
        // GET MUST return 400-BadRequest if {take} is < 0

        /// <summary>
        /// GET MUST skip {skip} resources in the collection and return up to {take} resources.   
        /// </summary>
        [TestMethod]
        public void GET_MUST_skip_skip_resources_in_the_collection_and_return_up_to_take_resources_Xml()
        {
            GET_MUST_skip_skip_resources_in_the_collection_and_return_up_to_take_resources(WebContentFormat.Xml);
        }

        [TestMethod]
        public void GET_MUST_skip_skip_resources_in_the_collection_and_return_up_to_take_resources_Json()
        {
            GET_MUST_skip_skip_resources_in_the_collection_and_return_up_to_take_resources(WebContentFormat.Json);
        }

        public void GET_MUST_skip_skip_resources_in_the_collection_and_return_up_to_take_resources(WebContentFormat format)
        {
            // Arrange
            int expectedSkip = 1;
            int expectedTake = 3;
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.GetResourceSet(expectedSkip, expectedTake, format);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.Status);
            Assert.AreEqual(expectedSkip, result.ResourceSet.Skip);
            Assert.AreEqual(expectedTake, result.ResourceSet.Take);
            Assert.AreEqual(expectedTake, result.ResourceSet.SetCount);
            Assert.AreEqual(expectedTake, result.ResourceSet.Resources.Count());
        }

        /// <summary>
        /// GET MUST return resources starting with the first one when {skip} is not defined
        /// </summary>
        [TestMethod]
        public void GET_MUST_return_resources_starting_with_the_first_one_when_skip_is_not_defined_Xml()
        {
            GET_MUST_return_resources_starting_with_the_first_one_when_skip_is_not_defined(WebContentFormat.Xml);
        }

        [TestMethod]
        public void GET_MUST_return_resources_starting_with_the_first_one_when_skip_is_not_defined_Json()
        {
            GET_MUST_return_resources_starting_with_the_first_one_when_skip_is_not_defined(WebContentFormat.Json);
        }

        public void GET_MUST_return_resources_starting_with_the_first_one_when_skip_is_not_defined(WebContentFormat format)
        {
            // Arrange
            int expectedKey = 1;
            int expectedTake = 3;
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.GetResourceSet(null, expectedTake, format);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.Status);
            Assert.AreEqual(0, result.ResourceSet.Skip);
            Assert.AreEqual(expectedKey, result.ResourceSet.Resources[0].Key);
        }

        /// <summary>
        /// GET MUST return zero resources when {skip} is greater than the number of resources in the collection 
        /// </summary>
        [TestMethod]
        public void GET_MUST_return_zero_resources_when_skip_is_greater_than_the_number_of_resources_in_the_collection_Xml()
        {
            GET_MUST_return_zero_resources_when_skip_is_greater_than_the_number_of_resources_in_the_collection(WebContentFormat.Xml);
        }

        [TestMethod]
        public void GET_MUST_return_zero_resources_when_skip_is_greater_than_the_number_of_resources_in_the_collection_Json()
        {
            GET_MUST_return_zero_resources_when_skip_is_greater_than_the_number_of_resources_in_the_collection(WebContentFormat.Json);
        }

        public void GET_MUST_return_zero_resources_when_skip_is_greater_than_the_number_of_resources_in_the_collection(WebContentFormat format)
        {
            // Arrange
            int expectedSkip = int.MaxValue;
            int expectedTake = 3;
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.GetResourceSet(expectedSkip, expectedTake, format);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.Status);
            Assert.AreEqual(expectedSkip, result.ResourceSet.Skip);
            Assert.AreEqual(expectedTake, result.ResourceSet.Take);
            Assert.AreEqual(0, result.ResourceSet.SetCount);
            Assert.AreEqual(0, result.ResourceSet.Resources.Count());
        }

        /// <summary>
        /// GET MUST return 400-BadRequest if {skip} is < 0 
        /// </summary>
        /// <remarks>Test is the same for XML or Json</remarks>
        [TestMethod]
        public void GET_MUST_return_400_BadRequest_if_skip_is_less_than_0()
        {
            // Arrange
            int expectedSkip = -1;
            int expectedTake = 3;
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.GetResourceSet(expectedSkip, expectedTake);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.Status);
            Assert.IsNull(result.ResourceSet);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorMessage));
        }

        /// <summary>
        /// GET MUST return zero or more resources when {take} is not provided
        /// </summary>
        [TestMethod]
        public void GET_MUST_return_zero_or_more_resources_when_take_is_not_provided_Xml()
        {
            GET_MUST_return_zero_or_more_resources_when_take_is_not_provided(WebContentFormat.Xml);
        }

        [TestMethod]
        public void GET_MUST_return_zero_or_more_resources_when_take_is_not_provided_Json()
        {
            GET_MUST_return_zero_or_more_resources_when_take_is_not_provided(WebContentFormat.Json);
        }

        public void GET_MUST_return_zero_or_more_resources_when_take_is_not_provided(WebContentFormat format)
        {
            // Arrange
            int expectedSkip = 5;
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.GetResourceSet(expectedSkip, format: format);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.Status);
            Assert.AreEqual(expectedSkip, result.ResourceSet.Skip);
            Assert.AreEqual(result.ResourceSet.Resources.Count(), result.ResourceSet.SetCount);
            Assert.IsTrue(result.ResourceSet.Resources.Count() > 0);
        }

        /// <summary>
        /// GET MUST return 400-BadRequest if {take} is < 0
        /// </summary>
        /// <remarks>Test is the same for XML or Json</remarks>
        [TestMethod]
        public void GET_MUST_return_400_BadRequest_if_take_is_less_than_0()
        {
            // Arrange
            int expectedTake = -1;
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.GetResourceSet(null, expectedTake);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.Status);
            Assert.IsNull(result.ResourceSet);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorMessage));
        }


        #endregion

        #region POST Tests

        // POST Spec http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#sec9.5

        // POST MUST append a valid resource to the resource collection using a server generated key and return 201 – Created with a location header, entity tag and entity body
        // POST MUST return 400-Bad Request if the entity is invalid
        // POST MUST return 409-Conflict if the entity conflicts with another entity
        // POST MUST ignore writes to entity fields the server considers read only

        /// <summary>
        /// POST MUST append a valid resource to the resource collection 
        /// using a server generated key and return 201 – Created 
        /// with a location header, entity tag and entity body
        /// </summary>
        [TestMethod]
        public void POST_MUST_append_a_valid_resource_to_the_resource_collection_Xml()
        {
            POST_MUST_append_a_valid_resource_to_the_resource_collection(WebContentFormat.Xml);
        }

        [TestMethod]
        public void POST_MUST_append_a_valid_resource_to_the_resource_collection_Json()
        {
            POST_MUST_append_a_valid_resource_to_the_resource_collection(WebContentFormat.Json);
        }

        public void POST_MUST_append_a_valid_resource_to_the_resource_collection(WebContentFormat format)
        {
            // Arrange
            string expectedData = "Post Data";
            var expectedResource = new Resource() { Data = expectedData };
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.Post(expectedResource, format);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, result.Status);

            // Check entity
            Assert.AreEqual(expectedData, result.Resource.Data);

            // Check headers
            Assert.IsNotNull(result.ResponseHeaders.ETag, "Null etag");
            Assert.IsNotNull(result.ResponseHeaders.Location, "Null location");

            // Check server generated key and location header
            Assert.AreEqual(result.Resource.Key, int.Parse(result.LocationKey), "Location header key should match entity key");
            Assert.IsTrue(result.Resource.Key > 5, "Server generated key should be > 5 on test data set");
        }

        /// <summary>
        /// POST MUST return 400-Bad Request if the entity is invalid
        /// </summary>
        [TestMethod]
        public void POST_MUST_return_400_Bad_Request_if_the_entity_is_invalid_Xml()
        {
            POST_MUST_return_400_Bad_Request_if_the_entity_is_invalid(WebContentFormat.Xml);
        }

        [TestMethod]
        public void POST_MUST_return_400_Bad_Request_if_the_entity_is_invalid_Json()
        {
            POST_MUST_return_400_Bad_Request_if_the_entity_is_invalid(WebContentFormat.Json);
        }

        public void POST_MUST_return_400_Bad_Request_if_the_entity_is_invalid(WebContentFormat format)
        {
            // Arrange
            string expectedData = string.Empty;
            var expectedResource = new Resource() { Data = expectedData };
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.Post(expectedResource, format);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.Status);
        }


        /// <summary>
        /// POST MUST return 409-Conflict if the entity conflicts with another entity
        /// </summary>
        [TestMethod]
        public void POST_MUST_return_409_Conflict_if_the_entity_conflicts_with_another_entity_Xml()
        {
            POST_MUST_return_409_Conflict_if_the_entity_conflicts_with_another_entity(WebContentFormat.Xml);
        }

        [TestMethod]
        public void POST_MUST_return_409_Conflict_if_the_entity_conflicts_with_another_entity_Json()
        {
            POST_MUST_return_409_Conflict_if_the_entity_conflicts_with_another_entity(WebContentFormat.Json);
        }

        public void POST_MUST_return_409_Conflict_if_the_entity_conflicts_with_another_entity(WebContentFormat format)
        {
            // Arrange
            string expectedData = "Post Data";
            var expectedResource = new Resource() { Data = expectedData };
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.Post(expectedResource, format);
            var result2 = testHelper.Post(expectedResource, format);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, result.Status);
            Assert.AreEqual(HttpStatusCode.Conflict, result2.Status);
        }

        /// <summary>
        /// POST MUST ignore writes to entity fields the server considers read only
        /// </summary>
        [TestMethod]
        public void POST_MUST_ignore_writes_to_entity_fields_the_server_considers_read_only_Xml()
        {
            POST_MUST_ignore_writes_to_entity_fields_the_server_considers_read_only(WebContentFormat.Xml);
        }

        [TestMethod]
        public void POST_MUST_ignore_writes_to_entity_fields_the_server_considers_read_only_Json()
        {
            POST_MUST_ignore_writes_to_entity_fields_the_server_considers_read_only(WebContentFormat.Json);
        }

        public void POST_MUST_ignore_writes_to_entity_fields_the_server_considers_read_only(WebContentFormat format)
        {
            // Arrange
            string expectedData = "Post Data";
            string notExpectedData = "Updated read only data";
            var expectedResource = new Resource() { Data = expectedData, ReadOnlyData = notExpectedData };
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.Post(expectedResource, format);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, result.Status);
            Assert.AreNotEqual(notExpectedData, result.Resource.ReadOnlyData);
        }

        #endregion

        #region PUT Tests

        // PUT Spec http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#sec9.6

        // Tests PUT /{key}

        // PUT MUST Update the entity identified by the URI if it exists and return 200-OK with the modified entity and etag header
        // PUT MAY Add a new entity using the key provided in the URI and return 201-Created with entity location and etag
        // PUT MUST respect the Precondition IfMatch
        // PUT MUST be Idempotent 
        // PUT MUST NOT alter the key of the entity so that it does not match the key of the URI
        // PUT MUST return 400-BadRequest if the entity is invalid
        // PUT MUST return 400-BadRequest if the key is invalid
        // PUT MUST ignore writes to entity fields the server considers read only
        // PUT MUST return 404-NotFound if the server does not allow new entities to be added with PUT


        /// <summary>
        /// PUT MUST Update the entity identified by the URI if it exists 
        /// and return 200-OK with the modified entity and etag header
        /// </summary>
        /// <remarks>Using AddOrUpdate implementation</remarks>
        [TestMethod]
        public void PUT_MUST_Update_the_entity_identified_by_the_URI_if_it_exists_AddOrUpdate_Xml()
        {
            PUT_MUST_Update_the_entity_identified_by_the_URI_if_it_exists_AddOrUpdate(WebContentFormat.Xml);
        }

        [TestMethod]
        public void PUT_MUST_Update_the_entity_identified_by_the_URI_if_it_exists_AddOrUpdate_Json()
        {
            PUT_MUST_Update_the_entity_identified_by_the_URI_if_it_exists_AddOrUpdate(WebContentFormat.Json);
        }

        public void PUT_MUST_Update_the_entity_identified_by_the_URI_if_it_exists_AddOrUpdate(WebContentFormat format)
        {
            ShouldUpdateResourceImpl_Xml(format, AddOrUpdateServiceUri);
        }

        /// <summary>
        /// PUT MUST Update the entity identified by the URI if it exists 
        /// and return 200-OK with the modified entity and etag header
        /// </summary>
        [TestMethod]
        public void PUT_MUST_Update_the_entity_identified_by_the_URI_if_it_exists_Xml()
        {
            PUT_MUST_Update_the_entity_identified_by_the_URI_if_it_exists(WebContentFormat.Xml);
        }

        [TestMethod]
        public void PUT_MUST_Update_the_entity_identified_by_the_URI_if_it_exists_Json()
        {
            PUT_MUST_Update_the_entity_identified_by_the_URI_if_it_exists(WebContentFormat.Json);
        }

        public void PUT_MUST_Update_the_entity_identified_by_the_URI_if_it_exists(WebContentFormat format)
        {
            ShouldUpdateResourceImpl_Xml(format: format);
        }

        public void ShouldUpdateResourceImpl_Xml(WebContentFormat format, string serviceUri = null)
        {
            // Arrange
            int resourceKey = 1;
            string expectedData = "modified";
            var testHelper = new HttpTestHelper<int, Resource>(serviceUri ?? ServiceUri);

            // Act
            var resultGet = testHelper.GetResource(resourceKey, requestUri: ServiceUri, format: format);
            Assert.AreEqual(HttpStatusCode.OK, resultGet.Status);

            var putResource = resultGet.Resource;
            putResource.Data = "modified";
            var resultPut = testHelper.PutResource(resourceKey, putResource, format:format);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, resultPut.Status);
            Assert.AreEqual(expectedData, resultPut.Resource.Data);
            Assert.AreNotEqual(resultGet.ResponseHeaders.ETag, resultPut.ResponseHeaders.ETag, "Entity tags should have changed");
            Assert.AreNotEqual(resultGet.Resource.Version, resultPut.Resource.Version, "Resource version should have changed");
        }

        /// <summary>
        /// PUT MAY Add a new entity using the key provided in the URI 
        /// and return 201-Created with entity location and etag
        /// </summary>
        [TestMethod]
        public void PUT_MAY_Add_a_new_entity_using_the_key_provided_in_the_URI_Xml()
        {
            PUT_MAY_Add_a_new_entity_using_the_key_provided_in_the_URI(WebContentFormat.Xml);
        }

        [TestMethod]
        public void PUT_MAY_Add_a_new_entity_using_the_key_provided_in_the_URI_Json()
        {
            PUT_MAY_Add_a_new_entity_using_the_key_provided_in_the_URI(WebContentFormat.Json);
        }

        public void PUT_MAY_Add_a_new_entity_using_the_key_provided_in_the_URI(WebContentFormat format)
        {
            // Arrange
            int resourceKey = 333;
            string expectedData = "Resource" + resourceKey.ToString();
            Resource putResource = new Resource() { Data = expectedData };
            Uri expectedUri = new Uri(ServiceUri + resourceKey.ToString());

            // Service implements AddOrUpdate with a different URI
            var testHelper = new HttpTestHelper<int, Resource>(AddOrUpdateServiceUri);

            // Act
            var result = testHelper.PutResource(resourceKey, putResource, format:format);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, result.Status);
            Assert.AreEqual(expectedData, result.Resource.Data);
            Assert.AreEqual(expectedUri.PathAndQuery, result.ResponseHeaders.Location.PathAndQuery);
            Assert.IsNotNull(result.ResponseHeaders.ETag);
        }


        /// <summary>
        /// PUT MUST respect the Precondition IfMatch
        /// </summary>
        [TestMethod]
        public void PUT_MUST_respect_the_Precondition_IfMatch_AddOrUpdate_Xml()
        {
            PUT_MUST_respect_the_Precondition_IfMatch_AddOrUpdate(WebContentFormat.Xml);
        }

        [TestMethod]
        public void PUT_MUST_respect_the_Precondition_IfMatch_AddOrUpdate_Json()
        {
            PUT_MUST_respect_the_Precondition_IfMatch_AddOrUpdate(WebContentFormat.Json);
        }

        public void PUT_MUST_respect_the_Precondition_IfMatch_AddOrUpdate(WebContentFormat format)
        {
            PutShouldRespectIfMatch(format, AddOrUpdateServiceUri);
        }

        /// <summary>
        /// PUT MUST respect the Precondition IfMatch
        /// </summary>
        [TestMethod]
        public void PUT_MUST_respect_the_Precondition_IfMatch_Xml()
        {
            PUT_MUST_respect_the_Precondition_IfMatch(WebContentFormat.Xml);
        }

        [TestMethod]
        public void PUT_MUST_respect_the_Precondition_IfMatch_Json()
        {
            PUT_MUST_respect_the_Precondition_IfMatch(WebContentFormat.Json);
        }

        public void PUT_MUST_respect_the_Precondition_IfMatch(WebContentFormat format)
        {
            PutShouldRespectIfMatch(format);
        }

        public void PutShouldRespectIfMatch(WebContentFormat format, string serviceUri = null)
        {
            // Arrange
            int resourceKey = 1;
            var testHelper = new HttpTestHelper<int, Resource>(serviceUri ?? ServiceUri);

            // Act
            var resultGet = testHelper.GetResource(resourceKey, requestUri: ServiceUri);

            Assert.AreEqual(HttpStatusCode.OK, resultGet.Status);

            var putResource = resultGet.Resource;
            putResource.Data = "modified";

            // Put without an entity tag
            var resultPut1 = testHelper.PutResource(resourceKey, putResource, format: format);

            // Put with an entity tag
            var resultPut2 = testHelper.PutResource(resourceKey, putResource, new EntityTag(putResource.Version.ToString()), format: format);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, resultPut1.Status);
            Assert.AreEqual(HttpStatusCode.PreconditionFailed, resultPut2.Status);
        }

        /// <summary>
        /// PUT MUST be Idempotent
        /// </summary>
        [TestMethod]
        public void PUT_MUST_be_Idempotent_Xml()
        {
            PUT_MUST_be_Idempotent(WebContentFormat.Xml);
        }

        [TestMethod]
        public void PUT_MUST_be_Idempotent_Json()
        {
            PUT_MUST_be_Idempotent(WebContentFormat.Json);
        }

        public void PUT_MUST_be_Idempotent(WebContentFormat format)
        {
            PutShouldBeIdempotent(format);
        }

        /// <summary>
        /// PUT MUST be Idempotent
        /// </summary>
        [TestMethod]
        public void PUT_MUST_be_Idempotent_AddOrUpdate_Xml()
        {
            PUT_MUST_be_Idempotent_AddOrUpdate(WebContentFormat.Xml);
        }

        [TestMethod]
        public void PUT_MUST_be_Idempotent_AddOrUpdate_Json()
        {
            PUT_MUST_be_Idempotent_AddOrUpdate(WebContentFormat.Json);
        }

        public void PUT_MUST_be_Idempotent_AddOrUpdate(WebContentFormat format)
        {
            PutShouldBeIdempotent(format, AddOrUpdateServiceUri);
        }

        private void PutShouldBeIdempotent(WebContentFormat format, string serviceUri = null)
        {
            // Arrange
            int resourceKey = 1;
            var testHelper = new HttpTestHelper<int, Resource>(serviceUri ?? ServiceUri);

            // Act
            var resultGet = testHelper.GetResource(resourceKey, requestUri: ServiceUri);

            Assert.AreEqual(HttpStatusCode.OK, resultGet.Status);

            var putResource = resultGet.Resource;
            putResource.Data = "modified";

            // Put without an entity tag
            var resultPut1 = testHelper.PutResource(resourceKey, putResource);

            // Put without an entity tag
            var resultPut2 = testHelper.PutResource(resourceKey, putResource);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, resultPut1.Status);
            Assert.AreEqual(HttpStatusCode.OK, resultPut2.Status);
        }

        #endregion

        #region DELETE Tests

        // DELETE Spec http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#sec9.7

        // Tests DELETE /{key}

        // DELETE SHOULD delete an entity that exists and return 200-OK with the deleted entity or 204-No Content if the response does not include the entity
        // DELETE SHOULD be idempotent
        // DELETE SHOULD return with 412-PreconditionFailed if no matching entity for IfMatch etag 
        // DELETE SHOULD succeed if matching entity for If-Match etag 
        // DELETE SHOULD succeed if wildcard used in If-Match etag
        // (Not Implemented) DELETE SHOULD return 202-Accepted if the request to delete has not been enacted
        // DELETE SHOULD return 400-BadRequest if the key is invalid

        /// <summary>
        /// DELETE SHOULD delete an entity that exists and 
        /// return 200-OK with the deleted entity or 
        /// 204-No Content if the response does not include the entity
        /// </summary>
        /// <remarks>Test is the same for XML or Json</remarks>
        [TestMethod]
        public void DELETE_SHOULD_delete_an_entity_that_exists()
        {
            // Arrange
            int resourceKey = 1;
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.DeleteResource(resourceKey);

            // Assert
            AssertAcceptableDeleteStatus(result);
        }

        /// <summary>
        /// DELETE SHOULD be idempotent
        /// </summary>
        /// <remarks>Test is the same for XML or Json</remarks>
        [TestMethod]
        public void DELETE_SHOULD_be_idempotent()
        {
            // Arrange
            int resourceKey = 1;
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var result = testHelper.DeleteResource(resourceKey);
            var result2 = testHelper.DeleteResource(resourceKey);

            // Assert
            AssertAcceptableDeleteStatus(result);
            AssertAcceptableDeleteStatus(result2);
        }

        /// <summary>
        /// DELETE SHOULD return with 412-PreconditionFailed if no matching entity for IfMatch etag 
        /// </summary>
        /// <remarks>Test is the same for XML or Json</remarks>
        [TestMethod]
        public void DELETE_SHOULD_return_with_412_PreconditionFailed_if_no_matching_entity_for_IfMatch_etag()
        {
            // Arrange
            int resourceKey = 1;
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act

            // Get the resource
            var resultGet = testHelper.GetResource(resourceKey);
            Assert.AreEqual(HttpStatusCode.OK, resultGet.Status);
            var deleteResource = resultGet.Resource;

            // Update so the etag won't match
            var putResource = resultGet.Resource;
            putResource.Data = "modified";
            var resultPut = testHelper.PutResource(resourceKey, putResource);
            Assert.AreEqual(HttpStatusCode.OK, resultPut.Status);

            // Try to delete - this should fail because of the precondition
            var result = testHelper.DeleteResource(resourceKey, new EntityTag(deleteResource.Version.ToString()));

            // Assert
            Assert.AreEqual(HttpStatusCode.PreconditionFailed, result.Status);
        }

        /// <summary>
        /// DELETE SHOULD succeed if matching entity for If-Match etag  
        /// </summary>
        /// <remarks>Test is the same for XML or Json</remarks>
        [TestMethod]
        public void DELETE_SHOULD_succeed_if_matching_entity_for_IfMatch_etag()
        {
            // Arrange
            int resourceKey = 1;
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var resultGet = testHelper.GetResource(resourceKey);
            Assert.AreEqual(HttpStatusCode.OK, resultGet.Status);
            var deleteResource = resultGet.Resource;

            // Delete with etag
            var result = testHelper.DeleteResource(resourceKey, new EntityTag(deleteResource.Version.ToString()));
            var result2 = testHelper.DeleteResource(resourceKey, new EntityTag(deleteResource.Version.ToString()));

            // Assert
            AssertAcceptableDeleteStatus(result);
            AssertAcceptableDeleteStatus(result2);
        }

        /// <summary>
        /// DELETE SHOULD succeed if wildcard used in If-Match etag
        /// </summary>
        /// <remarks>Test is the same for XML or Json</remarks>
        [TestMethod]
        public void DELETE_SHOULD_succeed_if_wildcard_used_in_IfMatch_etag()
        {
            // Arrange
            int resourceKey = 1;
            var testHelper = new HttpTestHelper<int, Resource>(ServiceUri);

            // Act
            var resultGet = testHelper.GetResource(resourceKey);
            Assert.AreEqual(HttpStatusCode.OK, resultGet.Status);
            var deleteResource = resultGet.Resource;

            // Update so the etag won't match
            var putResource = resultGet.Resource;
            putResource.Data = "modified";
            var resultPut = testHelper.PutResource(resourceKey, putResource);
            Assert.AreEqual(HttpStatusCode.OK, resultPut.Status);

            // Delete with wildcard etag
            var result = testHelper.DeleteResource(resourceKey, new EntityTag("*"));

            var result2 = testHelper.DeleteResource(resourceKey, new EntityTag(deleteResource.Version.ToString()));

            // Assert
            AssertAcceptableDeleteStatus(result);
            AssertAcceptableDeleteStatus(result2);
        }

        // Not implemented
        /// <summary>
        /// DELETE SHOULD return 202-Accepted if the request to delete has not been enacted
        /// </summary>
        // [TestMethod]
        // public void DELETE_SHOULD_return_202_Accepted_if_the_request_to_delete_has_not_been_enacted() { }

        /// <summary>
        /// DELETE SHOULD return 400-BadRequest if the key is invalid
        /// </summary>
        /// <remarks>Test is the same for XML or Json</remarks>
        [TestMethod]
        public void DELETE_SHOULD_return_400_BadRequest_if_the_key_is_invalid()
        {
            // Arrange
            string resourceKey = "badkey";
            // using key of type string to force bad request
            var testHelper = new HttpTestHelper<string, Resource>(ServiceUri);

            // Act
            var result = testHelper.DeleteResource(resourceKey);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.Status);
        }

        private static void AssertAcceptableDeleteStatus(ResourceResult<Resource> result)
        {
            switch (result.Status)
            {
                case HttpStatusCode.OK:
                    Assert.IsNotNull(result.Resource);
                    break;

                case HttpStatusCode.NoContent:
                    Assert.IsNull(result.Resource);
                    break;

                default:
                    Assert.Fail(string.Format("Unexpected status <{0}>", result.Status));
                    break;
            }
        }

        #endregion

        #region Service Tests
        // GET with name should return 200-OK with message

        const string HelloWorldMessageFormat = "Hello {0}!";
        const string HelloWorldUriFormat = "{0}Hello/{1}";

        [TestMethod]
        public void GET_With_Name_Should_Say_Hello_Xml()
        {
            GET_With_Name_Should_Say_Hello(WebContentFormat.Xml);
        }

        [TestMethod]
        public void GET_With_Name_Should_Say_Hello_Json()
        {
            GET_With_Name_Should_Say_Hello(WebContentFormat.Json);
        }

        public void GET_With_Name_Should_Say_Hello(WebContentFormat format)
        {
            // Arrange
            var testHelper = new HttpTestHelper<int, string>(ServiceUri);
            string expectedName = "Test Name";
            string expectedMessage = string.Format(HelloWorldMessageFormat, expectedName);
            string requestUri = string.Format(HelloWorldUriFormat, ServiceUri, expectedName);

            // Act
            var result = testHelper.SendRequest("GET", new Uri(requestUri), format);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.Status);
            Assert.AreEqual(expectedMessage, result.Resource);
        }

        #endregion
    }
}

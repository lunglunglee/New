using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ServiceModel.Web;
using System.Net;

namespace CannonicalRESTWebApp
{
    public class ResourceRepository : IResourceRepository<int, Resource>
    {
        public Resource Delete(int resourceKey, Action<Resource> checkPreCondition)
        {
            // Need to Get, Check PreCondition and Remove in one atomic operation
            lock (repository)
            {
                Resource resource = Get(resourceKey);

                if (resource != null)
                {
                    // Will throw if pre-condition fails
                    checkPreCondition(resource);

                    repository.TryRemove(resourceKey, out resource);
                }

                return resource;
            }
        }

        public Resource Get(int resourceKey)
        {
            Resource result;
            repository.TryGetValue(resourceKey, out result);

            // If not found, returns null
            return result;
        }

        public IList<Resource> GetResources(int skip, int take)
        {
            return repository.Values.Skip(skip).Take(take).ToList();
        }

        public Resource Post(Resource resourceToPost)
        {
            // Sanitize the data provided by the caller using the version the caller supplied
            var sanitizedResource = Resource.CreateSanitizedResource(GenerateId(), resourceToPost, ResourceVersionOption.New);

            if (ResourceConflict(sanitizedResource))
                throw new WebFaultException(HttpStatusCode.Conflict);

            if (repository.TryAdd(sanitizedResource.Key, sanitizedResource))
                return sanitizedResource;
            else
                throw new WebFaultException(HttpStatusCode.InternalServerError);
        }

        public bool ResourceConflict(Resource sanitizedResource)
        {
            // look for other resources with the same data
            return repository.Contains(
                new KeyValuePair<int, Resource>(sanitizedResource.Key, sanitizedResource), 
                new ResourceConflictComparer());
        }


        public Resource Put(int resourceKey, Resource resourceToPut, Resource comparisonResource)
        {
            var sanitizedResource = Resource.CreateSanitizedResource(
                resourceKey, 
                resourceToPut, 
                ResourceVersionOption.UseExisting);

            if (sanitizedResource.IsValid() && comparisonResource.DataChanged(sanitizedResource))
            {
                if (repository.TryUpdate(resourceKey, sanitizedResource, comparisonResource))
                {
                    sanitizedResource.UpdateVersion();
                    return sanitizedResource;
                }
                else
                    return null;
            }
            else // Not changed
                return sanitizedResource;
        }

        public Resource AddOrUpdate(
            int resourceKey,
            Resource resource,
            Action<int> SetStatusCreated,
            Action<Guid> CheckConditionalUpdate)
        {
            return repository.AddOrUpdate(resourceKey,

                // If resource was not found
                // This delegate will return a resource to add to the store
                (key) =>
                {
                    var sanitizedResource = 
                        Resource.CreateSanitizedResource(
                        key, resource, ResourceVersionOption.New);

                    if (SetStatusCreated != null)
                        SetStatusCreated(sanitizedResource.Key);

                    return sanitizedResource;
                },

                // If the resource was found
                // This delegate will update the resource found based on the caller provided resource
                (key, existingResource) =>
                {
                    var sanitizedResource = 
                        Resource.CreateSanitizedResource(
                        key, 
                        resource, 
                        ResourceVersionOption.UseExisting);

                    if (CheckConditionalUpdate != null)
                        CheckConditionalUpdate(existingResource.Version);

                    // Because PUT requests are Idempotent (multiple calls yield the same result)
                    // Don't change the version of the resource unless the data is really changed
                    return existingResource.UpdateFrom(sanitizedResource);
                });
        }

        public IList<Resource> Resources
        {
            get { return new ReadOnlyCollection<Resource>(repository.Values.ToList()); }
        }

        private int GenerateId()
        {
            return (from r in Resources
                    select r.Key).Max() + 1;
        }

        public static ResourceRepository Create()
        {
            var rr = new ResourceRepository();

            for (int i = 1; i <= 20; i++)
            {
                rr.repository.AddOrUpdate(i, new Resource() { Key = i, Data = "Resource" + i.ToString() }, (key, existing)=> existing);
            }

            return rr;
        }

        private ConcurrentDictionary<int, Resource> repository = new ConcurrentDictionary<int, Resource>();
    }
}
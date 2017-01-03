using System;
using System.Collections.Generic;
namespace CannonicalRESTWebApp
{
    interface IResourceRepository<TKey, TResource>
    {
        TResource AddOrUpdate(
            TKey resourceKey, 
            TResource resource, 
            Action<TKey> SetStatusCreated, 
            Action<Guid> CheckConditionalUpdate);

        TResource Delete(
            TKey resourceKey, 
            Action<TResource> checkPreCondition);

        TResource Get(TKey resourceKey);

        IList<TResource> GetResources(TKey skip, TKey take);

        TResource Post(TResource resourceToPost);

        TResource Put(
            TKey resourceKey, 
            TResource resourceToPut, 
            TResource comparisonResource);

        bool ResourceConflict(TResource sanitizedResource);

        IList<TResource> Resources { get; }
    }
}

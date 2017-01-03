using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CannonicalRESTWebApp
{
    public enum ResourceVersionOption
    {
        New,
        UseExisting
    }
    /// <summary>
    /// The server implementation of the updatedResource class
    /// </summary>
    // TIP: Use DataContract to supply a namespace
    [DataContract(Namespace = "http://microsoft.com/samples/wcf/rest")]
    public class Resource 
    {
        internal Resource() { Version = Guid.NewGuid(); }

        /// <summary>
        /// Creates a sanitized updatedResource based on one supplied by the caller
        /// </summary>
        /// <param name="id">The id of the updatedResource to create</param>
        /// <param name="untrustedResource">The resource supplied by the caller</param>
        /// <returns>A new resource based on the untrusted resource</returns>
        internal static Resource CreateSanitizedResource(
            int id, 
            Resource untrustedResource, 
            ResourceVersionOption versionOption)
        {
            // Return a new sanitized updatedResource with only the changes allowed
            return new Resource()
            {
                Key = id,
                Data = untrustedResource.Data,
                Version = (versionOption==ResourceVersionOption.New) ? Guid.NewGuid() : untrustedResource.Version,
                // ReadOnlyData is not initialized because we 
                // don't allow the caller to update it
            };
        }

        /// <summary>
        /// Updates the updatedResource and version if values have changed
        /// </summary>
        /// <param data="updateResource">The updatedResource you are comparing to</param>
        internal Resource UpdateFrom(Resource updateResource)
        {
            if (IsSameVersionAs(updateResource))
            {
                if (updateResource.IsValid() && DataChanged(updateResource))
                {
                    // Update only the fields you allow
                    // the caller to update
                    this.Data = updateResource.Data;

                    UpdateVersion();
                }
            }

            // Ignore fields you don't want to allow the caller to change
            // Ignore updatedResource.ReadOnlyValue
            // Ignore updatedResource.Version

            return this;
        }

        internal bool IsSameVersionAs(Resource updateResource)
        {
            return this.Version == updateResource.Version;
        }

        internal bool DataChanged(Resource updateResource)
        {
            return this.Data != updateResource.Data;
        }

        // Tip: Provide a method to update the version
        internal void UpdateVersion()
        {
            Version = Guid.NewGuid();
        }

        /// <summary>
        /// Determines if a updatedResource is valid
        /// </summary>
        /// <returns></returns>
        internal bool IsValid()
        {
            // Don't validate ID here - callers can't modify it
            // Only validate things callers can modify
            return !string.IsNullOrWhiteSpace(Data);
        }

        [DataMember]
        internal string Data { get; set; }

        #region Read Only Values

        // HTTP does not define how you should deal with a resource where
        // a portion of the entity is not writable.
        // The generally accepted practice is to ignore fields that
        // cannot be written

        // TIP: Don't be lulled into a false sense of security by the 
        // readonly attribute on this field
        // Deserialization will happily write to it
        // You must protect it with defensive coding
        [DataMember]
        internal readonly string ReadOnlyData = "This is read only data";

        [DataMember]
        internal int Key {get; set;}

        // TIP: Add a version member to support ETags
        // Make the set private so that you can control
        // when version is updated from this class
        [DataMember]
        internal Guid Version { get; private set; }

        #endregion
    }
}
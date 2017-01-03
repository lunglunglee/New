//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Framework.Entity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;

    public partial class FrameworkDataEntities : DbContext
    {
        public FrameworkDataEntities()
            : base("name=FrameworkDataEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<CustomerInfo> CustomerInfoes { get; set; }
        public virtual DbSet<CustomerType> CustomerTypes { get; set; }

        public virtual int CustomerInfoDelete(Nullable<int> iD, Nullable<int> activityID)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CustomerInfoDelete", iDParameter, activityIDParameter);
        }

        public virtual ObjectResult<Nullable<int>> CustomerInfoInsert(string firstName, string middleName, string lastName, Nullable<System.DateTime> birthDate, Nullable<int> customerTypeID, Nullable<int> activityID)
        {
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));

            var middleNameParameter = middleName != null ?
                new ObjectParameter("MiddleName", middleName) :
                new ObjectParameter("MiddleName", typeof(string));

            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));

            var birthDateParameter = birthDate.HasValue ?
                new ObjectParameter("BirthDate", birthDate) :
                new ObjectParameter("BirthDate", typeof(System.DateTime));

            var customerTypeIDParameter = customerTypeID.HasValue ?
                new ObjectParameter("CustomerTypeID", customerTypeID) :
                new ObjectParameter("CustomerTypeID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("CustomerInfoInsert", firstNameParameter, middleNameParameter, lastNameParameter, birthDateParameter, customerTypeIDParameter, activityIDParameter);
        }

        public virtual ObjectResult<Nullable<int>> CustomerInfoUpdate(Nullable<int> iD, string firstName, string middleName, string lastName, Nullable<System.DateTime> birthDate, Nullable<int> customerTypeID, Nullable<int> activityID)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));

            var middleNameParameter = middleName != null ?
                new ObjectParameter("MiddleName", middleName) :
                new ObjectParameter("MiddleName", typeof(string));

            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));

            var birthDateParameter = birthDate.HasValue ?
                new ObjectParameter("BirthDate", birthDate) :
                new ObjectParameter("BirthDate", typeof(System.DateTime));

            var customerTypeIDParameter = customerTypeID.HasValue ?
                new ObjectParameter("CustomerTypeID", customerTypeID) :
                new ObjectParameter("CustomerTypeID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("CustomerInfoUpdate", iDParameter, firstNameParameter, middleNameParameter, lastNameParameter, birthDateParameter, customerTypeIDParameter, activityIDParameter);
        }

        public virtual ObjectResult<Nullable<int>> ActivityQueryflowInsert(Nullable<int> activitySessionflowID, Nullable<System.Guid> flowKey)
        {
            var activitySessionflowIDParameter = activitySessionflowID.HasValue ?
                new ObjectParameter("ActivitySessionflowID", activitySessionflowID) :
                new ObjectParameter("ActivitySessionflowID", typeof(int));

            var flowKeyParameter = flowKey.HasValue ?
                new ObjectParameter("FlowKey", flowKey) :
                new ObjectParameter("FlowKey", typeof(System.Guid));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ActivityQueryflowInsert", activitySessionflowIDParameter, flowKeyParameter);
        }

        public virtual int ActivityQueryflowUpdate(Nullable<int> iD, string identityUserName)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var identityUserNameParameter = identityUserName != null ?
                new ObjectParameter("IdentityUserName", identityUserName) :
                new ObjectParameter("IdentityUserName", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ActivityQueryflowUpdate", iDParameter, identityUserNameParameter);
        }

        public virtual ObjectResult<Nullable<int>> ActivitySessionflowInsert(Nullable<System.Guid> flowKey, string identityUserName, string deviceUUID, string applicationUUID, Nullable<System.Guid> applicationKey)
        {
            var flowKeyParameter = flowKey.HasValue ?
                new ObjectParameter("FlowKey", flowKey) :
                new ObjectParameter("FlowKey", typeof(System.Guid));

            var identityUserNameParameter = identityUserName != null ?
                new ObjectParameter("IdentityUserName", identityUserName) :
                new ObjectParameter("IdentityUserName", typeof(string));

            var deviceUUIDParameter = deviceUUID != null ?
                new ObjectParameter("DeviceUUID", deviceUUID) :
                new ObjectParameter("DeviceUUID", typeof(string));

            var applicationUUIDParameter = applicationUUID != null ?
                new ObjectParameter("ApplicationUUID", applicationUUID) :
                new ObjectParameter("ApplicationUUID", typeof(string));

            var applicationKeyParameter = applicationKey.HasValue ?
                new ObjectParameter("ApplicationKey", applicationKey) :
                new ObjectParameter("ApplicationKey", typeof(System.Guid));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ActivitySessionflowInsert", flowKeyParameter, identityUserNameParameter, deviceUUIDParameter, applicationUUIDParameter, applicationKeyParameter);
        }

        public virtual int ActivitySessionflowUpdate(Nullable<int> iD, Nullable<System.Guid> entityKey, string identityUserName, string sessionflowDataSerialized)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var entityKeyParameter = entityKey.HasValue ?
                new ObjectParameter("EntityKey", entityKey) :
                new ObjectParameter("EntityKey", typeof(System.Guid));

            var identityUserNameParameter = identityUserName != null ?
                new ObjectParameter("IdentityUserName", identityUserName) :
                new ObjectParameter("IdentityUserName", typeof(string));

            var sessionflowDataSerializedParameter = sessionflowDataSerialized != null ?
                new ObjectParameter("SessionflowDataSerialized", sessionflowDataSerialized) :
                new ObjectParameter("SessionflowDataSerialized", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ActivitySessionflowUpdate", iDParameter, entityKeyParameter, identityUserNameParameter, sessionflowDataSerializedParameter);
        }

        public virtual ObjectResult<Nullable<int>> ActivityWorkflowInsert(Nullable<int> activitySessionflowID, Nullable<System.Guid> flowKey, Nullable<System.Guid> flowStepKey, Nullable<System.Guid> entityKey, Nullable<int> parentActivityID)
        {
            var activitySessionflowIDParameter = activitySessionflowID.HasValue ?
                new ObjectParameter("ActivitySessionflowID", activitySessionflowID) :
                new ObjectParameter("ActivitySessionflowID", typeof(int));

            var flowKeyParameter = flowKey.HasValue ?
                new ObjectParameter("FlowKey", flowKey) :
                new ObjectParameter("FlowKey", typeof(System.Guid));

            var flowStepKeyParameter = flowStepKey.HasValue ?
                new ObjectParameter("FlowStepKey", flowStepKey) :
                new ObjectParameter("FlowStepKey", typeof(System.Guid));

            var entityKeyParameter = entityKey.HasValue ?
                new ObjectParameter("EntityKey", entityKey) :
                new ObjectParameter("EntityKey", typeof(System.Guid));

            var parentActivityIDParameter = parentActivityID.HasValue ?
                new ObjectParameter("ParentActivityID", parentActivityID) :
                new ObjectParameter("ParentActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ActivityWorkflowInsert", activitySessionflowIDParameter, flowKeyParameter, flowStepKeyParameter, entityKeyParameter, parentActivityIDParameter);
        }

        public virtual int ActivityWorkflowUpdate(Nullable<int> iD, string identityUserName, Nullable<System.Guid> flowStepKey)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var identityUserNameParameter = identityUserName != null ?
                new ObjectParameter("IdentityUserName", identityUserName) :
                new ObjectParameter("IdentityUserName", typeof(string));

            var flowStepKeyParameter = flowStepKey.HasValue ?
                new ObjectParameter("FlowStepKey", flowStepKey) :
                new ObjectParameter("FlowStepKey", typeof(System.Guid));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ActivityWorkflowUpdate", iDParameter, identityUserNameParameter, flowStepKeyParameter);
        }

        public virtual int AppointmentInfoDelete(Nullable<int> iD, Nullable<int> activityID)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AppointmentInfoDelete", iDParameter, activityIDParameter);
        }

        public virtual ObjectResult<Nullable<int>> AppointmentInfoSave(Nullable<int> eventID, Nullable<int> locationID, Nullable<System.DateTime> beginDate, Nullable<System.DateTime> endDate, Nullable<int> activityID)
        {
            var eventIDParameter = eventID.HasValue ?
                new ObjectParameter("EventID", eventID) :
                new ObjectParameter("EventID", typeof(int));

            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));

            var beginDateParameter = beginDate.HasValue ?
                new ObjectParameter("BeginDate", beginDate) :
                new ObjectParameter("BeginDate", typeof(System.DateTime));

            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("EndDate", endDate) :
                new ObjectParameter("EndDate", typeof(System.DateTime));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("AppointmentInfoSave", eventIDParameter, locationIDParameter, beginDateParameter, endDateParameter, activityIDParameter);
        }

        public virtual int BusinessInfoDelete(Nullable<int> iD, Nullable<int> activityID)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BusinessInfoDelete", iDParameter, activityIDParameter);
        }

        public virtual ObjectResult<Nullable<int>> BusinessInfoSave(Nullable<System.Guid> key, string name, string taxNumber, Nullable<int> activityID)
        {
            var keyParameter = key.HasValue ?
                new ObjectParameter("Key", key) :
                new ObjectParameter("Key", typeof(System.Guid));

            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));

            var taxNumberParameter = taxNumber != null ?
                new ObjectParameter("TaxNumber", taxNumber) :
                new ObjectParameter("TaxNumber", typeof(string));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("BusinessInfoSave", keyParameter, nameParameter, taxNumberParameter, activityIDParameter);
        }

        public virtual int EventCoordinatorDelete(Nullable<int> iD, Nullable<int> activityID)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EventCoordinatorDelete", iDParameter, activityIDParameter);
        }

        public virtual ObjectResult<Nullable<int>> EventCoordinatorInsert(Nullable<int> eventID, Nullable<int> entityID, Nullable<int> requestedByID, Nullable<int> activityID)
        {
            var eventIDParameter = eventID.HasValue ?
                new ObjectParameter("EventID", eventID) :
                new ObjectParameter("EventID", typeof(int));

            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));

            var requestedByIDParameter = requestedByID.HasValue ?
                new ObjectParameter("RequestedByID", requestedByID) :
                new ObjectParameter("RequestedByID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("EventCoordinatorInsert", eventIDParameter, entityIDParameter, requestedByIDParameter, activityIDParameter);
        }

        public virtual int EventDetailDelete(Nullable<int> iD, Nullable<int> activityID)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EventDetailDelete", iDParameter, activityIDParameter);
        }

        public virtual ObjectResult<Nullable<int>> EventDetailSave(Nullable<int> eventID, Nullable<int> detailTypeID, string description, Nullable<int> activityID)
        {
            var eventIDParameter = eventID.HasValue ?
                new ObjectParameter("EventID", eventID) :
                new ObjectParameter("EventID", typeof(int));

            var detailTypeIDParameter = detailTypeID.HasValue ?
                new ObjectParameter("DetailTypeID", detailTypeID) :
                new ObjectParameter("DetailTypeID", typeof(int));

            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("EventDetailSave", eventIDParameter, detailTypeIDParameter, descriptionParameter, activityIDParameter);
        }

        public virtual int EventInfoDelete(Nullable<int> iD, Nullable<int> activityID)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EventInfoDelete", iDParameter, activityIDParameter);
        }

        public virtual ObjectResult<Nullable<int>> EventInfoSave(Nullable<System.Guid> key, Nullable<int> eventGroupID, Nullable<int> eventTypeID, Nullable<int> creatorID, string name, string description, string slogan, Nullable<int> activityID)
        {
            var keyParameter = key.HasValue ?
                new ObjectParameter("Key", key) :
                new ObjectParameter("Key", typeof(System.Guid));

            var eventGroupIDParameter = eventGroupID.HasValue ?
                new ObjectParameter("EventGroupID", eventGroupID) :
                new ObjectParameter("EventGroupID", typeof(int));

            var eventTypeIDParameter = eventTypeID.HasValue ?
                new ObjectParameter("EventTypeID", eventTypeID) :
                new ObjectParameter("EventTypeID", typeof(int));

            var creatorIDParameter = creatorID.HasValue ?
                new ObjectParameter("CreatorID", creatorID) :
                new ObjectParameter("CreatorID", typeof(int));

            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));

            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));

            var sloganParameter = slogan != null ?
                new ObjectParameter("Slogan", slogan) :
                new ObjectParameter("Slogan", typeof(string));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("EventInfoSave", keyParameter, eventGroupIDParameter, eventTypeIDParameter, creatorIDParameter, nameParameter, descriptionParameter, sloganParameter, activityIDParameter);
        }

        public virtual int GovernmentInfoDelete(Nullable<int> iD, Nullable<int> activityID)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GovernmentInfoDelete", iDParameter, activityIDParameter);
        }

        public virtual ObjectResult<Nullable<int>> GovernmentInfoSave(Nullable<System.Guid> key, string name, Nullable<int> locationID, Nullable<int> activityID)
        {
            var keyParameter = key.HasValue ?
                new ObjectParameter("Key", key) :
                new ObjectParameter("Key", typeof(System.Guid));

            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));

            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("GovernmentInfoSave", keyParameter, nameParameter, locationIDParameter, activityIDParameter);
        }

        public virtual int PersonInfoDelete(Nullable<int> iD, Nullable<int> activityID)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PersonInfoDelete", iDParameter, activityIDParameter);
        }

        public virtual ObjectResult<Nullable<int>> PersonInfoSave(Nullable<System.Guid> key, string firstName, string middleName, string lastName, Nullable<System.DateTime> birthDate, Nullable<int> activityID)
        {
            var keyParameter = key.HasValue ?
                new ObjectParameter("Key", key) :
                new ObjectParameter("Key", typeof(System.Guid));

            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));

            var middleNameParameter = middleName != null ?
                new ObjectParameter("MiddleName", middleName) :
                new ObjectParameter("MiddleName", typeof(string));

            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));

            var birthDateParameter = birthDate.HasValue ?
                new ObjectParameter("BirthDate", birthDate) :
                new ObjectParameter("BirthDate", typeof(System.DateTime));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("PersonInfoSave", keyParameter, firstNameParameter, middleNameParameter, lastNameParameter, birthDateParameter, activityIDParameter);
        }

        public virtual int PropertyEntityDelete(Nullable<int> iD, Nullable<int> activityID)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PropertyEntityDelete", iDParameter, activityIDParameter);
        }

        public virtual ObjectResult<Nullable<int>> PropertyEntitySave(Nullable<System.Guid> entityKey, Nullable<System.Guid> propertyKey, Nullable<int> activityID)
        {
            var entityKeyParameter = entityKey.HasValue ?
                new ObjectParameter("EntityKey", entityKey) :
                new ObjectParameter("EntityKey", typeof(System.Guid));

            var propertyKeyParameter = propertyKey.HasValue ?
                new ObjectParameter("PropertyKey", propertyKey) :
                new ObjectParameter("PropertyKey", typeof(System.Guid));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("PropertyEntitySave", entityKeyParameter, propertyKeyParameter, activityIDParameter);
        }

        public virtual int CustomerInfoDelete1(Nullable<int> iD, Nullable<int> activityID)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CustomerInfoDelete1", iDParameter, activityIDParameter);
        }

        public virtual ObjectResult<Nullable<int>> CustomerInfoInsert1(string firstName, string middleName, string lastName, Nullable<System.DateTime> birthDate, Nullable<int> customerTypeID, Nullable<int> activityID)
        {
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));

            var middleNameParameter = middleName != null ?
                new ObjectParameter("MiddleName", middleName) :
                new ObjectParameter("MiddleName", typeof(string));

            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));

            var birthDateParameter = birthDate.HasValue ?
                new ObjectParameter("BirthDate", birthDate) :
                new ObjectParameter("BirthDate", typeof(System.DateTime));

            var customerTypeIDParameter = customerTypeID.HasValue ?
                new ObjectParameter("CustomerTypeID", customerTypeID) :
                new ObjectParameter("CustomerTypeID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("CustomerInfoInsert1", firstNameParameter, middleNameParameter, lastNameParameter, birthDateParameter, customerTypeIDParameter, activityIDParameter);
        }

        public virtual ObjectResult<Nullable<int>> CustomerInfoUpdate1(Nullable<int> iD, string firstName, string middleName, string lastName, Nullable<System.DateTime> birthDate, Nullable<int> customerTypeID, Nullable<int> activityID)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));

            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));

            var middleNameParameter = middleName != null ?
                new ObjectParameter("MiddleName", middleName) :
                new ObjectParameter("MiddleName", typeof(string));

            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));

            var birthDateParameter = birthDate.HasValue ?
                new ObjectParameter("BirthDate", birthDate) :
                new ObjectParameter("BirthDate", typeof(System.DateTime));

            var customerTypeIDParameter = customerTypeID.HasValue ?
                new ObjectParameter("CustomerTypeID", customerTypeID) :
                new ObjectParameter("CustomerTypeID", typeof(int));

            var activityIDParameter = activityID.HasValue ?
                new ObjectParameter("ActivityID", activityID) :
                new ObjectParameter("ActivityID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("CustomerInfoUpdate1", iDParameter, firstNameParameter, middleNameParameter, lastNameParameter, birthDateParameter, customerTypeIDParameter, activityIDParameter);
        }
    }
}

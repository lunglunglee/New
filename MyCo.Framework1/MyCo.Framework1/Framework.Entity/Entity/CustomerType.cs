//-----------------------------------------------------------------------
// <copyright file="CustomerType.cs" company="Genesys Source">
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Foundation.Data;
using Framework.Entity;
using Genesys.Foundation.Entity;
using System.Linq;

namespace Framework.Entity
{
    /// <summary>
    /// EntityCustomer
    /// </summary>
    [CLSCompliant(true), ConnectionString("MyCodeConnection")]
    public partial class CustomerType : ReadOnlyEntity<CustomerType>, ICustomerType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerType()
            : base()
        {
        }

        /// <summary>
        /// Gets all records that exactly equal passed name
        /// </summary>
        /// <param name="name">Value to search CustomerTypeName field </param>
        /// <returns>All records matching the passed name</returns>
        public static IQueryable<CustomerType> GetByName(string name)
        {
            DatabaseContext dbContext = new DatabaseContext();
            IQueryable<CustomerType> returnValue = dbContext.EntityData.Where(x => x.Name == name);
            return returnValue;
        }
    }
}

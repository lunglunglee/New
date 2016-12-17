
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReSTAuthLibrary
{
    /// <summary>
    /// Sample AppUser class
    /// </summary>
    public class AppUser
    {
        public AppUser(string name, string password)
        {
            this.UserName = name;
            this.Password = password;
        }

        public string UserName {get; set;}
        public string Password { get; set; }
    }

    public class AppUserHelper
    {
        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="objAppUser"></param>
        /// <returns></returns>
        public static bool ValidateUser(AppUser objAppUser)
        {
            // Your custom logic to validate user goes here
            return !(objAppUser != null) || (objAppUser.UserName.Equals("kamal") && objAppUser.Password.Equals("password"));            
        }
    }
}



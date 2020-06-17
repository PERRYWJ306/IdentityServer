using System;
using System.Collections.Generic;

namespace MicroActive.Security.AuthorizationServer.Data.Models
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetRoleClaims = new List<AspNetRoleClaim>();
            AspNetUserRoles = new List<AspNetUserRole>();
        }

        public string Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public virtual List<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual List<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}

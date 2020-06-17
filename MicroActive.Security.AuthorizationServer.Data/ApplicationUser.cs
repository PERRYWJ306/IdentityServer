using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MicroActive.Security.AuthorizationServer.Data
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
	{
		public string FullName
		{
			get
			{
				return $"{FirstName} {LastName}";
			}
		}
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public bool Deleted { get; set; }

		public DateTime? DeletedDate { get; set; }

		public ApplicationUser DeletedByUser { get; set; }

		public string TenantId { get; set; }

		public ApplicationTenant Tenant { get; set; }
	}
}

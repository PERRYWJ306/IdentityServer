using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroActive.Security.AuthorizationServer.Web
{
	public class ClaimsAuthAttribute : AuthorizeAttribute
	{
		private string claimType;
		private string claimValue;
		public ClaimsAuthAttribute(string type, string value)
		{
			this.claimType = type;
			this.claimValue = value;
		}

		
		//public override void OnAuthorization(AuthorizationContext filterContext)
		//{
		//	var user = filterContext.HttpContext.User as ClaimsPrincipal;
		//	if (user != null && user.HasClaim(claimType, claimValue))
		//	{
		//		base.OnAuthorization(filterContext);
		//	}
		//	else
		//	{
		//		base.HandleUnauthorizedRequest(filterContext);
		//	}
		//}
	}
}

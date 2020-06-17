using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using MicroActive.Security.AuthorizationServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace MicroActive.Security.AuthorizationServer.Web.Identity
{
	public class CustomProfileService : IProfileService
	{
		private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
		private readonly UserManager<ApplicationUser> _userManager;

		public CustomProfileService(UserManager<ApplicationUser> userManager,
			IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
		{
			_userManager = userManager;
			_claimsFactory = claimsFactory;
		}

		// not virtual or abstract, therefore not overridable
		public async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			var sub = context.Subject.FindFirst("sub");

			var user = await _userManager.FindByIdAsync(sub.Value);
			var principal = await _claimsFactory.CreateAsync(user);

			var claims = principal.Claims.ToList();
//			if (!context.)
//			{
				claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
//			}

			// Add User Properties
			claims.Add(new System.Security.Claims.Claim(StandardScopes.Email, user.Email));
			claims.Add(new System.Security.Claims.Claim("full_name", user.FullName));

			context.IssuedClaims = claims;
		}

		public async Task IsActiveAsync(IsActiveContext context)
		{
			var sub = context.Subject.FindFirst("sub");
			var user = await _userManager.FindByIdAsync(sub.Value);
			context.IsActive = user != null;
		}
	}
}

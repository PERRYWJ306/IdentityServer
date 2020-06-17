using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MicroActive.Security.AuthorizationServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroActive.Security.AuthorizationServer.Web.Identity
{
	public class CustomUserClaimsFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
	{
		public CustomUserClaimsFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
		{
			Options.ClaimsIdentity.RoleClaimType = "roleaction";
		}
		
		public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
		{
			var principal = await base.CreateAsync(user);
			var usr = await UserManager.GetUserAsync(principal);

			var identity = principal.Identities.First();

			var username = await UserManager.GetUserNameAsync(user);
			var usernameClaim = identity.FindFirst(claim => claim.Type == Options.ClaimsIdentity.UserNameClaimType && claim.Value == username);
			if (usernameClaim != null)
			{
				identity.RemoveClaim(usernameClaim);
				identity.AddClaim(new Claim(JwtClaimTypes.PreferredUserName, username));
			}

			if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Name))
			{
				identity.AddClaim(new Claim(JwtClaimTypes.Name, username));
			}

			if (UserManager.SupportsUserEmail)
			{
				var email = await UserManager.GetEmailAsync(user);
				if (!String.IsNullOrWhiteSpace(email))
				{
					identity.AddClaims(new[]
					{
						new Claim(JwtClaimTypes.Email, email),
						new Claim(JwtClaimTypes.EmailVerified,
							await UserManager.IsEmailConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)
					});
				}
			}

			if (UserManager.SupportsUserPhoneNumber)
			{
				var phoneNumber = await UserManager.GetPhoneNumberAsync(user);
				if (!String.IsNullOrWhiteSpace(phoneNumber))
				{
					identity.AddClaims(new[]
					{
						new Claim(JwtClaimTypes.PhoneNumber, phoneNumber),
						new Claim(JwtClaimTypes.PhoneNumberVerified,
							await UserManager.IsPhoneNumberConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)
					});
				}
			}
			//var xy = Options.ClaimsIdentity.RoleClaimType;
			//if (RoleManager.SupportsRoleClaims)
			//{
			//	foreach(var role in await UserManager.GetRolesAsync(user))
			//	{
			//		var r = await RoleManager.FindByNameAsync(role);
			//		var xyz = await RoleManager.AddClaimAsync(r, new Claim("permission", "IdentityMgr.CreateUser"));
			//		if (r != null)
			//		{
			//			identity.AddClaims(await RoleManager.GetClaimsAsync(r));
			//		}
			//	}
			//}

			return principal;
		}
	}
}

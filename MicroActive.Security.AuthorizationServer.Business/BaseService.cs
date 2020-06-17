using MicroActive.Security.AuthorizationServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MicroActive.Security.AuthorizationServer.Business
{
	public abstract class BaseService
	{
		protected IdentityContext _context = null;

		public BaseService(IdentityContext context)
		{
			_context = context;
		}
		//TODO: This breaks the separation between business and the web, but for now its fine.Later I can create a custom Context class and have the caller(webapi) set it upon initilaization of the service.
		protected ClaimsPrincipal User
		{
			get
			{
				return new System.Security.Claims.ClaimsPrincipal();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using MicroActive.Security.AuthorizationServer.Business.Models;
using Microsoft.AspNetCore.Mvc;
using MicroActive.Security.AuthorizationServer.Data.Models;

namespace MicroActive.Security.AuthorizationServer.Web.Controllers
{
	public class BaseController: Controller
	{
		protected IdentityContext _context = null;
		public BaseController(IdentityContext context)
		{
			_context = context;
		}
		//protected IHttpActionResult BadRequest(BaseResponse resp)
		//{
		//	if (resp.Error != null)
		//	{				
		//		return this.InternalServerError(resp.Error);
		//	}
		//	else
		//	{
		//		return BadRequest(resp.ErrorMessage);
		//	}
		//}
	}
}
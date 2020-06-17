using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


using AutoMapper;

using MicroActive.Security.AuthorizationServer.Business;
using MicroActive.Security.AuthorizationServer.Web.Models;
using MicroActive.Security.AuthorizationServer.Data.Models;
using MicroActive.Security.AuthorizationServer.Data;

namespace MicroActive.Security.AuthorizationServer.Web.Controllers
{
	[SecurityHeaders]
	[Route("api/[controller]")]
	public class UsersController : BaseController
	{
		private readonly IMapper _mapper;
		private UserManager<ApplicationUser> _manager = null;

		public UsersController(IdentityContext context, IMapper mapper, UserManager<ApplicationUser> manager) : base(context)
		{
			_mapper = mapper;
			userSvc = new UsersService(context);
			_manager = manager;
		}

		#region Fields
		private UsersService userSvc = null;
		#endregion

		#region Public methods
		[Authorize(Roles = "IdentityManagement.ViewUsers")]
		[HttpGet]
		public IActionResult Get(bool enabledOnly = true, [FromQuery] params string[] ids)
		{
			var resp = userSvc.GetUsers(ids);

			if (resp.IsValid)
			{
				if (resp.Users.Any())
				{
					List<UserModel> userModels = resp.Users.Select(s => Mapper.Map<AspNetUser, UserModel>(s)).ToList();

					return Ok(userModels);
				}
				else
				{
					//Doing this because Im not sure if the mapper will blow up if the collection is empty, so just return an empty OK
					return Ok();
				}
			}
			else
			{
				return BadRequest(resp);
			}
		}


		//[Authorize(Roles = "IdentityManagement.ViewUsers")]
		//[HttpGet]
		//public IActionResult Get(string id, string email = null, bool enabledOnly = true)
		//{
		//	var resp = userSvc.GetUser(u => u.Id == id || u.Email.ToLower() == email.ToLower());

		//	if (resp.IsValid)
		//	{
		//		if (resp.User != null)
		//		{
		//			UserModel roleModel = Mapper.Map<AspNetUser, UserModel>(resp.User);
		//			return Ok(roleModel);
		//		}
		//		else
		//		{
		//			return Ok();
		//		}
		//	}
		//	else
		//	{
		//		return BadRequest(resp);
		//	}

		//}

		[Authorize(Roles = "IdentityManagement.CreateUser")]
		[HttpPost]
		public async Task<IActionResult> Post([FromBody]UserModel value)
		{
			var user = Mapper.Map<UserModel, AspNetUser>(value);			

			var resp = await userSvc.CreateUser(user, _manager, value.Pwd);

			if (resp.IsValid)
			{
				return Ok(Mapper.Map<AspNetUser, UserModel>(resp.User));
			}
			else
			{
				return BadRequest(resp);
			}
		}

		[Authorize(Roles = "IdentityManagement.UpdateUser")]
		[HttpPut]
		public IActionResult Put(string id, [FromBody]UserModel value)
		{
			var user = Mapper.Map<UserModel, AspNetUser>(value);
			user.Id = id;

			var resp = userSvc.UpdateUser(user);

			if (resp.IsValid)
			{
				return Ok(Mapper.Map<AspNetUser, UserModel>(resp.User));
			}
			else
			{
				return BadRequest(resp);
			}
		}

		[Authorize(Roles = "IdentityManagement.DeleteUser")]
		[HttpDelete]
		public async Task<IActionResult> Delete(string id)
		{
			
			var resp = await userSvc.DeleteUser(id, _manager, await _manager.GetUserAsync(this.User));

			if (resp.IsValid)
			{
				return Ok();
			}
			else
			{
				return BadRequest(resp);
			}
		}
		#endregion

		#region Helpers



		#endregion

	}
}

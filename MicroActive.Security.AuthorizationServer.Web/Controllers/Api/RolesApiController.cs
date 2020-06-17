using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using MicroActive.Security.AuthorizationServer.Business;
using MicroActive.Security.AuthorizationServer.Web.Models;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MicroActive.Security.AuthorizationServer.Data.Models;

namespace MicroActive.Security.AuthorizationServer.Web.Controllers
{
	[SecurityHeaders]
	[Route("api/[controller]")]
	public class RolesController : BaseController
    {
		private readonly IMapper _mapper;

		public RolesController(IdentityContext context, IMapper mapper) : base(context)
		{
			_mapper = mapper;
			roleSvc = new RoleService(context);

		}
		#region Fields
		private RoleService roleSvc = null;
		#endregion

		#region Public methods
		// GET: api/Roles
		[Authorize(Roles = "IdentityManagement.ViewRoles")]//(RoleActions = "IdentityManagement.ViewRoles")
		[HttpGet]
		public IActionResult Get(bool enabledOnly = true, [FromQuery] params string[] ids)
        {
			var resp = roleSvc.GetRoles(ids);
			
			if (resp.IsValid)
			{
				if (resp.Roles.Any())
				{
					List<RoleModel> roleModels = resp.Roles.Select(s => Mapper.Map<AspNetRole, RoleModel>(s)).ToList();

					return Ok(roleModels);
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


		//[Authorize(Roles = "IdentityManagement.ViewRoles")]
		//[HttpGet]
		//public IActionResult Get(string id, bool enabledOnly = true)
		//{
		//	var resp = roleSvc.GetRole(id);

		//	if (resp.IsValid)
		//	{
		//		if (resp.Role != null)
		//		{
		//			RoleModel roleModel = Mapper.Map<AspNetRole, RoleModel>(resp.Role);
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

		[Authorize(Roles = "IdentityManagement.CreateRole")]
		[HttpPost]
		public IActionResult Post([FromBody]RoleModel value)
        {
			var role = Mapper.Map<RoleModel, AspNetRole>(value);

			var resp = roleSvc.CreateRole(role);
			
			if (resp.IsValid)
			{
				return Ok();
			}
			else
			{
				return BadRequest(resp);
			}
		}

		[Authorize(Roles = "IdentityManagement.CreateRole")]
		[HttpPut]
		public IActionResult Put(string id, [FromBody]RoleModel value)
        {
			var role = Mapper.Map<RoleModel, AspNetRole>(value);
			role.Id = id;

			var resp = roleSvc.UpdateRole(role);

			if (resp.IsValid)
			{
				return Ok();
			}
			else
			{
				return BadRequest(resp);
			}
		}

		[Authorize(Roles = "IdentityManagement.DeleteRoles")]
		[HttpDelete]
		public IActionResult Delete(string id)
        {
			var resp = roleSvc.DeleteRole(id);

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
		//private RoleModel CopyRoleToModel(bool enabledOnly, AspNetRole role)
		//{
		//	return new RoleModel
		//	{
		//		Id = role.Id,
		//		Name = role.Name,
		//		Actions = role.AspNetRoleActions.Where(w => w.Enabled && ((enabledOnly && w.Action.Enabled) || (!enabledOnly))).Select(s => new ActionModel
		//		{
		//			ClientId = s.Action.ClientId,
		//			Domain = s.Action.Domain,
		//			Enabled = s.Action.Enabled,
		//			Id = s.ActionId,
		//			Name = s.Action.Name,
		//			Deny = s.Deny
		//		}).ToList()
		//	};
		//}

		#endregion

	}
}

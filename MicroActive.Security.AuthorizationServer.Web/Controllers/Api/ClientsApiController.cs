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
	public class ClientsController : BaseController
    {
		private ClientService svc = null;
		private readonly IMapper _mapper;
		private UserManager<ApplicationUser> _manager = null;

		public ClientsController(IdentityContext context, IMapper mapper, UserManager<ApplicationUser> manager) : base(context)
		{
			_mapper = mapper;
			svc = new ClientService(context);
			_manager = manager;
		}


		// GET: api/Clients
		//[Auth(RoleActions = "IdentityManagement.ViewClients")]
		[Authorize(Roles = "IdentityManagement.ViewClients")]
		[HttpGet]
		public IActionResult Get([FromQuery] params int[] ids)
        {
			var resp = svc.GetClients(ids);
			
			if (resp.IsValid)
			{
				if (!resp.Clients.IsEmpty())
				{
					List<ClientModel> clients = resp.Clients.Select(s => Mapper.Map<Client, ClientModel>(s)).ToList();
					return Ok(clients.AsQueryable());
				}
				else
				{

					return Ok();
				}
			}
			else
			{
				return BadRequest(resp.ErrorMessage);
			}
		}

		//// GET: api/Clients/5
		//[Authorize(Roles = "IdentityManagement.ViewClients")]
		//[HttpGet]
		//public IActionResult Get(int id)
  //      {
		//	var resp = svc.GetClient(id);

		//	if (resp.IsValid)
		//	{
		//		if (resp.Client != null)
		//		{					
		//			return Ok(Mapper.Map<Client, ClientModel>(resp.Client));
		//		}
		//		else
		//		{

		//			return Ok();
		//		}
		//	}
		//	else
		//	{
		//		return BadRequest(resp.ErrorMessage);
		//	}
		//}

		// POST: api/Clients
		[Authorize(Roles = "IdentityManagement.CreateClients")]
		[HttpPost]
		public IActionResult Post([FromBody]ClientModel value)
        {
			var client = Mapper.Map<ClientModel, Client>(value);

			var resp = svc.CreateClient(client);

			if (resp.IsValid)
			{
				return Ok();
			}
			else
			{
				return BadRequest(resp.ErrorMessage);
			}

		}

		// PUT: api/Clients/5
		[Authorize(Roles = "IdentityManagement.UpdateClients")]
		[HttpPut]
		public IActionResult Put(int id, [FromBody]ClientModel value)
        {
			var client = Mapper.Map<ClientModel, Client>(value);

			client.Id = id;

			var resp = svc.UpdateClient(client);

			if (resp.IsValid)
			{
				return Ok();
			}
			else
			{
				return BadRequest(resp.ErrorMessage);
			}
		}

		// DELETE: api/Clients/5
		[Authorize(Roles = "IdentityManagement.DeleteClient")]
		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
        {
			var resp = svc.DeleteClient(id, await _manager.GetUserAsync(this.User));

			if (resp.IsValid)
			{
				return Ok();
			}
			else
			{
				return BadRequest(resp.ErrorMessage);
			}
		}
    }
}

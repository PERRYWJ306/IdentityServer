using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MicroActive.Security.AuthorizationServer.Data;
using MicroActive.Security.AuthorizationServer.Data.Models;
using MicroActive.Security.AuthorizationServer.Data.Repositories;
using MicroActive.Security.AuthorizationServer.Business.Models.Roles;
using MicroActive.Security.AuthorizationServer.Business.Models;

namespace MicroActive.Security.AuthorizationServer.Business
{
	public class RoleService : BaseService
	{
		private DataRepository<AspNetRole> roleRepo = null;

		public RoleService(IdentityContext context) : base(context)
		{
			roleRepo = new DataRepository<AspNetRole>(context);
		}


		public GetRoleResponse GetRole(string id)
		{
			var resp = new GetRoleResponse();

			if (string.IsNullOrEmpty(id))
			{
				resp.IsValid = false; //redundant
				resp.ErrorMessage = "id must be specified.";
			}
			else
			{
				var role = roleRepo.Filter(f => f.Id == id, "AspNetRoleActions.Action").FirstOrDefault();
				if (role == null)
				{
					resp.ErrorMessage = "id not found";
				}
				else
				{
					resp.IsValid = true;
					resp.Role = role;
				}
			}

			return resp;
		}
		//System.Web.HttpContext.Current.User
		public GetRoleResponse GetRoles(params string[] ids)
		{
			var resp = new GetRoleResponse();
			var list = new List<AspNetRole>();

			if (ids == null || ids.Length == 0)
			{
				list = roleRepo.All("AspNetRoleActions.Action").ToList();
			}
			else
			{
				list = roleRepo.Filter(f => ids.Contains(f.Id), "AspNetRoleActions.Action").ToList();
			}

			if (!list.Any())
			{
				resp.ErrorMessage = "ids not found";
			}
			else
			{
				resp.IsValid = true;
				resp.Roles = list;
			}

			return resp;
		}

		public BaseResponse CreateRole(AspNetRole role)
		{
			var resp = new BaseResponse();

			if (role == null)
			{
				resp.IsValid = false; //redundant
				resp.ErrorMessage = "role is invalid.";
			}
			else
			{
				//validate it doesnt already exist
				var tstRole = roleRepo.Filter(f => f.Name.ToLower() == role.Name.ToLower()).FirstOrDefault();
				if (tstRole == null)
				{
					//good
					try
					{
						//using (var ts = new TransactionScope())
						//{
						var r = new AspNetRole();
						r.Name = role.Name;
						r.Id = Guid.NewGuid().ToString();

						//							var newR = roleRepo.Create(r);

						foreach (var ra in role.AspNetRoleClaims)
						{
							r.AspNetRoleClaims.Add(new AspNetRoleClaim() { ClaimType = ra.ClaimType, ClaimValue = ra.ClaimValue });
						}

						roleRepo.Create(r);
						//							ts.Complete();
						resp.IsValid = true;
						//						}
					}
					catch (Exception e)
					{
						resp.Error = e;
					}
				}
				else
				{
					//bad
					resp.IsValid = false;
					resp.ErrorMessage = "Role already exists with the same name";
				}
			}

			return resp;
		}

		public BaseResponse DeleteRole(string id)
		{
			var resp = new BaseResponse();

			if (string.IsNullOrEmpty(id))
			{
				resp.IsValid = false; //redundant
				resp.ErrorMessage = "No role specified.";
			}
			else
			{
				//validate it doesnt already exist
				var tstRole = roleRepo.Filter(f => f.Id == id).FirstOrDefault();
				if (tstRole != null)
				{
					//good
					try
					{
						roleRepo.Delete(tstRole, false);
						resp.IsValid = true;
					}
					catch (Exception e)
					{
						resp.Error = e;
					}
				}
				else
				{
					//bad
					resp.IsValid = false;
					resp.ErrorMessage = "Role does not exist";
				}
			}

			return resp;
		}

		public GetRoleResponse UpdateRole(AspNetRole role)
		{
			var resp = new GetRoleResponse();

			if (role == null || string.IsNullOrEmpty(role.Id) || string.IsNullOrEmpty(role.Name))
			{
				resp.IsValid = false; //redundant
				resp.ErrorMessage = "role is invalid. Please validate fields";
			}
			else
			{
				//validate it doesnt already exist
				var tstRole = roleRepo.Filter(f => f.Id == role.Id).FirstOrDefault();
				if (tstRole != null)
				{
					//good
					try
					{
						//using (var ts = new TransactionScope())
						//{
						tstRole.Name = role.Name;
						roleRepo.Update(tstRole);

						//TODO: Instead of deleting all existing, just delete from tst where ID's dont match, then match ID's, to update, then add where tst.actions not in role.actions
						if (tstRole.AspNetRoleClaims.Count != role.AspNetRoleClaims.Count)
						{
							int cnt = tstRole.AspNetRoleClaims.Count;
							for (int i = tstRole.AspNetRoleClaims.Count - 1; i > -1; i--)
							{
								roleRepo.Delete(tstRole.AspNetRoleClaims[i]);
							}

							foreach (var ra in role.AspNetRoleClaims)
							{
								tstRole.AspNetRoleClaims.Add(new AspNetRoleClaim() { ClaimType = ra.ClaimType, ClaimValue = ra.ClaimValue });

							}
						}

						roleRepo.Update(tstRole);
						//							ts.Complete();
						resp.IsValid = true;
						//						}
					}
					catch (Exception e)
					{
						resp.Error = e;
					}
				}
				else
				{
					//bad
					resp.IsValid = false;
					resp.ErrorMessage = "Role already exists with the same name";
				}
			}

			return resp;
		}
	}
}

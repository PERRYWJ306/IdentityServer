using System;
using System.Collections.Generic;
using System.Linq;

using MicroActive.Security.AuthorizationServer.Data;
using MicroActive.Security.AuthorizationServer.Data.Repositories;
using MicroActive.Security.AuthorizationServer.Business.Models.Users;
using MicroActive.Security.AuthorizationServer.Business.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MicroActive.Security.AuthorizationServer.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace MicroActive.Security.AuthorizationServer.Business
{
	public class UsersService : BaseService
	{
		private DataRepository<AspNetUser> userRepo = null;

		public UsersService(IdentityContext context) : base(context)
		{
			userRepo = new DataRepository<AspNetUser>(context);
		}

		/// <summary>
		/// This will allow the caller to specify user name, email or first name as needed.
		/// </summary>
		/// <param name="u"></param>
		/// <returns></returns>
		public GetUserResponse GetUser(Expression<Func<AspNetUser, bool>> u)
		{
			var resp = new GetUserResponse();

			if (u == null)
			{
				resp.IsValid = false; //redundant
				resp.ErrorMessage = "id must be specified.";
			}
			else
			{
				var user = userRepo.Filter(u, "AspNetRoles.AspNetRolesClaims").FirstOrDefault();

				if (user == null)
				{
					resp.ErrorMessage = "id not found";
				}
				else
				{
					resp.IsValid = true;
					resp.User = user;
				}
			}

			return resp;
		}

		public GetUserResponse GetUsers(params string[] ids)
		{
			var resp = new GetUserResponse();
			var list = new List<AspNetUser>();

			if (ids.IsEmpty())
			{
				list = userRepo.All(nameof(AspNetUser.AspNetUserRoles)).ToList();
			}
			else
			{
				list = userRepo.Filter(f => ids.Contains(f.Id), nameof(AspNetUser.AspNetUserRoles)).ToList();
			}

			if (!list.Any())
			{
				resp.ErrorMessage = "ids not found";
			}
			else
			{
				resp.IsValid = true;
				resp.Users = list;
			}

			return resp;
		}

		public async Task<GetUserResponse> CreateUser(AspNetUser user, UserManager<ApplicationUser> manager, string password)
		{
			var resp = new GetUserResponse();
			
			if (user == null || !string.IsNullOrEmpty(user.Id) || string.IsNullOrEmpty(user.UserName))
			{
				resp.IsValid = false; //redundant
				resp.ErrorMessage = "user is invalid or already exists";
			}
			else
			{
				//validate it doesnt already exist
				var tstUser = userRepo.Filter(f => f.Email == user.Email).FirstOrDefault();
				if (tstUser == null)
				{
					var newUser = new ApplicationUser()
					{
						Email = user.Email,
						FirstName = user.FirstName,
						LastName = user.LastName,
						PhoneNumber = user.PhoneNumber,
						UserName = user.Email
					};

					var result = await manager.CreateAsync(newUser, password);

					if (result.Succeeded)
					{
						//tstUser.AspNetUserRoles = user.AspNetUserRoles;
						foreach (var role in user.AspNetUserRoles)
						{
							await manager.AddToRoleAsync(newUser, role.Role.Name);
						}
						//						UpdateUserRoles(tstUser);

						tstUser = userRepo.Filter(f => f.Email == user.Email).FirstOrDefault();

						resp.IsValid = true;
						resp.User = tstUser;
					}
					else
					{
						resp.ErrorMessage = "there was an error creating the User.";
					}
				}
				else
				{
					resp.ErrorMessage = "user is invalid or already exists";
				}
			}

			return resp;
		}

		//TODO:Extract current user to a context object so that each service/method can access it if needed.
		public async Task<BaseResponse> DeleteUser(string id, UserManager<ApplicationUser> manager, ApplicationUser currentUser)
		{
			var resp = new BaseResponse();

			if (string.IsNullOrEmpty(id))
			{
				resp.IsValid = false; //redundant
				resp.ErrorMessage = "No role specified.";
			}
			else
			{
				try
				{
					//TODO: Move this to a custom principal
					var delBy = currentUser;
					var user = await manager.FindByIdAsync(id);

					if (!delBy.Deleted && !user.Deleted)
					{
						//TODO: Need to move over ApplicationUserManager
						//await manager.DeleteAsync(user);//, delBy);
					}

					resp.IsValid = true;
				}
				catch (Exception e)
				{
					resp.Error = e;
				}
			}

			return resp;
		}

		public GetUserResponse UpdateUser(AspNetUser user)
		{
			var resp = new GetUserResponse();

			if (user == null || string.IsNullOrEmpty(user.Id) || string.IsNullOrEmpty(user.UserName))
			{
				resp.IsValid = false; //redundant
				resp.ErrorMessage = "user is invalid. Please validate fields";
			}
			else
			{
				//validate it doesnt already exist
				var tstUser = userRepo.Filter(f => f.Id == user.Id).FirstOrDefault();
				if (tstUser != null)
				{
					//good
					try
					{
						//tstUser.Email = tstUser.UserName = user.UserName;
						tstUser.FirstName = user.FirstName;
						tstUser.LastName = user.LastName;
						tstUser.LockoutEnd = user.LockoutEnd;
						tstUser.PhoneNumber = user.PhoneNumber;

						userRepo.Update(tstUser);

						//TODO: Add remove rolls

						userRepo.Update(tstUser);

						resp.User = tstUser;
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
					resp.ErrorMessage = "User already exists with the same name";
				}
			}

			return resp;
		}

		public GetUserResponse UpdateUserEmail(ApplicationUser user, string newEmail, UserManager<ApplicationUser> manager)
		{
			var resp = new GetUserResponse();

			if (user == null || string.IsNullOrEmpty(user.Id) || string.IsNullOrEmpty(user.UserName))
			{
				resp.IsValid = false; //redundant
				resp.ErrorMessage = "user is invalid. Please validate fields";
			}
			else
			{
				//validate it does already exist
				var tstUser = userRepo.Filter(f => f.Id == user.Id).FirstOrDefault();
				if (tstUser != null)
				{
					//good
					try
					{
						//Check if the new email exists already
						var tstUserEmail = userRepo.Filter(f => f.Email == newEmail).FirstOrDefault();
						if (tstUserEmail == null)
						{
							user.Email = user.UserName = newEmail;
							manager.UpdateAsync(user);

							resp.User = tstUser;
							resp.IsValid = true;
						}
						else
						{
							//bad
							resp.IsValid = false;
							resp.ErrorMessage = "User already exists with the same name";
						}
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
					resp.ErrorMessage = "User already exists with the same name";
				}
			}

			return resp;
		}

		public GetUserResponse UpdateUserRoles(ApplicationUser user, List<string> newRoles, UserManager<ApplicationUser> manager)
		{
			var resp = new GetUserResponse();

			if (user == null || string.IsNullOrEmpty(user.Id))
			{
				resp.IsValid = false; //redundant
				resp.ErrorMessage = "user is invalid. Please validate fields";
			}
			else
			{
				//validate it already exists
				var tstUser = userRepo.Filter(f => f.Id == user.Id).FirstOrDefault();
				if (tstUser != null)
				{
					//good
					try
					{
						var oldRoles = tstUser.AspNetUserRoles;
						

						foreach(var or in oldRoles)
						{
							manager.RemoveFromRolesAsync(user, tstUser.AspNetUserRoles.Select(s => s.Role.Name));
						}

						foreach(var nr in newRoles)
						{
							manager.AddToRolesAsync(user, newRoles);
						}

						resp.User = tstUser;
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
					resp.ErrorMessage = "Role already exists with the same name";
				}
			}

			return resp;
		}
	}
}

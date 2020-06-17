using System;
using System.Collections.Generic;
using System.Linq;
using MicroActive.Security.AuthorizationServer.Business.Models;
using MicroActive.Security.AuthorizationServer.Business.Models.Clients;
using MicroActive.Security.AuthorizationServer.Data.Repositories;
using MicroActive.Security.AuthorizationServer.Data.Models;
using Microsoft.AspNetCore.Identity;
using MicroActive.Security.AuthorizationServer.Data;

namespace MicroActive.Security.AuthorizationServer.Business
{
	public class ClientService: BaseService
	{
		private DataRepository<AspNetUser> userRepo = null;
		private DataRepository<Client> repo = null;

		public ClientService(IdentityContext context) : base(context)
		{
			userRepo = new DataRepository<AspNetUser>(context);
			repo = new DataRepository<Client>(context);
		}

		public GetClientResponse GetClient(int id)
		{
			var resp = new GetClientResponse();

			if (id < 1)
			{
				resp.IsValid = false; //redundant
				resp.ErrorMessage = "id must be specified.";
			}
			else
			{
				var client = repo.Filter(f => f.Id == id, nameof(Client.ClientClaims), nameof(Client.ClientScopes)).FirstOrDefault();
				
				if (client == null)
				{
					resp.ErrorMessage = "client id not found";
				}
				else
				{
					resp.IsValid = true;
					resp.Client = client;
				}
			}

			return resp;
		}

		public GetClientResponse GetClients(params int[] ids)
		{
			var resp = new GetClientResponse();
			var list = new List<Client>();

			if (ids == null || ids.Length == 0)
			{
				list = repo.All(nameof(Client.ClientClaims), nameof(Client.ClientScopes)).ToList();
			}
			else
			{
				list = repo.Filter(f => ids.Contains(f.Id), nameof(Client.ClientClaims), nameof(Client.ClientScopes)).ToList();
			}

			if (!list.Any())
			{
				resp.ErrorMessage = "ids not found";
			}
			else
			{
				resp.IsValid = true;
				resp.Clients = list;
			}

			return resp;
		}

		public BaseResponse CreateClient(Client client)
		{
			var resp = new BaseResponse();

			if (client == null || client.Id > 0)
			{
				resp.IsValid = false; //redundant
				resp.ErrorMessage = "client is invalid.";
			}
			else
			{
				//validate it doesnt already exist
				//at this point we are enforcing uniqueness based on name. 
				var tstClient = repo.Filter(f => f.ClientName.ToLower() == client.ClientName.ToLower()).FirstOrDefault();
				if (tstClient == null)
				{
					//good
					try
					{
						var c = CopyClient(client, null);

						//Try this. Should only be new actions
						if (client.ClientClaims.Any())
						{
							c.ClientClaims.AddRange(client.ClientClaims);
						}

						repo.Create(c);
						//ts.Complete();
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
					resp.ErrorMessage = "Client already exists with the same name";
				}
			}

			return resp;
		}

		public BaseResponse DeleteClient(int id, ApplicationUser appUser)
		{
			var resp = new BaseResponse();

			if (id < 0)
			{
				resp.ErrorMessage = "No client specified.";
			}
			else
			{
				var user = userRepo.Filter(f => f.Id == appUser.Id).FirstOrDefault();
				if (user == null)
				{
					resp.ErrorMessage = "user not authenticated.";
				}
				else
				{
					var tstClient = repo.Filter(f => f.Id == id).FirstOrDefault();
					if (tstClient != null)
					{
						//good
						try
						{
							tstClient.Enabled = false;

							repo.Update(tstClient);

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
						resp.ErrorMessage = "Client does not exist";
					}
				}
			}

			return resp;
		}

		public GetClientResponse UpdateClient(Client client)
		{
			var resp = new GetClientResponse();

			if (client == null || client.Id < 0 || string.IsNullOrEmpty(client.ClientName))
			{
				resp.ErrorMessage = "client is invalid. Please validate fields";
			}
			else
			{
				//validate it doesnt already exist
				var tstClient = repo.Filter(f => f.Id == client.Id).FirstOrDefault();
				if (tstClient != null)
				{
					//good
					try
					{
						//using (var ts = new TransactionScope())
						//{

						tstClient = CopyClient(client, tstClient);

						repo.Update(tstClient);

						//TODO: Instead of deleting all existing, just delete from tst where ID's dont match, then match ID's, to update, then add where tst.actions not in role.actions
						//Count is not a reliable method of determining if there is a change
						if (tstClient.ClientClaims.Count != client.ClientClaims.Count)
						{
							int cnt = tstClient.ClientClaims.Count;
							for (int i = tstClient.ClientClaims.Count - 1; i > -1; i--)
							{
								repo.Delete(tstClient.ClientClaims[i]);
							}

							foreach (var ca in client.ClientClaims)
							{
								tstClient.ClientClaims.Add(new ClientClaim()
								{
									Id = ca.Id,
									Type = ca.Type,
									Value = ca.Value
								});
							}
						}

						if (tstClient.ClientSecrets.Count != client.ClientSecrets.Count)
						{
							int cnt = tstClient.ClientSecrets.Count;
							for (int i = tstClient.ClientSecrets.Count - 1; i > -1; i--)
							{
								repo.Delete(tstClient.ClientSecrets[i]);
							}

							foreach (var ca in client.ClientSecrets)
							{
								tstClient.ClientSecrets.Add(new ClientSecret()
								{
									Id = ca.Id,
									Type = ca.Type,
									Value = ca.Value,
									Description = ca.Description,
									Expiration = ca.Expiration
								});
							}
						}

						repo.Update(tstClient);
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
					resp.ErrorMessage = "Client already exists with the same name";
				}
			}

			return resp;
		}

		private static Client CopyClient(Client client, Client newClient)
		{
			if(newClient == null)
			{
				newClient = new Client();
			}

			newClient.AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime;
			newClient.AccessTokenLifetime = client.AccessTokenLifetime;
			newClient.AccessTokenType = client.AccessTokenType;
			newClient.AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser;
			newClient.AllowOfflineAccess = client.AllowOfflineAccess;
			newClient.AllowRememberConsent = client.AllowRememberConsent;
			newClient.AlwaysIncludeUserClaimsInIdToken = client.AlwaysIncludeUserClaimsInIdToken;
			newClient.AlwaysSendClientClaims = client.AlwaysSendClientClaims;
			newClient.AuthorizationCodeLifetime = client.AuthorizationCodeLifetime;
			newClient.ClientClaims = client.ClientClaims;
			newClient.ClientCorsOrigins = client.ClientCorsOrigins;
			newClient.ClientId = client.ClientId;
			newClient.ClientGrantTypes = client.ClientGrantTypes;
			newClient.ClientName = client.ClientName;
			newClient.ClientScopes = client.ClientScopes;
			newClient.ClientSecrets = client.ClientSecrets;
			newClient.ClientUri = client.ClientUri;
			newClient.Enabled = client.Enabled;
			newClient.IdentityTokenLifetime = client.IdentityTokenLifetime;
			newClient.RefreshTokenExpiration = client.RefreshTokenExpiration;
			newClient.RequireClientSecret = client.RequireClientSecret;
			newClient.RequireConsent = client.RequireConsent;
			newClient.SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime;
			newClient.UpdateAccessTokenClaimsOnRefresh = client.UpdateAccessTokenClaimsOnRefresh;

			return newClient;
		}
	}
}

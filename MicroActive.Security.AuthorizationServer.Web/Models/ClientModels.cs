using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroActive.Security.AuthorizationServer.Web.Models
{
	public class ClientModel
	{
		public string ClientName
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public int AccessTokenLifetime
		{
			get;
			set;
		}

		public bool AllowRememberConsent
		{
			get;
			set;
		}

		public string APIKey
		{
			get;
			set;
		}

		public bool Enabled
		{
			get;
			set;
		}

		public string ClientId
		{
			get;
			set;
		}

		public bool RequireConsent
		{
			get;
			set;
		}

		public List<ActionModel> ClientClaims
		{
			get;
			set;
		}
	}

	public class ActionModel
	{
		public int Id
		{
			get;
			set;
		}

		public int ClientId
		{
			get;
			set;
		}

		public string Domain
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public bool Enabled
		{
			get;
			set;
		}
	}
}
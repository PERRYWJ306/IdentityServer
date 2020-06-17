using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroActive.Security.AuthorizationServer.Web.Models
{
	public class ScopeModel
	{
		public string Name
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public bool Enabled
		{
			get;
			set;
		}

		public int ScopeType
		{
			get;
			set;
		}

		public List<ClaimModel> Actions
		{
			get;
			set;
		}
	}

	public class ClaimModel
	{
		public int Id
		{
			get;
			set;
		}

		public string CustomName
		{
			get;
			set;
		}

		public int ClaimTypeId
		{
			get;
			set;
		}
	}
}
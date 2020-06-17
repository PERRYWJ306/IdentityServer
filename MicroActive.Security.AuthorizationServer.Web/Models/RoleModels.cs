using System;
using System.Collections.Generic;
using System.Linq;


namespace MicroActive.Security.AuthorizationServer.Web.Models
{
	public class RoleModel
	{
		public string Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public List<RoleActionModel> Actions
		{
			get;
			set;
		}
	}

	public class RoleActionModel
	{
		/// <summary>
		/// Maps to Action.Id
		/// </summary>
		public int ActionId
		{
			get;
			set;
		}

		/// <summary>
		/// Maps to AspNetRoleAction.Id
		/// </summary>
		public int Id
		{
			get;
			set;
		}

		public string RoleId
		{
			get;
			set;
		}

		public string ActionDomain
		{
			get;
			set;
		}

		public int ActionClientId
		{
			get;
			set;
		}

		public string ActionName
		{
			get;
			set;
		}

		public bool ActionEnabled
		{
			get;
			set;
		}

		public bool Enabled
		{
			get;
			set;
		}

		public bool ActionDeny
		{
			get;
			set;
		}
	}
}
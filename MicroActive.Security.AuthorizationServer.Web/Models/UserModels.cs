using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroActive.Security.AuthorizationServer.Web.Models
{
	public class UserModel
	{
		public string Id
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public DateTime? BirthDate
		{
			get;
			set;
		}

		public int AccessFailedCount
		{
			get;
			set;
		}

		public bool LockoutEnabled
		{
			get;
			set;
		}

		public DateTime? LockoutEndDateUtc
		{
			get;
			set;
		}

		public bool EmailConfirmed
		{
			get;
			set;
		}

		public bool Deleted
		{
			get;
			set;
		}

		public string DeletedByUser_Id
		{
			get;
			set;
		}

		//public UserModel DeletedByUser
		//{
		//	get;
		//	set;
		//}

		public DateTime? DeletedDate
		{
			get;
			set;
		}

		public string Pwd { get; set; }
	}
}
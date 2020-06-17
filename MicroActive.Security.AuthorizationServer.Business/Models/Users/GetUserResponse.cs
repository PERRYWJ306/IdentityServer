using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MicroActive.Security.AuthorizationServer.Data;
using MicroActive.Security.AuthorizationServer.Data.Models;

namespace MicroActive.Security.AuthorizationServer.Business.Models.Users
{
	public class GetUserResponse: BaseResponse
	{
		public List<AspNetUser> Users
		{
			get;
			set;
		}

		public AspNetUser User
		{
			get;
			set;
		}
	}
}

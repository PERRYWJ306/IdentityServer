using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MicroActive.Security.AuthorizationServer.Data.Models;

namespace MicroActive.Security.AuthorizationServer.Business.Models.Roles
{
	public class GetRoleResponse: BaseResponse
	{
		public List<AspNetRole> Roles
		{
			get;
			set;
		}

		public AspNetRole Role
		{
			get;
			set;
		}
	}
}

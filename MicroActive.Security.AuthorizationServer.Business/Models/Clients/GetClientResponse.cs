using MicroActive.Security.AuthorizationServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroActive.Security.AuthorizationServer.Business.Models.Clients
{
	public class GetClientResponse: BaseResponse
	{
		public Client Client
		{
			get;
			set;
		}

		public List<Client> Clients
		{
			get;
			set;
		}
	}
}

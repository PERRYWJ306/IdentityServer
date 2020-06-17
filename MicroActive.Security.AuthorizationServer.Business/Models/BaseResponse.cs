using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroActive.Security.AuthorizationServer.Business.Models
{
	public class BaseResponse
	{
		public string ErrorMessage
		{
			get;
			set;
		}

		public Exception Error
		{
			get;
			set;
		}

		public bool IsValid
		{
			get;
			set;
		}
	}
}

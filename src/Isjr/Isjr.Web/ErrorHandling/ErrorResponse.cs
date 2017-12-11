using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Isjr.Web.ErrorHandling
{
	public class ErrorResponse
	{
		public string Message { get; set; }

		public string StackTrace { get; set; }

		public string InnerException { get; set; }

		public string InnerStackTrace { get; set; }
	}
}

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Isjr.Web.ErrorHandling
{
	public class GlobalExceptionFilter : IExceptionFilter, IDisposable
	{
		public void OnException(ExceptionContext context)
		{
			var response = new ErrorResponse
			{
				Message = context.Exception.Message,
				StackTrace = context.Exception.StackTrace
			};
			if (context.Exception.InnerException != null)
			{
				response.InnerException = context.Exception.InnerException.Message;
				response.InnerStackTrace = context.Exception.InnerException.StackTrace;
			}

			context.Result = new ObjectResult(response)
			{
				StatusCode = 500,
				DeclaredType = typeof(ErrorResponse)
			};

			// context.Result = new RedirectResult($"Error/{500}");
		}

		public void Dispose()
		{
		}
	}
}

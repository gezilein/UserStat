using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UserStat.Factory;
using UserStat.Interfaces;

namespace UserStat.Controllers
{
	/// <summary>WEB API controller</summary>
	[Route("api/[controller]")]
	public class SessionsController : Controller
	{
		private readonly static ILogger _log = Log.ForContext<SessionsController>(); //just logging
		private UserStatProvider _provider = new UserStatProvider(); //instance of main controller class (application logic) - avoid having logic on API level

		// POST api/sessions
		[HttpPost]
		public UserSessionsResponse Post([FromBody] List<string> users)
		{
			return HandleResponse<UserSessionsResponse>((response) =>
			{
				if (users == null || users.Count == 0)
				{
					_log.Warning("REQUEST api/sessions with invalid parameters: {0}", users);
					throw new UserStatException(BaseResponse.ResponseCode.MissingRequiredQueryParameter, "REQUEST api/sessions with invalid parameter: users");
				}

				_log.Information("REQUEST api/sessions with {0} users", users.Count);

				response.UserSessions = _provider.GetUserSessions(users); //retrieve and prepare data
			});
		}

		/// <summary>
		/// Helper method for error handling and proper response handling of specific
		/// requests/methods (makes more sense when more methods are present)
		/// </summary>
		/// <typeparam name="T">Type of response (inherited from <see cref="BaseResponse"/>)</typeparam>
		/// <param name="action">Execution of method logic to handle</param>
		/// <param name="memberName">Name of calling request/method for tracing purposes</param>
		/// <returns>Populated object of T, which is inherited from <see cref="BaseResponse"/></returns>
		private T HandleResponse<T>(Action<T> action, [CallerMemberName] string memberName = null) where T : BaseResponse, new()
		{
			var response = new T(); //create response object

			try
			{
				action(response); //execute internal logic on object
				response.SetCode(BaseResponse.ResponseCode.OK);
			}
			catch (UserStatException usex) //catch application/request specific errors
			{
				_log.Error(usex, usex.ToString());
				response.SetCode(usex.ErrorCode);
			}
			catch (Exception ex) //general (unhandled) error
			{
				_log.Error(ex, "ERROR in {0}", memberName);
				response.SetCode(BaseResponse.ResponseCode.InternalServerError);
			}

			return response; //respond to request
		}
	}
}
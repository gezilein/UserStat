using System.Collections.Generic;

namespace UserStat.Interfaces
{
	/// <summary>DTO class for carrying data on user session count</summary>
	public class UserSession
	{
		/// <summary>Number of sessions for specific user</summary>
		public int Sessions { get; set; }

		/// <summary>Specific user name (client's token)</summary>
		public string User { get; set; }

		/// <summary>String override if ever needed for output (trace most likely)</summary>
		/// <returns>String representation of current object</returns>
		public override string ToString()
		{
			return string.Concat("[UserSession: Sessions=", Sessions, ", User=", User, "]");
		}
	}

	public class UserSessionsResponse : BaseResponse
	{
		/// <summary>List (resut set) of user key/session objects</summary>
		public List<UserSession> UserSessions { get; set; }

		/// <summary>String override if ever needed for output (trace most likely)</summary>
		/// <returns>String representation of current object</returns>
		public override string ToString()
		{
			return string.Concat("[UserSessionsResponse: UserSessions=", UserSessions.Count, ", " + base.ToString() + "]");
		}
	}
}
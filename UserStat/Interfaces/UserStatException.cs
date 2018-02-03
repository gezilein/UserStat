using System;

namespace UserStat.Interfaces
{
	/// <summary>Internal application level error/response handling</summary>s
	public class UserStatException : Exception
	{
		public UserStatException(BaseResponse.ResponseCode code, string message) : base(message)
		{
			ErrorCode = code;
		}

		public BaseResponse.ResponseCode ErrorCode { get; private set; }
	}
}
namespace UserStat.Interfaces
{
	/// <summary>
	/// Service level response object used for all responses (for clear and concurrent responses)
	/// </summary>
	/// <remarks>
	/// All response objects must inherit from this as it is handled globally for all API methods
	/// </remarks>
	/// TODO: could have corresponding HTTP status codes (to be proper RESTful API)
	public class BaseResponse
	{
		/// <summary>Default constructor</summary>
		/// <remarks>Must be present for proper response initialization</remarks>
		public BaseResponse()
		{
		}

		/// <summary>Predefined response (error) codes</summary>
		public enum ResponseCode : int
		{
			OK = 200,
			MissingRequiredQueryParameter = 400,
			InternalServerError = 500,
			ExternalServiceError = 503,
		}

		/// <summary>Response code on outgoing object</summary>
		public int Code { get; set; }

		/// <summary>Message for outgoing response object</summary>
		public string Message { get; set; }

		/// <summary>Change response object status (code)</summary>
		/// <param name="code">Response code to set to current response</param>
		public void SetCode(ResponseCode code)
		{
			Code = (int)code;
			Message = GetMessage(code);
		}

		/// <summary>String override if ever needed for output (trace most likely)</summary>
		/// <returns>String representation of current object</returns>
		public override string ToString()
		{
			return string.Concat("[BaseResponse: Code=", Code, ", Message=", Message, "]");
		}

		/// <summary>
		/// Internal helper method for proper message on response object (corresponding to response code)
		/// </summary>
		/// <param name="code">Response code to define message with</param>
		/// <returns>String which is text description of response code</returns>
		/// TODO: Could have corresponding resource manager for proper (and culture related) messages
		private string GetMessage(ResponseCode code)
		{
			switch (code)
			{
				case ResponseCode.OK: return "Success";
				case ResponseCode.MissingRequiredQueryParameter: return "Bad or missing Parameter";
				case ResponseCode.InternalServerError: return "Internal Server Error";
				case ResponseCode.ExternalServiceError: return "Error on External Service";
				default: return string.Empty;
			}
		}
	}
}
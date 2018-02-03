using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using UserStat.Interfaces;
using UserStat.Services;

namespace UserStat.Factory
{
	/// <summary>
	/// Controlling (main application) class for managing and gathering data and retrieveing specific information.
	/// </summary>
	public class UserStatProvider
	{
		private readonly static ILogger _log = Log.ForContext(typeof(UserStatProvider)); //jst trace output

		public UserStatProvider()
		{ }

		/// <summary>Method collects and returns session count for each requested user</summary>
		/// <param name="users">List of user keys (client's tokens) to retrieve statistics for</param>
		/// <returns>List of <see cref="UserSession"/> DTOs, provided with session data</returns>
		/// <remarks>Method internally calls and gathers data also from external</remarks>
		public List<UserSession> GetUserSessions(List<string> users)
		{
			_log.Debug("STARTING GetUserSessions with {0} users", users.Count);

			var result = new List<UserSession>();
			try
			{
				//TODO: store each query data in local database
				var metadata = new Dictionary<string, string>();
				try
				{
					metadata = ClientService.Metadata(users); //retrieve database information
					_log.Debug("Found {0} users in metadata", metadata.Count);

					if (metadata.Count == 0) //no further data to work on
					{
						_log.Warning("EXIT GetUserSessions with no metadata");
						return result;
					}
				}
				catch (Exception ex)
				{
					throw new UserStatException(BaseResponse.ResponseCode.ExternalServiceError, "ERROR in ClientService.Metadata: " + ex.ToString());
				}

				//retrieve identities (for remaining users only)
				var identities = new Dictionary<string, string>();
				try
				{
					foreach (var db in metadata.Values.Distinct()) //query client for each database (distinct databases to optimize number of requests)
					{
						foreach (var identity in ClientService.Identities(db, metadata.Keys.ToList())) //retrieve usernames in specific database //TODO: optimize user keys with GroupBy database
						{
							identities.Add(identity.Key, identity.Value); //add if found in database
						}
					}
					_log.Debug("Retrieved identities for {0} users", identities.Count);

					if (identities.Count == 0) //no data to work on
					{
						_log.Warning("EXIT GetUserSessions with no identites");
						return result;
					}
				}
				catch (Exception ex)
				{
					throw new UserStatException(BaseResponse.ResponseCode.ExternalServiceError, "ERROR in ClientService.Identities: " + ex.ToString());
				}

				var userSessions = new Dictionary<string, string>();
				try
				{
					userSessions = ClientService.Statistics(identities.Keys.ToList()); //retrieve session counts (for remaining user keys)
					_log.Debug("Found {0} users with statistics", userSessions.Count);
				}
				catch (Exception ex)
				{
					throw new UserStatException(BaseResponse.ResponseCode.ExternalServiceError, "ERROR in ClientService.Statistics: " + ex.ToString());
				}

				foreach (var stat in userSessions) //populate result set
				{
					result.Add(new UserSession() { User = identities[stat.Key], Sessions = int.Parse(stat.Value) });
				}
			}
			catch (Exception ex)
			{
				_log.Error(ex, "ERROR in GetUserSessions");
				throw;
			}

			_log.Debug("FINISHED GetUserSessions with {0} users", result.Count);

			return result;
		}
	}
}
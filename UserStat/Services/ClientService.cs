using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace UserStat.Services
{
	/// <summary>
	/// Class that represents external client service. It could be an API endpoint but for this
	/// purpose it is simply a static class
	/// </summary>
	/// <remarks>
	/// For speed and readability purposes the data is stored in corresponding JSON file
	/// </remarks>
	/// <remarks>Should be separate project/library, but it's not - because readability</remarks>
	public static class ClientService
	{
		/// <summary>Method retrieves user identities from specific database</summary>
		/// <param name="databaseId">Database ID (key) of identity store</param>
		/// <param name="users">List of user keys (tokens) to to retrieve identities for</param>
		/// <returns>Dictionary of user keys mapped to specific identity (name)</returns>
		/// <remarks>Method removes all keys (users) not present in the metadata</remarks>
		/// <remarks>
		/// All methods presume that all errors are properly handled and all data (files) properly
		/// formatted and present
		/// </remarks>
		public static Dictionary<string, string> Identities(string databaseId, List<string> users)
		{
			var result = new Dictionary<string, string>();

			var file = new FileInfo(@".\ClientServiceData\identities-" + databaseId + ".json");
			if (!file.Exists)
			{
				return result;
			}

			var map = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(file.FullName));

			users.ForEach(u =>
			{
				if (map.ContainsKey(u))
				{
					result.Add(u, map[u]);
				}
			});

			return result;
		}

		/// <summary>Method that returns users mapped to specific database</summary>
		/// <param name="users">List of users (keys) to retrieve databases for</param>
		/// <returns>Dictionary of user keys mapped to specific database ID</returns>
		/// <remarks>Method removes all keys (users) not present in the metadata</remarks>
		public static Dictionary<string, string> Metadata(List<string> users)
		{
			var result = new Dictionary<string, string>();

			var map = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(@".\ClientServiceData\metadata.json"));
			users.ForEach(u =>
			{
				if (map.ContainsKey(u))
				{
					result.Add(u, map[u]);
				}
			});

			return result;
		}

		/// <summary>Method that returns users mapped to their specific session count</summary>
		/// <param name="users">List of users (keys) to statistics for</param>
		/// <returns>Dictionary of user keys mapped to specific database ID</returns>
		/// <remarks>Method removes all keys (users) not present in the statistics</remarks>
		public static Dictionary<string, string> Statistics(List<string> users)
		{
			var result = new Dictionary<string, string>();

			var map = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(@".\ClientServiceData\statistics.json"));
			users.ForEach(u =>
			{
				if (map.ContainsKey(u))
				{
					result.Add(u, map[u]);
				}
			});

			return result;
		}
	}
}
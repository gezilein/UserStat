using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;

namespace UserStat
{
	public class Program
	{
		public static IWebHost BuildWebHost(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseUrls("http://localhost:5000/")
				.Build();
		}

		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Console()
				.WriteTo.File("logs\\UserStat-.log", rollingInterval: RollingInterval.Hour)
				.CreateLogger();

			try
			{
				Log.Debug("INIT main");
				BuildWebHost(args).Run();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Stopped program because of exception");
				throw;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}
	}
}
using FCM.IO.Loader;
using System;
using System.Configuration;
using System.IO;

namespace TestApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			#region Config system
			// folders
			string inputFolder = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%" + ConfigurationManager.AppSettings["InputDirectory"]);
			string outputFolder = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%" + ConfigurationManager.AppSettings["OutputDirectory"]);
			// separators
			string fieldSeparator = ConfigurationManager.AppSettings["fieldSeparator"];
			string itemSeparator = ConfigurationManager.AppSettings["itemSeparator"];
			string itemDataSeparator = ConfigurationManager.AppSettings["itemDataSeparator"];
			#endregion

			try
			{
				LoaderManager loader = new LoaderManager(inputFolder);
				loader.LoadFiles();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}

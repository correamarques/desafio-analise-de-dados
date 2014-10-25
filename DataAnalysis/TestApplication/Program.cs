using FCM.BLL;
using FCM.IO.Loader;
using FCM.Types;
using System;
using System.Collections.Generic;
using System.Configuration;

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
			char fieldSeparator = Convert.ToChar(ConfigurationManager.AppSettings["fieldSeparator"]);
			char itemSeparator = Convert.ToChar(ConfigurationManager.AppSettings["itemSeparator"]);
			char itemDataSeparator = Convert.ToChar(ConfigurationManager.AppSettings["itemDataSeparator"]);
			#endregion

			try
			{
				LoaderManager loader = new LoaderManager(inputFolder, fieldSeparator, itemSeparator, itemDataSeparator);
				List<FlatFile> files = loader.LoadFiles();

				Data data = new Data();
				List<FileReport> reportList = data.PerformAnalysis(files);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}

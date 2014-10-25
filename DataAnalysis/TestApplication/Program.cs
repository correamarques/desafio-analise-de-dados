using FCM.BLL;
using FCM.IO.Exporter;
using FCM.IO.Loader;
using FCM.Types;
using System;
using System.Collections.Generic;
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
			char fieldSeparator = Convert.ToChar(ConfigurationManager.AppSettings["fieldSeparator"]);
			char itemSeparator = Convert.ToChar(ConfigurationManager.AppSettings["itemSeparator"]);
			char itemDataSeparator = Convert.ToChar(ConfigurationManager.AppSettings["itemDataSeparator"]);
			#endregion

			try
			{
				if (DirectoriesExists(inputFolder, outputFolder))
				{
					LoaderManager loader = new LoaderManager(inputFolder, fieldSeparator, itemSeparator, itemDataSeparator);
					IList<FlatFile> files = loader.LoadFiles();

					Data data = new Data();
					IList<FileReport> reportList = data.PerformAnalysis(files);

					ExporterManager exporter = new ExporterManager(outputFolder, fieldSeparator);
					exporter.Save(reportList); 
				}
				else
					Console.WriteLine("Directories not found.");

			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		static bool DirectoriesExists(string inputFolder, string outputFolder)
		{
			if (Directory.Exists(inputFolder) && Directory.Exists(outputFolder))
				return true;
			return false;
		}
	}
}

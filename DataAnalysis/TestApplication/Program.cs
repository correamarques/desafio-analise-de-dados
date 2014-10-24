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
			//string inputFolder = System.IO.Path.GetFullPath(ConfigurationManager.AppSettings["InputDirectory"]);
			string inputFolder = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%" + ConfigurationManager.AppSettings["InputDirectory"]);
			//string outputFolder = ConfigurationManager.AppSettings["OutputDirectory"];

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

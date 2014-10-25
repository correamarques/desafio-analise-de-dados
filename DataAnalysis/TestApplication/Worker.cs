using FCM.BLL;
using FCM.IO.Exporter;
using FCM.IO.Loader;
using FCM.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FCM.TestApplication
{
	public class Worker
	{
		private int workerIndex;
		private System.Diagnostics.EventLog eventLog;
		private bool serviceStarted;
		public bool ServiceStarted
		{
			get { return serviceStarted; }
			set { serviceStarted = value; }
		}

		public Worker(int workerIndex, System.Diagnostics.EventLog eventLog)
		{
			this.workerIndex = workerIndex;
			this.eventLog = eventLog;
		}

		internal void ExecuteTask()
		{
			this.eventLog.WriteEntry(String.Format("Starting worker {0}", this.workerIndex));

			DateTime lastRunTime = DateTime.UtcNow;

			while (serviceStarted)
			{
				// check the current time against the last run plus interval
				if (((TimeSpan)(DateTime.UtcNow.Subtract(lastRunTime))).TotalSeconds >= 10)
				{
					// if time to do something, do so
					// exception handling omitted here for simplicity
					eventLog.WriteEntry("Multithreaded Service working; id = " + this.workerIndex.ToString(), System.Diagnostics.EventLogEntryType.Information);

					// set new run time
					lastRunTime = DateTime.UtcNow;
				}

				// yield
				if (serviceStarted)
				{
					ProcessInformation();
					Thread.Sleep(new TimeSpan(0, 0, 15));
				}
			}

			Thread.CurrentThread.Abort();
		}

		void ProcessInformation()
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

					DataAnalysis dataAnalysis = new DataAnalysis();
					IList<FileReport> reportList = dataAnalysis.PerformAnalysis(files);

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
		bool DirectoriesExists(string inputFolder, string outputFolder)
		{
			if (Directory.Exists(inputFolder) && Directory.Exists(outputFolder))
				return true;
			return false;
		}
	}
}

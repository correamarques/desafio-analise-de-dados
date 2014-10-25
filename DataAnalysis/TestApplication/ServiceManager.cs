﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApplication
{
	public class ServiceManager
	{
		// array of worker threads
		Thread[] workerThreads;

		// the objects that do the actual work
		Worker[] arrWorkers;

		// number of threads;
		int numberOfThreads = 10;

		private EventLog eventLog;

		public ServiceManager()
		{
			//InitializeComponent();
			//this.EventLog.Source = "FCM Data Analysis";
			//this.EventLog.Log = "Application";

			if (!EventLog.SourceExists("FCM Data Analysis"))
				EventLog.CreateEventSource("FCM Data Analysis", "Application");

			eventLog = new EventLog();
			eventLog.Source = "FCM Data Analysis";
			eventLog.Log = "Application";

			numberOfThreads = int.Parse(ConfigurationManager.AppSettings["NumberOfThreads"]);
		}

		public void Start()
		{
			arrWorkers = new Worker[numberOfThreads];
			workerThreads = new Thread[numberOfThreads];
			for (int i = 0; i < numberOfThreads; i++)
			{
				// create an object
				arrWorkers[i] = new Worker(i + 1, eventLog);

				// set properties on the object
				arrWorkers[i].ServiceStarted = true;

				// create a thread and attach to the object
				ThreadStart st = new ThreadStart(arrWorkers[i].ExecuteTask);
				workerThreads[i] = new Thread(st);
			}

			// start the threads
			for (int i = 0; i < numberOfThreads; i++)
			{
				workerThreads[i].Start();
			}
		}
		public void Stop()
		{
			for (int i = 0; i < numberOfThreads; i++)
			{
				// set flag to stop worker thread
				arrWorkers[i].ServiceStarted = false;

				// give it a little time to finish any pending work
				workerThreads[i].Join(new TimeSpan(0, 2, 0));
			}
		}
	}
}

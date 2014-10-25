using FCM.BLL;
using FCM.IO.Exporter;
using FCM.IO.Loader;
using FCM.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace FCM.TestApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			ServiceManager serviceManager = new ServiceManager();
			try
			{
				serviceManager.Start();
			}
			catch (Exception e)
			{
				serviceManager.Stop();
				Console.WriteLine(e.Message);
			}
		}
	}
}

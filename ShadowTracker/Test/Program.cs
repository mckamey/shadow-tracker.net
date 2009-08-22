﻿using System;
using System.Configuration;

using Shadow.Agent;
using Shadow.Model;

namespace Shadow.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			FileTracker tracker = new FileTracker();
			string watchFolder = ConfigurationManager.AppSettings["WatchFolder"];
			string watchFilter = ConfigurationManager.AppSettings["WatchFilter"];

			FileCatalog catalog = new FileCatalog(watchFolder, new MemoryTable<CatalogEntry>(new CatalogEntry.PathComparer()));

			Console.WriteLine("Begin tracking " + watchFolder);
			tracker.Start(watchFolder, watchFilter, catalog);

			Console.WriteLine("Press ENTER to exit.");
			Console.ReadLine();

			tracker.Stop();
		}
	}
}

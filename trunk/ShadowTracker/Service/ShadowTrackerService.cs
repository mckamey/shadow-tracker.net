﻿using System;
using System.Configuration;
using System.ServiceProcess;

using Shadow.Agent;
using Shadow.Model;

namespace ShadowTrackerService
{
	public partial class ShadowTrackerService : ServiceBase
	{
		#region Fields

		private readonly FileTracker Tracker = new FileTracker();

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public ShadowTrackerService()
		{
			this.InitializeComponent();
		}

		#endregion Init

		#region Methods

		protected override void OnStart(string[] args)
		{
			string watchFolder = ConfigurationManager.AppSettings["WatchFolder"];
			string watchFilter = ConfigurationManager.AppSettings["WatchFilter"];

			Catalog catalog = new Catalog(new MemoryTable<CatalogEntry>(CatalogEntry.PathComparer));
			FileUtility.SyncCatalog(catalog, watchFolder);

			this.Tracker.Start(watchFolder, watchFilter, catalog);
		}

		protected override void OnStop()
		{
			this.Tracker.Stop();
		}

		#endregion Methods
	}
}
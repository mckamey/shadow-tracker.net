﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Shadow.Model.Memory
{
	public class MemoryUnitOfWork : IUnitOfWork
	{
		#region Fields

		private IEnumerable<Catalog> CatalogsStorage;
		private MemoryTable<Catalog> CatalogsIdentityMap;

		private IEnumerable<CatalogEntry> EntriesStorage;
		private MemoryTable<CatalogEntry> EntriesIdentityMap;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public MemoryUnitOfWork()
			: this(null)
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="catalogs">initial catalogs</param>
		public MemoryUnitOfWork(IEnumerable<Catalog> catalogs)
			: this(null, null)
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="catalogs">initial catalogs</param>
		/// <param name="entities">initial entries</param>
		public MemoryUnitOfWork(IEnumerable<Catalog> catalogs, IEnumerable<CatalogEntry> entities)
		{
			this.CatalogsStorage = catalogs != null ? catalogs : Enumerable.Empty<Catalog>();
			this.EntriesStorage = entities != null ? entities : Enumerable.Empty<CatalogEntry>();
		}

		#endregion Init

		#region IUnitOfWork Members

		public void Save()
		{
			// "save" catalogs
			if (this.CatalogsIdentityMap != null)
			{
				// save contents to "storage"
				this.CatalogsStorage = this.CatalogsIdentityMap.AsEnumerable();
			}

			// reset change tracking
			this.CatalogsIdentityMap = null;

			// "save" entries
			if (this.EntriesIdentityMap != null)
			{
				// save contents to "storage"
				this.EntriesStorage = this.EntriesIdentityMap.AsEnumerable();
			}

			// reset change tracking
			this.EntriesIdentityMap = null;
		}

		public ITable<Catalog> Catalogs
		{
			get
			{
				if (this.CatalogsIdentityMap == null)
				{
					this.CatalogsIdentityMap = new MemoryTable<Catalog>(Catalog.PathComparer, this.CatalogsStorage);
				}
				return this.CatalogsIdentityMap;
			}
		}

		public ITable<CatalogEntry> Entries
		{
			get
			{
				if (this.EntriesIdentityMap == null)
				{
					this.EntriesIdentityMap = new MemoryTable<CatalogEntry>(CatalogEntry.PathComparer, this.EntriesStorage);
				}
				return this.EntriesIdentityMap;
			}
		}

		#endregion IUnitOfWork Members
	}
}
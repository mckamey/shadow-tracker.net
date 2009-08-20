﻿using System;
using System.Linq;

namespace Shadow.Model
{
	/// <summary>
	/// An in-memory implementation of DataNode Catalog.
	/// </summary>
	public class Catalog : ICatalogRepository
	{
		#region Fields

		private ITable<CatalogEntry> entries;

		#endregion Fields

		#region ICatalogRepository Members

		/// <summary>
		/// Gets and sets the sequence of data nodes
		/// </summary>
		public ITable<CatalogEntry> Entries
		{
			get { return this.entries; }
			set { this.entries = value; }
		}

		public CatalogEntry GetEntryAtPath(string path)
		{
			return
				(from entry in this.entries
				 where entry.Path == path
				 select entry).SingleOrDefault();
		}

		public bool ContainsPath(string path)
		{
			return
				(from entry in this.entries
				 where entry.Path == path
				 select entry.Path).Count() > 0;
		}

		public string GetPathOfEntryBySignature(string hash)
		{
			return
				(from entry in this.entries
				 where entry.Signature == hash
				 select entry.Path).SingleOrDefault();
		}

		public bool ContainsSignature(string hash)
		{
			return
				(from entry in this.entries
				 where entry.Signature == hash
				 select entry.Path).Count() > 0;
		}

		public void AddEntry(CatalogEntry entry)
		{
			this.entries.Add(entry);
		}

		public void UpdateEntry(CatalogEntry entry)
		{
			this.entries.Update(entry);
		}

		public void DeleteEntry(string path)
		{
			this.entries.RemoveWhere(n => n.Path == path);
		}

		#endregion ICatalogRepository Members
	}
}

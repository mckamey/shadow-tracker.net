﻿using System;
using System.IO;
using System.Text;

namespace Shadow.Agent
{
	public class TrackerTask
	{
		#region Fields

		private FileSystemInfo fileInfo;
		private FileSystemInfo oldFileInfo;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public TrackerTask()
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		public TrackerTask(FileSystemEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e", "FileSystemEventArgs was null");
			}

			this.TaskSource = TaskSource.UserAction;

			this.ChangeType = e.ChangeType;
			this.FullPath = e.FullPath;

			RenamedEventArgs re = e as RenamedEventArgs;
			if (re != null)
			{
				this.OldFullPath = re.OldFullPath;
			}
		}

		#endregion Init

		#region Properties

		public WatcherChangeTypes ChangeType
		{
			get;
			set;
		}

		public FileSystemInfo FileInfo
		{
			get
			{
				if (this.fileInfo == null)
				{
					if (!String.IsNullOrEmpty(this.FullPath))
					{
						this.fileInfo = FileUtility.CreateFileSystemInfo(this.FullPath);
					}
				}
				return this.fileInfo;
			}
			set { this.fileInfo = value; }
		}

		public string FullPath
		{
			get;
			set;
		}

		public string OldFullPath
		{
			get;
			set;
		}

		public FileSystemInfo OldFileInfo
		{
			get
			{
				if (this.oldFileInfo == null)
				{
					if (!String.IsNullOrEmpty(this.OldFullPath))
					{
						this.oldFileInfo = FileUtility.CreateFileSystemInfo(this.OldFullPath);
					}
				}
				return this.oldFileInfo;
			}
		}

		public decimal Priority
		{
			get;
			set;
		}

		public int RetryCount
		{
			get;
			set;
		}

		public TaskSource TaskSource
		{
			get;
			set;
		}

		#endregion Properties

		#region Object Overrides

		/// <summary>
		/// Represents the TaskItem as a string
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder("{ ");

			builder.Append(this.Priority.ToString("0.000"));

			if (this.ChangeType != default(WatcherChangeTypes))
			{
				builder.Append(", ");
				builder.Append(this.ChangeType.ToString());
			}

			if (!String.IsNullOrEmpty(this.FullPath))
			{
				builder.Append(", Path=");
				builder.Append(this.FullPath);
			}

			if (!String.IsNullOrEmpty(this.OldFullPath))
			{
				builder.Append(", Old=");
				builder.Append(this.OldFullPath);
			}

			builder.Append(", Source=");
			builder.Append(this.TaskSource);

			if (this.RetryCount > 0)
			{
				builder.Append(", Retry=");
				builder.Append(this.RetryCount);
			}

			builder.Append(" }");

			return builder.ToString();
		}

		#endregion Object Overrides
	}
}

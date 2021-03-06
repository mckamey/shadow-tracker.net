﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Shadow.Agent
{
	internal class FileHash
	{
		#region Fields

		private static readonly SHA1 HashProvider = SHA1.Create();

		#endregion Fields

		#region Utility Methods

		/// <summary>
		/// Generates a hash signature from file data
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		/// <exception cref="System.UnauthorizedAccessException">The path is read-only or is a directory.</exception>
		/// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
		/// <exception cref="System.IO.IOException">The file is already open.</exception>
		public static string ComputeHash(FileInfo file)
		{
			file.Refresh();
			if (!file.Exists)
			{
				return null;
			}

#if VERBOSE
			var timer = System.Diagnostics.Stopwatch.StartNew();
			try
#endif
			{
				using (Stream data = file.OpenRead())
				{
					return FileHash.ComputeHash(data);
				}
			}
#if VERBOSE
			finally
			{
				timer.Stop();
				long ms = timer.ElapsedMilliseconds;
				Console.WriteLine("Hash took {0} ms for {1} bytes", ms, file.Length);
			}
#endif
		}

		/// <summary>
		/// Generates a hash signature from file data
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string ComputeHash(Stream data)
		{
			byte[] hash;
			lock (HashProvider)
			{
				// generate hash signature
				hash = HashProvider.ComputeHash(data);
			}

			return FormatBytes(hash);
		}

		public static string FormatBytes(byte[] value)
		{
			if (value == null || value.Length == 0)
			{
				return String.Empty;
			}

			StringBuilder builder = new StringBuilder(value.Length*2);

			// Loop through each byte of the binary data 
			// and format each one as a hexadecimal string
			foreach (byte b in value)
			{
				builder.Append(b.ToString("x2"));
			}

			// the hexadecimal string
			return builder.ToString();
		}

		#endregion Utility Methods
	}
}

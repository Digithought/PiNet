﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digithought.PiNet
{
	public abstract class PortBase
	{
		public RPiPort Port { get; private set; }

		public PortBase(RPiPort port, bool autoConfigure = true)
		{
			Port = port;

			if (autoConfigure)
				Configure();
		}

		public abstract void Configure();

		public abstract void Unconfigure();

		protected static void WriteToFile(string fileName, string value)
		{
			Logging.Trace(PiNetLoggingCategory.IO, "Writing to '" + fileName + "': " + value);

			using (var writer = new StreamWriter(new FileStream(fileName, FileMode.Open, FileAccess.Write), Encoding.ASCII))
			{
				writer.Write(value);
			}
		}

		protected static string ReadFromFile(string fileName)
		{
			Logging.Trace(PiNetLoggingCategory.IO, "Attempting read from '" + fileName + "'");

			var result = File.ReadAllText(fileName, Encoding.ASCII);

			Logging.Trace(PiNetLoggingCategory.IO, "Read from file '" + fileName + "': " + result);

			return result;
		}

		protected static string[] ReadLinesFromFile(string fileName)
		{
			Logging.Trace(PiNetLoggingCategory.IO, "Attempting read from '" + fileName + "'");

			var result = File.ReadAllLines(fileName, Encoding.ASCII);

			Logging.Trace(PiNetLoggingCategory.IO, "Read from file '" + fileName + "': " + String.Join("\n", result));

			return result;
		}

		protected static bool ParseBool(string value)
		{
			return int.Parse(value) != 0;
		}
	}
}

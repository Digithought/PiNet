using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digithought.PiNet
{
	public class Gpio : PortBase
	{
        private static readonly Dictionary<RPiPort, int> GpioMappings =
            new Dictionary<RPiPort, int>
			{
				{ RPiPort.P1_3, 2 }, { RPiPort.P1_5, 3 }, { RPiPort.P1_7, 4 }, { RPiPort.P1_8, 14 }, { RPiPort.P1_10, 15 },
                { RPiPort.P1_11, 17 }, { RPiPort.P1_12, 18 }, { RPiPort.P1_13, 27 }, { RPiPort.P1_15, 22 }, { RPiPort.P1_16, 23 },
                { RPiPort.P1_18, 24 }, { RPiPort.P1_19, 10 }, { RPiPort.P1_21, 9 }, { RPiPort.P1_22, 25 }, { RPiPort.P1_23, 11 },
                { RPiPort.P1_24, 8 }, { RPiPort.P1_26, 7 }, { RPiPort.P1_29, 5 }, { RPiPort.P1_31, 6 }, { RPiPort.P1_32, 12 },
                { RPiPort.P1_33, 13 }, { RPiPort.P1_35, 19 }, { RPiPort.P1_36, 16 }, { RPiPort.P1_37, 26 }, { RPiPort.P1_38, 20 },
                { RPiPort.P1_40, 21 },
			};

		private const string GpioPath = "/sys/class/gpio/";
        private const string GpioPrefix = "gpio";
		private bool _checkSafety;

		/// <summary> Wraps a digital I/O port. </summary>
		/// <param name="autoConfigure"> Whether to automatically configure the port.  Set to true unless you're certain the port is already configured. </param>
		/// <param name="checkSafety"> When true, extra checks are performed to be sure the port is in ready state before writing. </param>
		public Gpio(RPiPort port, bool autoConfigure = true, bool checkSafety = true) : base(port, autoConfigure)
		{
			_checkSafety = checkSafety;
		}

		public override void Configure()
		{
			if (!Directory.Exists(GetGpioDevicePath()))
                WriteToFile(GpioPath + "export", GpioMappings[Port].ToString());
		}

		public override void Unconfigure()
		{
			if (Directory.Exists(GetGpioDevicePath()))
                WriteToFile(GpioPath + "unexport", GpioMappings[Port].ToString());
		}

		public RPiDirection? Direction
		{
			get
			{
				var value = ReadFromFile(DirectionFileName()).Trim();
				return value == "in" ? RPiDirection.In 
					: value == "out" ? RPiDirection.Out 
					: (RPiDirection?)null;
			}
			set
			{
				WriteToFile(DirectionFileName(), value.Value == RPiDirection.In ? "in" : "out");
			}
		}

		private string DirectionFileName()
		{
			return Path.Combine(GetGpioDevicePath(), "direction");
		}

		private string GetGpioDevicePath()
		{
            return Path.Combine(GpioPath, GpioPrefix + GpioMappings[Port]);
		}

		public bool IsReady
		{
			get { return IsConfigured && Direction != null; }
		}

		public bool IsConfigured
		{
			get { return File.Exists(ValueFileName()); }
		}

		public int Value
		{
			get
			{
				return int.Parse(ReadFromFile(ValueFileName()));
			}
			set
			{
				if (_checkSafety && !IsReady)
					throw new PiNetException(String.Format("Cannot write to port {0} as it is not ready.  The port must be configured and direction set.", Port));
				WriteToFile(ValueFileName(), value.ToString());
			}
		}

		private string ValueFileName()
		{
			return Path.Combine(GetGpioDevicePath(), "value");
		}

		public bool IsHigh
		{
			get
			{
				return Value != 0;
			}
			set
			{
				Value = value ? 1 : 0;
			}
		}
	}
}

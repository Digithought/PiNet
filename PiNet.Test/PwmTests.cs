using System;
using Digithought.PiNet;
using NUnit.Framework;

namespace PiNet.Test
{
	[TestFixture]
	public class PwmTests
	{
		[Test]
		public void ConstructWithConfigureTest()
		{
			// TODO: allow file IO to be mocked so that these can be unit tested
			//var pwm = new Pwm(RPiPort.P9_14, true); 
		}

		[Test]
		public void UnconfigureTest()
		{
			//var pwm = new Pwm(RPiPort.P9_14, false);
			//pwm.Unconfigure();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Digithought.PiNet
{
	[Serializable]
	public class PiNetException : Exception, ISerializable
	{
		public PiNetException(string message) : base(message)
		{
		}

		protected PiNetException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
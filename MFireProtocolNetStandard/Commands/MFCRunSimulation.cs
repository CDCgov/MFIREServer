using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
	public class MFCRunSimulation : MFireCmd
	{
        [ProtoMember(1)]
		public double SimulationTimestep;
    }
}

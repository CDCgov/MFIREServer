using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
	public class MFCSimulationUpdated : MFireCmd
	{
        [ProtoMember(1)]
		public double ElapsedTimeMs;
    }
}

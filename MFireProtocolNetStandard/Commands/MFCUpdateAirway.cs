using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
	public class MFCUpdateAirway : MFireCmd
	{
        [ProtoMember(1)]
		public int AirwayIndex;
        [ProtoMember(2)]
		public MFAirway Airway;

    }
}

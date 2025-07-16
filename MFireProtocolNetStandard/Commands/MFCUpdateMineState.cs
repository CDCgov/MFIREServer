using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
	public class MFCUpdateMineState : MFireCmd
	{
        [ProtoMember(1)]
		public List<MFAirway> Airways;
        [ProtoMember(2)]
        public List<MFJunction> Junctions;
    }
}

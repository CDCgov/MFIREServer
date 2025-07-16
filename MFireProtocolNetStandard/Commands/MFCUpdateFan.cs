using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
	public class MFCUpdateFan : MFireCmd
	{
        [ProtoMember(1)]
		public int FanIndex;
        [ProtoMember(2)]
		public MFFan Fan;
    }
}

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCUpdateJunction : MFireCmd
	{
        [ProtoMember(1)]
		public int JunctionIndex;
        [ProtoMember(2)]
        public MFJunction Junction;

    }
}

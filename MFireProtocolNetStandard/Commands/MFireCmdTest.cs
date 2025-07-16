using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
	public class MFireCmdTest : MFireCmd
	{
        [ProtoMember(1)]
		public int Int1;
        [ProtoMember(2)]
        public int Int2;
        [ProtoMember(3)]
        public string TestString;
    }
}

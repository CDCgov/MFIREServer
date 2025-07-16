using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCUpdateFire : MFireCmd
	{
        [ProtoMember(1)]
		public int FireIndex;
        [ProtoMember(2)]
        public MFFire Fire;

    }
}

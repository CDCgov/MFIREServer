using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCGetFanAirFlowData : MFireCmd
    {
        [ProtoMember(1)]
        public int A_0 { get; set; }
    }
}

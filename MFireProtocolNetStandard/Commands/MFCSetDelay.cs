using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCSetDelay : MFireCmd
    {
        [ProtoMember(1)]
        public double Seconds { get; set; }
    }
}

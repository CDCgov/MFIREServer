using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCSetContinuousMode : MFireCmd
    {
        [ProtoMember(1)]
        public bool Continuous { get; set; }
    }
}

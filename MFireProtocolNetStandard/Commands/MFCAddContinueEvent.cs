using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCAddContinueEvent : MFireCmd
    {
        [ProtoMember(1)]
        public double TimeStamp { get; set; }
    }
}

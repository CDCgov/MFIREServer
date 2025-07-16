using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCAddClearFireEvent : MFireCmd
    {
        [ProtoMember(1)]
        public double TimeStamp { get; set; }
        [ProtoMember(2)]
        public int AirwayNumber { get; set; }
    }
}

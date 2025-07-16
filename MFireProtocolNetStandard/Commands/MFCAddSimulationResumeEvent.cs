using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCAddSimulationResumeEvent : MFireCmd
    {
        [ProtoMember(1)]
        public double TimeStamp { get; set; }
    }
}

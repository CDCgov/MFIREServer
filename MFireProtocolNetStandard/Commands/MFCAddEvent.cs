using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCAddEvent : MFireCmd
    {
        [ProtoMember(1)]
        public SimulationEvent SimulationEvent { get; set; }
    }
}

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFRGetEventQueue : MFireResult
    {
        [ProtoMember(1)]
        public List<SimulationEvent> SimulationEvents { get; set; }
    }
}

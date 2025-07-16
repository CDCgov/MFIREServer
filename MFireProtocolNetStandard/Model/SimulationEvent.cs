using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class SimulationEvent
    {
        [ProtoMember(1)]
        public int EventType { get; set; }
        [ProtoMember(2)]
        public bool Periodic { get; set; }
        [ProtoMember(3)]
        public double RecurringInterval { get; set; }
        [ProtoMember(4)]
        public double TS { get; set; }
    }
}

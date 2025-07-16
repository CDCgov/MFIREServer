using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCAddChangeParameterEvent : MFireCmd
    {
        [ProtoMember(1)]
        public double TimeStamp { get; set; }
        [ProtoMember(2)]
        public SimulationParameter SimulationParameter { get; set; }
        [ProtoMember(3)]
        public double Value { get; set; }
    }
}

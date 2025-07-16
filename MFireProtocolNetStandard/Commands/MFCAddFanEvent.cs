using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCAddFanEvent : MFireCmd
    {
        [ProtoMember(1)]
        public double TimeStamp { get; set; }
        [ProtoMember(2)]
        public int AirwayNumber { get; set; }
        [ProtoMember(3)]
        public List<double> AirflowData { get; set; }
        [ProtoMember(4)]
        public List<double> PressureData { get; set; }
    }
}

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCAddFireSourceEvent : MFireCmd
    {
        [ProtoMember(1)]
        public double TimeStamp { get; set; }
        [ProtoMember(2)]
        public int AirwayNumber { get; set; }
        [ProtoMember(3)]
        public double ContamFlowRate { get; set; }
        [ProtoMember(4)]
        public double ContamConcentration { get; set; }
        [ProtoMember(5)]
        public double HeatInput { get; set; }
        [ProtoMember(6)]
        public double O2ConcLeavingFire { get; set; }
        [ProtoMember(7)]
        public double ContamPerCuFtO2 { get; set; }
        [ProtoMember(8)]
        public double HeatPerCuFtO2 { get; set; }
        [ProtoMember(9)]
        public double StandardAirFlow { get; set; }
        [ProtoMember(10)]
        public double TransitionTime { get; set; }
    }
}

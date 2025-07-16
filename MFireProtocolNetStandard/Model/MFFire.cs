using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProtoBuf;

namespace MFireProtocol
{
    [ProtoContract]
	public class MFFire
	{
        [ProtoMember(1)]
		public int Number;
        [ProtoMember(2)]
        public int AirwayNo;
        [ProtoMember(3)]
        public double ContamFlowRate;
        [ProtoMember(4)]
        public double ContamConcentration;
        [ProtoMember(5)]
        public double HeatInput;
        [ProtoMember(6)]
        public double O2ConcLeavingFire;
        [ProtoMember(7)]
        public double ContamPerCuFtO2;
        [ProtoMember(8)]
        public double HeatPerCuFtO2;
        [ProtoMember(9)]
        public double StandardAirFlow;
        [ProtoMember(10)]
        public double TransitionTime;
	}
}

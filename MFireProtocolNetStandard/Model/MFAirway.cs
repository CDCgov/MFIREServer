using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProtoBuf;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFAirway
	{
        [ProtoMember(1)]
		public int Number;
        [ProtoMember(2)]
        public int StartJunction;
        [ProtoMember(3)]
        public int EndJunction;
        [ProtoMember(4)]
        public int Type;
        [ProtoMember(5)]
        public int FlowDirection;

        [ProtoMember(6)]
        public double CH4EmissionRateAirway;
        [ProtoMember(7)]
        public double CH4EmissionRateSurfArea;
        [ProtoMember(8)]
        public double FlowRate;
        [ProtoMember(9)]
        public double FrictionFactor;
        [ProtoMember(10)]
        public double Length;
        [ProtoMember(11)]
        public double CrossSectionalArea;
        [ProtoMember(12)]
        public double Perimeter;
        [ProtoMember(13)]
        public double Resistance;
        [ProtoMember(14)]
        public double RockTemperature;
        [ProtoMember(15)]
        public double ThermalConductivity;
        [ProtoMember(16)]
        public double ThermalDefusivity;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProtoBuf;

namespace MFireProtocol
{
    [ProtoContract]
	public class MFJunction
	{
		//input data
        [ProtoMember(1)]
		public int Number;
        [ProtoMember(2)]
        public int AtmosphereJuncType;
        [ProtoMember(3)]
        public bool IsInAtmosphere;
        [ProtoMember(4)]
        public double CH4InitialConc;
        [ProtoMember(5)]
        public double Elevation;
        [ProtoMember(6)]
        public double Temperature;

        //calculation results
        [ProtoMember(7)]
        public bool ConditionChanged;
        [ProtoMember(8)]
        public double TotalHeat;
        [ProtoMember(9)]
        public double AtmosphereTemperature;
        [ProtoMember(10)]
        public double TemperatureInter;
        [ProtoMember(11)]
        public double TemperatureBkp;
        [ProtoMember(12)]
        public double ContamConcentrationBkp;
        [ProtoMember(13)]
        public double CH4ConcentrationBkp;
        [ProtoMember(14)]
        public double TotalCH4;
        [ProtoMember(15)]
        public double TotalContaminant;
        [ProtoMember(16)]
        public double TotalAirFlow;
        [ProtoMember(17)]
        public double CH4Concentration;
        [ProtoMember(18)]
        public double ContamConcentration;
        
	}
}

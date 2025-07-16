using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCConfigureMFire : MFireCmd
	{
        [ProtoMember(1)]
		public int StartJunction = 1;
        [ProtoMember(2)]
        public double StartJunctionTemperature = 50;
        [ProtoMember(3)]
        public double TimeEquillibrium = 0.1f;
        [ProtoMember(4)]
        public double FumeCriteria = 0.5f;
        [ProtoMember(5)]
        public double CH4Criteria = 0.5f;
        [ProtoMember(6)]
        public double TemperatureCriteria = 0.5f;
        [ProtoMember(7)]
        public double PressureDropWarningLimit = 0.25f;
        [ProtoMember(8)]
        public double FumeWarningLimit = 0.5f;
        [ProtoMember(9)]
        public double CH4WarningLimit = 9.0f;
        [ProtoMember(10)]
        public double TemperatureLimit = 900;
        [ProtoMember(11)]
        public int AvgValueDataPresent = 0;
        [ProtoMember(12)]
        public int MaxDynamicIterations = 10;
        [ProtoMember(13)]
        public int MaxTemperatureIterations = 10;
        [ProtoMember(14)]
        public double TimeSpan = 25;
    }
}

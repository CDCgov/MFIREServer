using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProtoBuf;

namespace MFireProtocol
{
    [ProtoContract]
	public class MFFan
	{
        [ProtoMember(1)]
		public int Number;
        [ProtoMember(2)]
        public int AirwayNo;
        [ProtoMember(3)]
        public List<double> AirflowData { get; set; }
        [ProtoMember(4)]
        public List<double> PressureData { get; set; }
    }
}

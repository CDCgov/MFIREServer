using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFRGetFanAirFlowData : MFireResult
    {
        [ProtoMember(1)]
        public List<double> AirflowData { get; set; }
    }
}

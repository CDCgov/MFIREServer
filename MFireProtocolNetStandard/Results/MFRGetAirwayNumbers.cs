using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFRGetAirwayNumbers : MFireResult
    {
        [ProtoMember(1)]
        public List<int> AirwayNumbers { get; set; }
    }
}

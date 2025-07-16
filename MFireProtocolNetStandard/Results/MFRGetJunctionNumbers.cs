using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFRGetJunctionNumbers : MFireResult
    {
        [ProtoMember(1)]
        public List<int> JunctionNumbers { get; set; }
    }
}

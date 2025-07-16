using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFRGetSimulationSnapshot : MFireResult
    {
        [ProtoMember(1)]
        public Dictionary<string, object> Snapshot { get; set; }
    }
}

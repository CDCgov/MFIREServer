using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFRGetEngineState : MFireResult
    {
        [ProtoMember(1)]
        public EngineState EngineState { get; set; }
    }
}

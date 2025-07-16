using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFRConfigChanged : MFireResult
    {
        [ProtoMember(1)]
        public bool ConfigChanged { get; set; }
    }
}

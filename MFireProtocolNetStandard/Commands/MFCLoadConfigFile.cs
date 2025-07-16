using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCLoadConfigFile : MFireCmd
    {
        [ProtoMember(1)]
        public string FileName { get; set; }
    }
}

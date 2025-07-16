using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFRIsResuming : MFireResult
    {
        [ProtoMember(1)]
        public bool IsResuming { get; set; }
    }
}

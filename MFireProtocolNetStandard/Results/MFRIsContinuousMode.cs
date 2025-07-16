using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFRIsContinuousMode : MFireResult
    {
        [ProtoMember(1)]
        public bool IsContinuous { get; set; }
    }
}

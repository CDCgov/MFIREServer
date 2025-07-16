using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFCSetInputUnits : MFireCmd
    {
        [ProtoMember(1)]
        public EngineeringUnits EngineeringUnits { get; set; }
    }
}

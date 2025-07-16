using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public class MFRGetFanPressureData : MFireResult
    {
        [ProtoMember(1)]
        public List<double> PressureData { get; set; }
    }
}

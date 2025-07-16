using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public enum EngineeringUnits
    {
        Imperial = 0,
        Metric = 1
    }
}

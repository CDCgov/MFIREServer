using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
   public class MFRGetResumeTime : MFireResult
    {
        [ProtoMember(1)]
        public double ResumeTime { get; set; }
    }
}

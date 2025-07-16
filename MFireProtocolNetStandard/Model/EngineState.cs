using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public enum EngineState
    {
        STOPPED = 0,
        RUNNING = 1,
        PAUSED = 2,
        COMPLETED = 3,
        CONVERGENCE_FAILURE = 4,
        FATAL_ERROR = 5
    }
}

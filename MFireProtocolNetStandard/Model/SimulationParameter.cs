using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFireProtocol
{
    [ProtoContract]
    public enum SimulationParameter
    {
        AutoStart = 0,
        OmitJunctions = 1,
        NetworkOnly = 2,
        ReferenceTemperature = 3,
        MaxTemperatureIterations = 4,
        MaxDynamicIterations = 5,
        TemperatureOnly = 6,
        TimeIncrement = 7,
        TimeSpan = 8,
        OutputDetail = 9,
        TimeIntervalForReport = 10,
        TransientState = 11,
        ReferenceDensity = 12,
        FanCurveBoundry = 13,
        StartJunction = 14,
        StartJunctionTemperature = 15,
        TimeEquillibrium = 16,
        FumeCriteria = 17,
        CH4Criteria = 18,
        TemperatureCriteria = 19,
        PressureDropWarningLimit = 20,
        FumeWarningLimit = 21,
        CH4WarningLimit = 22,
        TemperatureLimit = 23,
        AverageDiffusivity = 24,
        AverageConductivity = 25,
        AverageFrictionFactor = 26,
        AverageLength = 27,
        AverageCrossSection = 28,
        AveragePerimiter = 29
    }
}

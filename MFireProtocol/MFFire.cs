using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MFireProtocol
{
	public class MFFire
	{
		public int Number;
		public int AirwayNo;
		public double ContamFlowRate;
		public double ContamConcentration;
		public double HeatInput;
		public double O2ConcLeavingFire;
		public double ContamPerCuFtO2;
		public double HeatPerCuFtO2;
		public double StandardAirFlow;
		public double TransitionTime;

		public void Serialize(BinaryWriter writer)
		{
			writer.Write(Number);
			writer.Write(AirwayNo);
			writer.Write(ContamFlowRate);
			writer.Write(ContamConcentration);
			writer.Write(HeatInput);
			writer.Write(O2ConcLeavingFire);
			writer.Write(ContamPerCuFtO2);
			writer.Write(HeatPerCuFtO2);
			writer.Write(StandardAirFlow);
			writer.Write(TransitionTime);
		}

		public void Deserialize(BinaryReader reader)
		{
			Number = reader.ReadInt32();
			AirwayNo = reader.ReadInt32();
			ContamFlowRate = reader.ReadDouble();
			ContamConcentration = reader.ReadDouble();
			HeatInput = reader.ReadDouble();
			O2ConcLeavingFire = reader.ReadDouble();
			ContamPerCuFtO2 = reader.ReadDouble();
			HeatPerCuFtO2 = reader.ReadDouble();
			StandardAirFlow = reader.ReadDouble();
			TransitionTime = reader.ReadDouble();
		}
	}
}

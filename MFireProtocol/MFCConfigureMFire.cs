using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
	public class MFCConfigureMFire : MFireCmd
	{
		public int StartJunction = 1;
		public double StartJunctionTemperature = 50;
		public double TimeEquillibrium = 0.1f;
		public double FumeCriteria = 0.5f;
		public double CH4Criteria = 0.5f;
		public double TemperatureCriteria = 0.5f;
		public double PressureDropWarningLimit = 0.25f;
		public double FumeWarningLimit = 0.5f;
		public double CH4WarningLimit = 9.0f;
		public double TemperatureLimit = 900;
		public int AvgValueDataPresent = 0;
		public int MaxDynamicIterations = 10;
		public int MaxTemperatureIterations = 10;
		public double TimeSpan = 25;

		public override ulong GetCommandID()
		{
			return 88000;
		}

		public override void Deserialize(BinaryReader reader)
		{
			StartJunction = reader.ReadInt32();
			StartJunctionTemperature = reader.ReadDouble();
			TimeEquillibrium = reader.ReadDouble();
			FumeCriteria = reader.ReadDouble();
			CH4Criteria = reader.ReadDouble();
			TemperatureCriteria = reader.ReadDouble();
			PressureDropWarningLimit = reader.ReadDouble();
			FumeWarningLimit = reader.ReadDouble();
			CH4WarningLimit = reader.ReadDouble();
			TemperatureLimit = reader.ReadDouble();
			AvgValueDataPresent = reader.ReadInt32();
			MaxDynamicIterations = reader.ReadInt32();
			MaxTemperatureIterations = reader.ReadInt32();
			TimeSpan = reader.ReadDouble();
		}

		public override void Serialize(BinaryWriter writer)
		{
			writer.Write(StartJunction);
			writer.Write(StartJunctionTemperature);
			writer.Write(TimeEquillibrium);
			writer.Write(FumeCriteria);
			writer.Write(CH4Criteria);
			writer.Write(TemperatureCriteria);
			writer.Write(PressureDropWarningLimit);
			writer.Write(FumeWarningLimit);
			writer.Write(CH4WarningLimit);
			writer.Write(TemperatureLimit);
			writer.Write(AvgValueDataPresent);
			writer.Write(MaxDynamicIterations);
			writer.Write(MaxTemperatureIterations);
			writer.Write(TimeSpan);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MFireProtocol
{
	public class MFAirway
	{
		public int Number;
		public int StartJunction;
		public int EndJunction;
		public int Type;
		public int FlowDirection;

		public double CH4EmissionRateAirway;
		public double CH4EmissionRateSurfArea;
		public double FlowRate;
		public double FrictionFactor;
		public double Length;
		public double CrossSectionalArea;
		public double Perimeter;
		public double Resistance;
		public double RockTemperature;
		public double ThermalConductivity;
		public double ThermalDefusivity;

		public void Serialize(BinaryWriter writer)
		{
			writer.Write(Number);
			writer.Write(StartJunction);
			writer.Write(EndJunction);
			writer.Write(Type);
			writer.Write(CH4EmissionRateAirway);
			writer.Write(CH4EmissionRateSurfArea);
			writer.Write(FlowRate);
			writer.Write(FrictionFactor);
			writer.Write(Length);
			writer.Write(CrossSectionalArea);
			writer.Write(Perimeter);
			writer.Write(Resistance);
			writer.Write(RockTemperature);
			writer.Write(ThermalConductivity);
			writer.Write(ThermalDefusivity);
			writer.Write(FlowDirection);
		}

		public void Deserialize(BinaryReader reader)
		{
			Number = reader.ReadInt32();
			StartJunction = reader.ReadInt32();
			EndJunction = reader.ReadInt32();
			Type = reader.ReadInt32();

			CH4EmissionRateAirway = reader.ReadDouble();
			CH4EmissionRateSurfArea = reader.ReadDouble();
			FlowRate = reader.ReadDouble();
			FrictionFactor = reader.ReadDouble();
			Length = reader.ReadDouble();
			CrossSectionalArea = reader.ReadDouble();
			Perimeter = reader.ReadDouble();
			Resistance = reader.ReadDouble();
			RockTemperature = reader.ReadDouble();
			ThermalConductivity = reader.ReadDouble();
			ThermalDefusivity = reader.ReadDouble();
			FlowDirection = reader.ReadInt32();
		}
		
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MFireProtocol
{
	public class MFJunction
	{
		//input data
		public int Number;
		public int AtmosphereJuncType;
		public bool IsInAtmosphere;
		public double CH4InitialConc;
		public double Elevation;
		public double Temperature;

		//calculation results
		public bool ConditionChanged;
		public double TotalHeat;
		public double AtmosphereTemperature;
		public double TemperatureInter;
		public double TemperatureBkp;
		public double ContamConcentrationBkp;
		public double CH4ConcentrationBkp;
		public double TotalCH4;
		public double TotalContaminant;
		public double TotalAirFlow;
		public double CH4Concentration;
		public double ContamConcentration;

		public void Serialize(BinaryWriter writer)
		{
			writer.Write(Number);
			writer.Write(AtmosphereJuncType);
			writer.Write(IsInAtmosphere);
			writer.Write(CH4InitialConc);
			writer.Write(Elevation);
			writer.Write(Temperature);

			writer.Write(ConditionChanged);
			writer.Write(TotalHeat);
			writer.Write(AtmosphereTemperature);
			writer.Write(TemperatureInter);
			writer.Write(TemperatureBkp);
			writer.Write(ContamConcentrationBkp);
			writer.Write(CH4ConcentrationBkp);
			writer.Write(TotalCH4);
			writer.Write(TotalContaminant);
			writer.Write(TotalAirFlow);
			writer.Write(CH4Concentration);
			writer.Write(ContamConcentration);
		}

		public void Deserialize(BinaryReader reader)
		{
			Number = reader.ReadInt32();
			AtmosphereJuncType = reader.ReadInt32();
			IsInAtmosphere = reader.ReadBoolean();
			CH4InitialConc = reader.ReadDouble();
			Elevation = reader.ReadDouble();
			Temperature = reader.ReadDouble();

			ConditionChanged = reader.ReadBoolean();
			TotalHeat = reader.ReadDouble();
			AtmosphereTemperature = reader.ReadDouble();
			TemperatureInter = reader.ReadDouble();
			TemperatureBkp = reader.ReadDouble();
			ContamConcentrationBkp = reader.ReadDouble();
			CH4ConcentrationBkp = reader.ReadDouble();
			TotalCH4 = reader.ReadDouble();
			TotalContaminant = reader.ReadDouble();
			TotalAirFlow = reader.ReadDouble();
			CH4Concentration = reader.ReadDouble();
			ContamConcentration = reader.ReadDouble();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFireDLL;
using MFireProtocol;

namespace MFireServer
{
	static class MFHelper
	{

		public static void CopyAirwayData(MFAirway from, Airway to)
		{			
			to.CH4EmissionRateAirway = from.CH4EmissionRateAirway;
			to.CH4EmissionRateSurfArea = from.CH4EmissionRateSurfArea;
			to.CrossSectionalArea = from.CrossSectionalArea;
			to.EndJunction = from.EndJunction;
			to.FlowRate = from.FlowRate;
			to.FrictionFactor = from.FrictionFactor;
			to.Length = from.Length;
			to.Number = from.Number;
			to.Perimeter = from.Perimeter;
			to.Resistance = from.Resistance;
			to.RockTemperature = from.RockTemperature;
			to.StartJunction = from.StartJunction;
			to.ThermalConductivity = from.ThermalConductivity;
			to.ThermalDefusivity = from.ThermalDefusivity;
			to.Type = from.Type;
		}

		public static void CopyAirwayData(Airway from, MFAirway to)
		{
			to.CH4EmissionRateAirway = from.CH4EmissionRateAirway;
			to.CH4EmissionRateSurfArea = from.CH4EmissionRateSurfArea;
			to.CrossSectionalArea = from.CrossSectionalArea;
			to.EndJunction = from.EndJunction;
			to.FlowRate = from.FlowRate;
			to.FrictionFactor = from.FrictionFactor;
			to.Length = from.Length;
			to.Number = from.Number;
			to.Perimeter = from.Perimeter;
			to.Resistance = from.Resistance;
			to.RockTemperature = from.RockTemperature;
			to.StartJunction = from.StartJunction;
			to.ThermalConductivity = from.ThermalConductivity;
			to.ThermalDefusivity = from.ThermalDefusivity;
			to.Type = from.Type;
		}

		public static void CopyJunctionData(MFJunction from, Junction to)
		{
			to.AtmosphereJuncType = from.AtmosphereJuncType;
			to.CH4InitialConc = from.CH4InitialConc;
			to.Elevation = from.Elevation;
			to.IsInAtmosphere = from.IsInAtmosphere;
			to.Number = from.Number;
			to.Temperature = from.Temperature;

			to.ConditionChanged = from.ConditionChanged;
			to.TotalHeat = from.TotalHeat;
			to.AtmosphereTemperature = from.AtmosphereTemperature;
			to.TemperatureInter = from.TemperatureInter;
			to.TemperatureBkp = from.TemperatureBkp;
			to.ContamConcentrationBkp = from.ContamConcentrationBkp;
			to.CH4ConcentrationBkp = from.CH4ConcentrationBkp;
			to.TotalCH4 = from.TotalCH4;
			to.TotalContaminant = from.TotalContaminant;
			to.TotalAirFlow = from.TotalAirFlow;
			to.CH4Concentration = from.CH4Concentration;
			to.ContamConcentration = from.ContamConcentration;
		}

		public static void CopyJunctionData(Junction from, MFJunction to)
		{
			to.AtmosphereJuncType = from.AtmosphereJuncType;
			to.CH4InitialConc = from.CH4InitialConc;
			to.Elevation = from.Elevation;
			to.IsInAtmosphere = from.IsInAtmosphere;
			to.Number = from.Number;
			to.Temperature = from.Temperature;

			to.ConditionChanged = from.ConditionChanged;
			to.TotalHeat = from.TotalHeat;
			to.AtmosphereTemperature = from.AtmosphereTemperature;
			to.TemperatureInter = from.TemperatureInter;
			to.TemperatureBkp = from.TemperatureBkp;
			to.ContamConcentrationBkp = from.ContamConcentrationBkp;
			to.CH4ConcentrationBkp = from.CH4ConcentrationBkp;
			to.TotalCH4 = from.TotalCH4;
			to.TotalContaminant = from.TotalContaminant;
			to.TotalAirFlow = from.TotalAirFlow;
			to.CH4Concentration = from.CH4Concentration;
			to.ContamConcentration = from.ContamConcentration;
		}
		
		public static void CopyFireData(MFFire from, Fire to)
		{			
			to.AirwayNo = from.AirwayNo;
			to.ContamFlowRate = from.ContamFlowRate;
			to.ContamConcentration = from.ContamConcentration;
			to.HeatInput = from.HeatInput;
			to.O2ConcLeavingFire = from.O2ConcLeavingFire;
			to.ContamPerCuFtO2 = from.ContamPerCuFtO2;
			to.HeatPerCuFtO2 = from.HeatPerCuFtO2;
			to.StandardAirFlow = from.StandardAirFlow;
			to.TransitionTime = from.TransitionTime;
		}

		public static void CopyFireData(Fire from, MFFire to)
		{
			to.AirwayNo = from.AirwayNo;
			to.ContamFlowRate = from.ContamFlowRate;
			to.ContamConcentration = from.ContamConcentration;
			to.HeatInput = from.HeatInput;
			to.O2ConcLeavingFire = from.O2ConcLeavingFire;
			to.ContamPerCuFtO2 = from.ContamPerCuFtO2;
			to.HeatPerCuFtO2 = from.HeatPerCuFtO2;
			to.StandardAirFlow = from.StandardAirFlow;
			to.TransitionTime = from.TransitionTime;
		}

		public static void AddConfigAirways(MFireConfig config, MFCUpdateMineState mineState)
		{
			if (mineState == null || mineState.Airways == null)
				return;

			AddConfigAirways(config, mineState.Airways);
		}

		public static void AddConfigAirways(MFireConfig config, List<MFAirway> airways)
		{
			config.NumOfAirways = airways.Count;
			if (config.Airways == null)
				config.Airways = new List<Airway>();

			config.Airways.Clear();

			foreach (MFAirway mfair in airways)
			{
				Airway airway = new Airway();
				CopyAirwayData(mfair, airway);
				config.Airways.Add(airway);
			}
		}

		public static void AddConfigAirways(MFireConfig config, Dictionary<int, MFAirway> airways)
		{
			config.NumOfAirways = airways.Count;
			if (config.Airways == null)
				config.Airways = new List<Airway>();

			config.Airways.Clear();

			foreach (MFAirway mfair in airways.Values)
			{
				Airway airway = new Airway();
				CopyAirwayData(mfair, airway);
				config.Airways.Add(airway);
			}
		}

		/*
		public static void AddConfigJunctions(MFireConfig config, MFCUpdateMineState mineState)
		{
			if (mineState == null || mineState.Junctions == null)
				return;

			AddConfigJunctions(config, mineState.Junctions);
		}

		public static void AddConfigJunctions(MFireConfig config, List<MFJunction> junctions)
		{
			config.NumOfJunctions = junctions.Count;
			if (config.Junctions == null)
				config.Junctions = new List<Junction>();

			config.Junctions.Clear();

			foreach (MFJunction mfj in junctions)
			{
				Junction junc = new Junction();
				junc.AtmosphereJuncType = mfj.AtmosphereJuncType;
				junc.CH4InitialConc = mfj.CH4InitialConc;
				junc.Elevation = mfj.Elevation;
				junc.IsInAtmosphere = mfj.IsInAtmosphere;
				junc.Number = mfj.Number;
				junc.Temperature = mfj.Temperature;

				config.Junctions.Add(junc);
			}
		}*/

		public static void AddConfigJunctions(MFireConfig config, Dictionary<int, MFJunction> junctions)
		{
			config.NumOfJunctions = junctions.Count;
			if (config.Junctions == null)
				config.Junctions = new List<Junction>();

			config.Junctions.Clear();

			foreach (MFJunction mfj in junctions.Values)
			{
				Junction junc = new Junction();
				MFHelper.CopyJunctionData(mfj, junc);
				config.Junctions.Add(junc);
			}
		}

		//public static void AddConfigFans(MFireConfig config, List<MFFan> fans)
		//{
		//	config.NumOfFans = fans.Count;
		//	config.Fans = new List<Fan>();

		//	foreach (MFFan fan in fans)
		//	{
		//		Fan f = new Fan();
		//		f.AirwayNo = fan.AirwayNo;
		//		f.CurveFittingMethod = 1;

		//		if (fan.AirflowData != null && fan.PressureData != null &&
		//			fan.AirflowData.Count >= 2 &&
		//			fan.AirflowData.Count == fan.PressureData.Count)
		//		{
		//			for (int i = 0; i < fan.AirflowData.Count; i++)
  //                  {
		//				AddFanData(f, fan.AirflowData[i], fan.PressureData[i]);
  //                  }
		//		}
		//		else
		//		{
		//			AddFanData(f, 20000, 3.6);
		//			AddFanData(f, 25000, 4.3);
		//			AddFanData(f, 30000, 4.6);
		//			AddFanData(f, 40000, 4.78);
		//			AddFanData(f, 55000, 4.58);
		//			AddFanData(f, 70000, 4.29);
		//			AddFanData(f, 85000, 3.96);
		//			AddFanData(f, 100000, 3.7);
		//			AddFanData(f, 150000, 3);
		//			AddFanData(f, 200000, 2.52);
		//		}

		//		config.Fans.Add(f);

		//		for (int i = 0; i < config.Airways.Count; i++)
		//		{
		//			if (config.Airways[i].Number == f.AirwayNo)
		//			{
		//				config.Airways[i].Type = 1;
		//			}
		//		}
		//	}
		//}

		public static void AddConfigFans(MFireConfig config, Dictionary<int, MFFan> fans)
		{
			config.NumOfFans = fans.Count;
			config.Fans = new List<Fan>();

			foreach (MFFan fan in fans.Values)
			{
				Fan f = new Fan();
				f.AirwayNo = fan.AirwayNo;
				//f.CurveFittingMethod = 1;
				f.CurveFittingMethod = 2; //changed to method 2 wjh 2021-05-19

				if (fan.AirflowData != null && fan.PressureData != null &&
				fan.AirflowData.Count >= 2 &&
				fan.AirflowData.Count == fan.PressureData.Count)
				{
					for (int i = 0; i < fan.AirflowData.Count; i++)
					{
						AddFanData(f, fan.AirflowData[i], fan.PressureData[i]);
					}
				}
				else
				{
                    //AddFanData(f, 20000, 3.6);
                    //AddFanData(f, 25000, 4.3);
                    //AddFanData(f, 30000, 4.6);
                    //AddFanData(f, 40000, 4.78);
                    //AddFanData(f, 55000, 4.58);
                    //AddFanData(f, 70000, 4.29);
                    //AddFanData(f, 85000, 3.96);
                    //AddFanData(f, 100000, 3.7);
                    //AddFanData(f, 150000, 3);
                    //AddFanData(f, 200000, 2.52);

                    AddFanData(f, 20000, 3.6);
                    //AddFanData(f, 25000, 4.3);
                    AddFanData(f, 30000, 4.6);
                    //AddFanData(f, 40000, 4.78);
                    AddFanData(f, 55000, 4.58);
                    //AddFanData(f, 70000, 4.29);
                    AddFanData(f, 85000, 3.96);
                    //AddFanData(f, 100000, 3.7);
                    AddFanData(f, 150000, 3);
                    //AddFanData(f, 200000, 2.52);
                }

				config.Fans.Add(f);

				for (int i = 0; i < config.Airways.Count; i++)
				{
					if (config.Airways[i].Number == f.AirwayNo)
					{
						config.Airways[i].Type = 1;
					}
				}
			}
		}

		//public static void AddConfigFans(MFireConfig config, MFCUpdateMineState mineState)
		//{
		//	/*config.NumOfFans = 0;
		//	config.Fans = new List<Fan>();
		//	return;*/

		//	config.NumOfFans = 1;
		//	config.Fans = new List<Fan>();
		//	Fan f1 = new Fan();
		//	f1.AirwayNo = 6;
		//	f1.CurveFittingMethod = 1;
		//	/*
		//	FanDataPoint p1 = new FanDataPoint();
		//	p1.AirFlowData = 10;
		//	p1.PressureData = 10;

		//	FanDataPoint p2 = new FanDataPoint();
		//	p2.AirFlowData = 20;
		//	p2.PressureData = 20;

		//	f1.Data = new List<FanDataPoint>();
		//	f1.Data.Add(p1);
		//	f1.Data.Add(p2);

		//	f1.DataCount = f1.Data.Count; */

		//	AddFanData(f1, 20000, 3.6);
		//	AddFanData(f1, 25000, 4.3);
		//	AddFanData(f1, 30000, 4.6);
		//	AddFanData(f1, 40000, 4.78);
		//	AddFanData(f1, 55000, 4.58);
		//	AddFanData(f1, 70000, 4.29);
		//	AddFanData(f1, 85000, 3.96);
		//	AddFanData(f1, 100000, 3.7);
		//	AddFanData(f1, 150000, 3);
		//	AddFanData(f1, 200000, 2.52);

		//	config.Fans.Add(f1);

		//	for (int i = 0; i < config.Airways.Count; i++)
		//	{
		//		if (config.Airways[i].Number == f1.AirwayNo)
		//		{
		//			config.Airways[i].Type = 1;
		//		}
		//	}
		//}

		public static void AddFanData(Fan fan, double airFlow, double pressure)
		{
			FanDataPoint p = new FanDataPoint();
			p.AirFlowData = airFlow;
			p.PressureData = pressure;

			if (fan.Data == null)
				fan.Data = new List<FanDataPoint>();

			fan.Data.Add(p);
			fan.DataCount = fan.Data.Count;
		}

		public static void AddConfigFires(MFireConfig config, Dictionary<int, MFFire> fires)
		{
			config.NumOfFires = fires.Count;
			config.Fires = new List<Fire>();

			foreach (MFFire fire in fires.Values)
			{
				Fire f = new Fire();
				MFHelper.CopyFireData(fire, f);

				config.Fires.Add(f);
			}
		}

		public static void UpdateConfigFire(MFireConfig config, MFFire mffire)
        {
			foreach (var fire in config.Fires)
            {
				if (fire.AirwayNo == mffire.AirwayNo)
                {
					MFHelper.CopyFireData(mffire, fire);
					break;
                }
            }
        }

		public static void SetConfigParameters(MFireConfig config, MFCConfigureMFire cmd)
		{
			config.StartJunction = cmd.StartJunction;
			config.StartJunctionTemperature = cmd.StartJunctionTemperature;
			config.TimeEquillibrium = cmd.TimeEquillibrium;
			config.FumeCriteria = cmd.FumeCriteria;
			config.CH4Criteria = cmd.CH4Criteria;
			config.TemperatureCriteria = cmd.TemperatureCriteria;
			config.PressureDropWarningLimit = cmd.PressureDropWarningLimit;
			config.FumeWarningLimit = cmd.FumeWarningLimit;
			config.CH4WarningLimit = cmd.CH4WarningLimit;
			config.TemperatureLimit = cmd.TemperatureLimit;
			config.AvgValueDataPresent = cmd.AvgValueDataPresent;
			config.MaxDynamicIterations = cmd.MaxDynamicIterations;
			config.MaxTemperatureIterations = cmd.MaxTemperatureIterations;
			config.TimeSpan = cmd.TimeSpan;
		}

		public static void UpdateMineStateData(MFCUpdateMineState mineState, MFireConfig config)
		{
			Dictionary<int, MFAirway> airwayTable = new Dictionary<int, MFAirway>();

			if (mineState.Airways == null || mineState.Airways.Count != config.Airways.Count)
			{
				mineState.Airways = new List<MFAirway>(config.Airways.Count);
				for (int i = 0; i < config.Airways.Count; i++)
					mineState.Airways.Add(new MFAirway());
			}

			if (mineState.Junctions == null || mineState.Junctions.Count != config.Junctions.Count)
			{
				mineState.Junctions = new List<MFJunction>(config.Junctions.Count);
				for (int i = 0; i < config.Junctions.Count; i++)
					mineState.Junctions.Add(new MFJunction());
			}

			for (int i = 0; i < config.Airways.Count; i++)
			{
				MFAirway mfa = mineState.Airways[i];
				Airway a = config.Airways[i];

				CopyAirwayData(a, mfa);

				airwayTable.Add(mfa.Number, mfa);
			}

			for (int i = 0; i < config.Junctions.Count; i++)
			{
				MFJunction mfj = mineState.Junctions[i];
				Junction j = config.Junctions[i];

				CopyJunctionData(j, mfj);
			}

			
			//pull flow direction from the mesh list
			if (config.Meshes != null && config.Meshes.Count > 0)
			{
				foreach (Mesh m in config.Meshes)
				{
					foreach (MeshEntry entry in m.MeshList)
					{
						MFAirway airway;
						if (airwayTable.TryGetValue(entry.AirwayIndex, out airway))
						{
							airway.FlowDirection = entry.FlowDirection;
						}
					}
				}
			} 
		}
	}
}

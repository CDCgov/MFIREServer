using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
	public class MFCRunSimulation : MFireCmd
	{
		public double SimulationTimestep;
		
		public override ulong GetCommandID()
		{
			return 70000;
		}

		public override void Deserialize(BinaryReader reader)
		{
			SimulationTimestep = reader.ReadDouble();
		}

		public override void Serialize(BinaryWriter writer)
		{
			writer.Write(SimulationTimestep);
		}
	}
}

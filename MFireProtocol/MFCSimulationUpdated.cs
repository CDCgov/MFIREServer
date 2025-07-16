using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
	public class MFCSimulationUpdated : MFireCmd
	{
		public double ElapsedTimeMs;

		public override ulong GetCommandID()
		{
			return 77000;
		}

		public override void Deserialize(BinaryReader reader)
		{
			ElapsedTimeMs = reader.ReadDouble();			
		}

		public override void Serialize(BinaryWriter writer)
		{
			writer.Write(ElapsedTimeMs);
		}
	}
}

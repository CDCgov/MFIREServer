using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
	public class MFCUpdateAirway : MFireCmd
	{
		public int AirwayIndex;
		public MFAirway Airway;

		public override ulong GetCommandID()
		{
			return 76200;
		}

		public override void Deserialize(BinaryReader reader)
		{
			AirwayIndex = reader.ReadInt32();

			if (Airway == null)
				Airway = new MFAirway();

			Airway.Deserialize(reader);
		}

		public override void Serialize(BinaryWriter writer)
		{
			writer.Write(AirwayIndex);

			if (Airway == null)
				Airway = new MFAirway(); //shouldn't happen...

			Airway.Serialize(writer);			
		}
	}
}

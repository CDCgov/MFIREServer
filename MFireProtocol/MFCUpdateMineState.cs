using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
	public class MFCUpdateMineState : MFireCmd
	{
		public List<MFAirway> Airways;
		public List<MFJunction> Junctions;

		public override ulong GetCommandID()
		{
			return 76000;
		}

		public override void Deserialize(BinaryReader reader)
		{
			int numAirways = reader.ReadInt32();

			if (Airways == null || Airways.Count != numAirways)
				Airways = new List<MFAirway>(numAirways);

			for (int i = 0; i < numAirways; i++)
			{
				if (i >= Airways.Count)
					Airways.Add(new MFAirway());
				else if (Airways[i] == null)
					Airways[i] = new MFAirway();

				Airways[i].Deserialize(reader);
			}

			int numJunctions = reader.ReadInt32();

			if (Junctions == null || Junctions.Count != numJunctions)
				Junctions = new List<MFJunction>(numJunctions);

			for (int j = 0; j < numJunctions; j++)
			{
				if (j >= Junctions.Count)
					Junctions.Add(new MFJunction());
				else if (Junctions[j] == null)
					Junctions[j] = new MFJunction();

				Junctions[j].Deserialize(reader);
			}
		}

		public override void Serialize(BinaryWriter writer)
		{
			if (Airways == null)
				Airways = new List<MFAirway>();

			if (Junctions == null)
				Junctions = new List<MFJunction>();

			int numAirways = Airways.Count;
			int numJunctions = Junctions.Count;

			writer.Write(numAirways);
			for (int i = 0; i < numAirways; i++)
				Airways[i].Serialize(writer);

			writer.Write(numJunctions);
			for (int j = 0; j < numJunctions; j++)
				Junctions[j].Serialize(writer);
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
	public class MFCUpdateJunction : MFireCmd
	{
		public int JunctionIndex;
		public MFJunction Junction;

		public override ulong GetCommandID()
		{
			return 76100;
		}

		public override void Deserialize(BinaryReader reader)
		{
			JunctionIndex = reader.ReadInt32();

			if (Junction == null)
				Junction = new MFJunction();

			Junction.Deserialize(reader);
		}

		public override void Serialize(BinaryWriter writer)
		{
			writer.Write(JunctionIndex);

			if (Junction == null)
				Junction = new MFJunction(); //shouldn't happen...

			Junction.Serialize(writer);
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
	public class MFCUpdateFan : MFireCmd
	{
		public int FanIndex;
		public MFFan Fan;
		
		
		public override ulong GetCommandID()
		{
			return 76300;
		}

		public override void Deserialize(BinaryReader reader)
		{
			FanIndex = reader.ReadInt32();

			if (Fan == null)
				Fan = new MFFan();

			Fan.Deserialize(reader);
		}

		public override void Serialize(BinaryWriter writer)
		{
			writer.Write(FanIndex);

			if (Fan == null)
				Fan = new MFFan(); //shouldn't happen...

			Fan.Serialize(writer);
		}
	}
}

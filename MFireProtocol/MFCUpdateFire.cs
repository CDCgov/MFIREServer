using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
	public class MFCUpdateFire : MFireCmd
	{
		public int FireIndex;
		public MFFire Fire;


		public override ulong GetCommandID()
		{
			return 76400;
		}

		public override void Deserialize(BinaryReader reader)
		{
			FireIndex = reader.ReadInt32();

			if (Fire == null)
				Fire = new MFFire();

			Fire.Deserialize(reader);
		}

		public override void Serialize(BinaryWriter writer)
		{
			writer.Write(FireIndex);

			if (Fire == null)
				Fire = new MFFire(); //shouldn't happen...

			Fire.Serialize(writer);
		}
	}
}

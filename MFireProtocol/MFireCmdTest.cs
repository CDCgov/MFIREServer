using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
	public class MFireCmdTest : MFireCmd
	{
		public int Int1;
		public int Int2;
		public string TestString;

		public override ulong GetCommandID()
		{
			return 90000;
		}

		public override void Deserialize(BinaryReader reader)
		{
			Int1 = reader.ReadInt32();
			Int2 = reader.ReadInt32();
			TestString = reader.ReadString();
		}

		public override void Serialize(BinaryWriter writer)
		{
			writer.Write(Int1);
			writer.Write(Int2);

			if (TestString == null)
				TestString = "";

			writer.Write(TestString);
		}
	}
}

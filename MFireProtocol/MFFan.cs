using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MFireProtocol
{
	public class MFFan
	{
		public int Number;
		public int AirwayNo;

		public void Serialize(BinaryWriter writer)
		{
			writer.Write(Number);
			writer.Write(AirwayNo);
		}

		public void Deserialize(BinaryReader reader)
		{
			Number = reader.ReadInt32();
			AirwayNo = reader.ReadInt32();
		}
	}
}

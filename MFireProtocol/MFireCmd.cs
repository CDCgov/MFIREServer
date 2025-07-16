using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MFireProtocol
{
	public abstract class MFireCmd 
	{
		public abstract UInt64 GetCommandID();

		public abstract void Serialize(BinaryWriter writer);
		public abstract void Deserialize(BinaryReader reader);
	}
}

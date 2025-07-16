using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
	public class MFCResetSimulation : MFireCmd
	{

		public override ulong GetCommandID()
		{
			return 75000;
		}

		public override void Deserialize(BinaryReader reader)
		{
			
		}

		public override void Serialize(BinaryWriter writer)
		{
			
		}
	}
}

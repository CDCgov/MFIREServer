using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MFireProtocol
{
	public class MFireCmdTerminateServer : MFireCmd
	{
		public override ulong GetCommandID()
		{
			return 5001;
		}

		public override void Deserialize(BinaryReader reader)
		{
			
		}

		public override void Serialize(BinaryWriter writer)
		{
			
		}
	}
}

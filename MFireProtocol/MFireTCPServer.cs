using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MFireProtocol
{
	public class MFireTCPServer : TCPServer
	{
		protected override TCPConnectionHandler CreateConnectionHandler(TcpClient client)
		{
			var handler = new MFireConnection(client);
			return (TCPConnectionHandler)handler;
		}

		
	}
}

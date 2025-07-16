using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using MFireProtocol;

namespace MFireUnitTests
{
	[TestClass]
	public class TCPServerTests
	{
		
		[TestMethod]
		public void TestMethod1()
		{
		}

		[TestMethod]
		public void TestServerStartStop()
		{
			using (MFireTCPServer server = new MFireTCPServer())
			{
				server.StartServer();
			}

			//dispose should stop the server
		}

		[TestMethod]
		public void TestServerConnect()
		{
			using (MFireTCPServer server = new MFireTCPServer())
			using (MFireConnection conn = new MFireConnection())
			{
				server.StartServer();
				bool connected = conn.Connect("localhost");
				if (!connected)
					throw new Exception("Connect to server failed");

				conn.SendTest();
			}

		}


		private class TestPacketReceivedHandler
		{
			public bool PacketetReceived = false;
			public void TestPacketReceivedOnPacketReceived()
			{
				System.Diagnostics.Debug.Print("Packet Received");
				PacketetReceived = true;
			}
		}
		
		[TestMethod]
		public void TestPacketReceived()
		{
			using (MFireTCPServer server = new MFireTCPServer())
			using (MFireConnection conn = new MFireConnection())
			{
				System.Diagnostics.Debug.Print("Test starting");
				server.StartServer(3445);
				bool connected = conn.Connect("localhost", 3445);
				if (!connected)
					throw new Exception("Connect to server failed");
				
				Assert.AreEqual(server.GetNumConnections(), 1);	

				var msgHandler = new TestPacketReceivedHandler();
				conn.PacketReceived += msgHandler.TestPacketReceivedOnPacketReceived;

				MFireConnection client = (MFireConnection)server.GetConnectedClient(0);
				client.SendTest();

				Thread.Sleep(200);


				System.Diagnostics.Debug.Print("Checking for packet");
				Assert.IsTrue(msgHandler.PacketetReceived);
			}
		}

		private class TestMFireCmdReceived
		{
			public int NumCmdsReceived = 0;

			public void OnMFireCmdReceived(MFireConnection obj)
			{
				MFireCmd cmd;

				//MFireCmd cmd = obj.DequeueReceivedCmd();
				while ((cmd = obj.DequeueReceivedCmd()) != null)
				{
					NumCmdsReceived++;
				}
			}
		}

		[TestMethod]
		public void TestMFireCommandSpam()
		{
			using (MFireTCPServer server = new MFireTCPServer())
			using (MFireConnection conn = new MFireConnection())
			{
				System.Diagnostics.Debug.Print("Test starting");
				server.StartServer(3445);
				bool connected = conn.Connect("localhost", 3445);
				if (!connected)
					throw new Exception("Connect to server failed");

				Thread.Sleep(100);
				Assert.AreEqual(1, server.GetNumConnections());

				var msgHandler = new TestMFireCmdReceived();
				conn.MFireCmdReceived += msgHandler.OnMFireCmdReceived;

				MFireConnection client = (MFireConnection)server.GetConnectedClient(0);

				const int numTestCommands = 100;
				MFireCmdTest cmdTest = new MFireCmdTest();
				cmdTest.Int2 = 42;
				cmdTest.TestString = "Hello World";
				for (int i = 0; i < numTestCommands; i++)
				{
					cmdTest.Int1 = i;
					client.SendMFireCmd(cmdTest);
					if (i % 10 == 0)
						Thread.Sleep(1);
				}

				Thread.Sleep(100);
				Assert.AreEqual(numTestCommands, msgHandler.NumCmdsReceived);
			}
		}

		[TestMethod]
		public void TestCommandMap()
		{
            var id = CommandIds.GetId(typeof(MFCConfigureMFire));
            var t = CommandIds.GetType(id);
            Assert.AreEqual((ulong)1, id);
            Assert.AreEqual(typeof(MFCConfigureMFire), t);
		}

		[TestMethod]
		public void TestMFireCmd()
		{
			using (MFireTCPServer server = new MFireTCPServer())
			using (MFireConnection conn = new MFireConnection())
			{
				server.StartServer();
				bool connected = conn.Connect("localhost");
				if (!connected)
					throw new Exception("Connect to server failed");

				Thread.Sleep(100);

				Assert.AreEqual(1, server.GetNumConnections());
				

				MFireConnection client = (MFireConnection)server.GetConnectedClient(0);
				MFireCmdTest cmd = new MFireCmdTest();
				cmd.Int1 = 42;
				cmd.Int2 = 78395;
				cmd.TestString = "Hello World";
				client.SendMFireCmd(cmd);


				Thread.Sleep(200);

				System.Diagnostics.Debug.Print("Checking for packet");

				MFireCmdTest recvCommand = conn.DequeueReceivedCmd() as MFireCmdTest;
				Assert.IsNotNull(recvCommand);

				System.Diagnostics.Debug.Print("Got {0} {1} {2}", recvCommand.Int1, recvCommand.Int2, 
					recvCommand.TestString);

				Assert.AreEqual(recvCommand.Int1, cmd.Int1);
				Assert.AreEqual(recvCommand.Int2, cmd.Int2);
				Assert.AreEqual(recvCommand.TestString, cmd.TestString);
			}
		}

		[TestMethod]
		public void TestSmallCommand()
		{
			using (MFireTCPServer server = new MFireTCPServer())
			using (MFireConnection conn = new MFireConnection())
			{
				server.StartServer();
				bool connected = conn.Connect("localhost");
				if (!connected)
					throw new Exception("Connect to server failed");

				Thread.Sleep(100);

				Assert.AreEqual(1, server.GetNumConnections());
				MFireConnection client = (MFireConnection)server.GetConnectedClient(0);

				for (int i = 0; i < 5; i++)
				{
					client.SendRunSimulation();

					Thread.Sleep(200);

					System.Diagnostics.Debug.Print("Checking for packet");

					MFCRunSimulation recvCommand = conn.DequeueReceivedCmd() as MFCRunSimulation;
					Assert.IsNotNull(recvCommand);

					System.Diagnostics.Debug.Print("Got Packet");

					Thread.Sleep(300);
				}
			}

		}

		[TestMethod]
		public void TestSimpleMFireConfig()
		{		

		}

	}
}

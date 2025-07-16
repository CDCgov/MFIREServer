using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace MFireProtocol
{
	public class MFireConnection : TCPConnectionHandler
	{

		public static List<Type> FindAllDerivedTypes<T>()
		{
			return FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(T)));
		}

		public static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
		{
			var derivedType = typeof(T);
			return assembly
				.GetTypes()
				.Where(t =>
					t != derivedType &&
					derivedType.IsAssignableFrom(t)
					).ToList();

		}

		public event Action<MFireConnection> MFireCmdReceived;


		private static Queue<MFireCmd> _receiveQueue;
		private static Dictionary<UInt64, Type> _commandMap;

		public MFireConnection() : base()
		{
			InitializeCommandMap();
		}

		public MFireConnection(TcpClient client) : base(client)
		{
			InitializeCommandMap();
		}

		private void InitializeCommandMap()
		{
			if (_receiveQueue == null)
				_receiveQueue = new Queue<MFireCmd>();

			if (_commandMap != null)
				return;

			_commandMap = new Dictionary<ulong, Type>();
			var cmdTypes = FindAllDerivedTypes<MFireCmd>();

			for (int i = 0; i < cmdTypes.Count; i++)
			{
				Type t = cmdTypes[i];

				MFireCmd cmd = (MFireCmd)Activator.CreateInstance(t);
				UInt64 cmdID = cmd.GetCommandID();

				_commandMap.Add(cmdID, t);
			}
		}

		public Type LookupCommand(UInt64 commandID)
		{
			if (_commandMap == null)
				return null;

			Type result;
			if (_commandMap.TryGetValue(commandID, out result))
				return result;
			else
				return null;
		}

		//TODO: Change all the send commands to write directly to the stream
		//or at least do something to minimize the # of allocations...

		public void SendResetSimulation()
		{
			SendMFireCmd(new MFCResetSimulation());
		}

		public void SendTerminateServer()
		{
			SendMFireCmd(new MFireCmdTerminateServer());
		}

		public void SendRunSimulation()
		{
			var cmd = new MFCRunSimulation();
			SendMFireCmd(cmd);
		}

		public void SendUpdateAirway(int airwayIndex, MFAirway airway)
		{
			MFCUpdateAirway cmd = new MFCUpdateAirway();
			cmd.Airway = airway;
			cmd.AirwayIndex = airwayIndex;
			SendMFireCmd(cmd);
		}

		public void SendUpdateJunction(int junctionIndex, MFJunction junction)
		{
			MFCUpdateJunction cmd = new MFCUpdateJunction();
			cmd.JunctionIndex = junctionIndex;
			cmd.Junction = junction;
			SendMFireCmd(cmd);
		}

		public void SendUpdateFan(int fanIndex, MFFan fan)
		{
			MFCUpdateFan cmd = new MFCUpdateFan();
			cmd.FanIndex = fanIndex;
			cmd.Fan = fan;
			SendMFireCmd(cmd);
		}

		public void SendUpdateFire(int fireIndex, MFFire fire)
		{
			MFCUpdateFire cmd = new MFCUpdateFire();
			cmd.FireIndex = fireIndex;
			cmd.Fire = fire;
			SendMFireCmd(cmd);
		}

		public void SendSimulationUpdated(double elapsedTime)
		{
			MFCSimulationUpdated cmd = new MFCSimulationUpdated();
			cmd.ElapsedTimeMs = elapsedTime;
			SendMFireCmd(cmd);
		}

		public void SendMFireCmd(MFireCmd cmd)
		{
			using (MemoryStream memStream = new MemoryStream())
			{
				BinaryWriter writer = new BinaryWriter(memStream);

				cmd.Serialize(writer);

				byte[] data = memStream.ToArray();
				SendPacket(cmd.GetCommandID(), data);
			}
		}

		public void SendMFireCmdTest(int int1, int int2, string message)
		{
			MFireCmdTest cmd = new MFireCmdTest();
			cmd.Int1 = int1;
			cmd.Int2 = int2;
			cmd.TestString = message;

			SendMFireCmd(cmd);
		}

		public void SendTest()
		{
			byte[] testData = new byte[10];
			for (int i = 0; i < testData.Length; i++)
				testData[i] = (byte)i;

			SendPacket(999, testData);
		}

		private void RaiseMFireCmdReceived()
		{
			var handler = MFireCmdReceived;
			if (handler != null)
			{
				handler(this);
			}
		}

		public MFireCmd DequeueReceivedCmd()
		{
			MFireCmd cmd = null;

			lock (_receiveQueue)
			{
				if (_receiveQueue.Count > 0)
					cmd = _receiveQueue.Dequeue();
			}

			return cmd;
		}

		protected override void DecodePacket(UInt64 packetID, BinaryReader reader, UInt32 packetSize)
		{
			Debug.Print("Decoded Packet ID: {0}", packetID);

			Type cmdType = LookupCommand(packetID);
			if (cmdType == null)
				return;

			MFireCmd cmd = (MFireCmd)Activator.CreateInstance(cmdType);
			cmd.Deserialize(reader);

			lock (_receiveQueue)
			{
				_receiveQueue.Enqueue(cmd);
			}

			RaiseMFireCmdReceived();
		}
		
	}
}

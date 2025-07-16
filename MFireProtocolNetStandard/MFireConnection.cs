using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using ProtoBuf;
using System.Threading.Tasks;

namespace MFireProtocol
{
	public class MFireConnection : TCPConnectionHandler
	{
		public event Action<MFireConnection> MFireCmdReceived;
		public event Action<string> PacketDecodeError;


		private static Queue<MFireCmd> _receiveQueue = new Queue<MFireCmd>();
        private static object _lastCommandLock = new object();
        private static MFireCmd _lastCommand;

		public MFireConnection() : base()
		{
		}

        public MFireConnection(TcpClient client) : base(client)
		{
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
                var cmdId = CommandIds.GetId(cmd.GetType());
                Serializer.NonGeneric.SerializeWithLengthPrefix(memStream, cmd, PrefixStyle.Base128, (int)cmdId);

				byte[] data = memStream.ToArray();
				SendPacket(cmdId, data);
			}
		}

        public async Task<MFireResult> SendMFireCmdWithResult<T>(MFireCmd cmd) where T : MFireResult
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                var cmdId = CommandIds.GetId(cmd.GetType());
                Serializer.NonGeneric.SerializeWithLengthPrefix(memStream, cmd, PrefixStyle.Base128, (int)cmdId);

                byte[] data = memStream.ToArray();
                SendPacket(cmdId, data);
                return await waitForResult<T>();
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

        private async Task<T> waitForResult<T>() where T : MFireResult
        {
            while (_lastCommand == null || _lastCommand.GetType() != typeof(T))
            {
                await Task.Delay(1);   
            }
            lock (_lastCommandLock)
            {
                var cmd = _lastCommand;
                _lastCommand = null;
                return (T)cmd;
            }
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

		protected override void DecodePacket(Stream s)
		{
			try
			{
				Serializer.NonGeneric.TryDeserializeWithLengthPrefix(s, PrefixStyle.Base128, (id) => CommandIds.GetType((ulong)id), out object cmd);
				Debug.Print("Decoded Packet ID: {0}", CommandIds.GetId(cmd.GetType()));

				lock (_receiveQueue)
				{
					_receiveQueue.Enqueue((MFireCmd)cmd);
				}
				lock (_lastCommandLock)
					_lastCommand = (MFireCmd)cmd;

				RaiseMFireCmdReceived();
			}
			catch (Exception ex)
			{
				PacketDecodeError?.Invoke($"MFire Packet Decode Error - {ex.Message} - {ex.StackTrace}");
			}
		}
		
	}
}

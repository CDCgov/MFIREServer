using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace MFireProtocol
{
	public abstract class TCPConnectionHandler : IDisposable
	{
		private const int BUFFER_SIZE = 1500000;


		public event Action<TCPConnectionHandler> ClientDisconnected;
		public event Action<UInt64> PacketReceived;

		private TcpClient _client;
		private Thread _thread;
		private ManualResetEvent _terminateThread;
		private byte[] _buffer;
		private MemoryStream _bufferStream;
		private BinaryReader _bufferReader;
		private uint _bufferedBytes;

		private object _sendQueueLock;
		private Queue<PacketData> _sendQueue;
		private ManualResetEvent _sendQueueHasData;

		private bool _hasDisconnected = false;

		private struct PacketData
		{
			public UInt64 PacketID;
			public byte[] Data;
		}

		public TCPConnectionHandler()
		{
			_terminateThread = new ManualResetEvent(false);
		}

		public TCPConnectionHandler(TcpClient client) : this()
		{			
			Initialize(client);
		}

		public void Initialize(TcpClient client)
		{
			_client = client;
		}

		public bool IsConnected
		{
			get
			{
				if (_client != null && _thread != null && _hasDisconnected == false)
					return true;
				else
					return false;
			}
		}

		public bool Connect(string address, int port = 3444)
		{
			try
			{
				TcpClient client = new TcpClient();
				client.Connect(address, port);

				Initialize(client);
				StartWorkerThread();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public void StartWorkerThread()
		{
			if (_thread != null)
			{
				StopWorkerThread();
			}

			_buffer = new byte[BUFFER_SIZE];
			_bufferStream = new MemoryStream(_buffer);
			_bufferReader = new BinaryReader(_bufferStream, Encoding.UTF8);
			_bufferedBytes = 0;

			_sendQueue = new Queue<PacketData>();
			_sendQueueLock = new object();
			_sendQueueHasData = new ManualResetEvent(false);

			_terminateThread.Reset();
			_thread = new Thread(WorkerThreadEntry);
			_thread.Start();
		}

		public void StopWorkerThread()
		{
			if (_thread == null)
				return;

			_terminateThread.Set();
			if (!_thread.Join(3000))
			{
				_thread.Abort();
			}

			_thread = null;
		}

		private void WorkerThreadEntry()
		{

			WaitHandle[] waitHandles = new WaitHandle[3];
			waitHandles[0] = _terminateThread;
			waitHandles[2] = _sendQueueHasData;

			Socket s = _client.Client;

			byte[] sendBuffer = new byte[BUFFER_SIZE + 20];
			MemoryStream sendBufferStream = new MemoryStream(sendBuffer);
			BinaryWriter sendBufferWriter = new BinaryWriter(sendBufferStream);


			var asyncResult = s.BeginReceive(_buffer, (int)_bufferedBytes, BUFFER_SIZE - (int)_bufferedBytes, SocketFlags.None, null, null);
			waitHandles[1] = asyncResult.AsyncWaitHandle;

			while (true)
			{
				int waitResult = WaitHandle.WaitAny(waitHandles);

				if (waitResult == 1)
				{
					//receive completed

					int bytesRecvd = s.EndReceive(asyncResult);

					if (bytesRecvd > 0)
						_bufferedBytes += (uint)bytesRecvd;
					else
					{
						if (!s.Connected)
							break;
						else
							Thread.Sleep(250);//something strange happend
					}

					//attempt to extract a packet if a full packet has been read
					while (ExtractPacket()) { }

					asyncResult = s.BeginReceive(_buffer, (int)_bufferedBytes, BUFFER_SIZE - (int)_bufferedBytes, SocketFlags.None, null, null);
					waitHandles[1] = asyncResult.AsyncWaitHandle;
				}
				else if (waitResult == 2)
				{
					//send queue has data
					_sendQueueHasData.Reset();

					while (true)
					{
						bool packetValid = false;
						PacketData pdata = new PacketData();

						//attempt to retrive data from the queue
						lock (_sendQueueLock)
						{
							if (_sendQueue.Count > 0)
							{
								pdata = _sendQueue.Dequeue();
								packetValid = true;
							}
						}

						if (!packetValid)
							break;

						SendPacket(pdata, s, sendBuffer, sendBufferStream, sendBufferWriter);
						
					}
				}
				else
				{
					//error or thread terminating
					break;
				}
			}

			_hasDisconnected = true;

			RaiseClientDisconnected();

		}

		private bool SendPacket(PacketData pdata, Socket s, byte[] sendBuffer, MemoryStream sendBufferStream, BinaryWriter sendBufferWriter)
		{
			if (_hasDisconnected)
				return false;

			UInt32 packetSize = (UInt32)pdata.Data.Length + sizeof(UInt64);

			sendBufferStream.Seek(0, SeekOrigin.Begin);
			sendBufferWriter.Write(packetSize);
			sendBufferWriter.Write(pdata.PacketID);
			sendBufferWriter.Write(pdata.Data);

			int sendSize = sizeof(UInt32) + (int)packetSize;

			int sendCount = s.Send(sendBuffer, 0, sendSize, SocketFlags.None);
			//System.Diagnostics.Debug.Print("TCPConnectionHandler: Sent {0} bytes on socket {1}", sendCount, s.Handle.ToString());
			if (sendCount != sendSize)
			{
				throw new Exception("Error in SendPacket - Failed to Send");
				//send error on socket
				//return false;
			}

			return true;
		}

		private bool ExtractPacket()
		{
			if (_buffer == null || _bufferedBytes <= 0)
				return false;

			_bufferStream.Seek(0, SeekOrigin.Begin);

			UInt32 packetSize = _bufferReader.ReadUInt32();
			UInt64 packetID = _bufferReader.ReadUInt64();

			if (packetSize <= (_bufferedBytes - sizeof(UInt32)))
			{
				RaisePacketReceived(packetID);
				//we have a complete packet in the buffer, decode it
				DecodePacket(packetID, _bufferReader, packetSize);

				//clear the decoded packet from the buffer
				uint totalPacketSize = (sizeof(UInt32) + packetSize); //bytes to be removed from the buffer
				uint remainingBytes = _bufferedBytes - totalPacketSize;


				if (remainingBytes <= 0)
					_bufferedBytes = 0;
				else
				{
					//System.Diagnostics.Debug.Print("Buffer shifting {0} bytes", remainingBytes);

					//very ineffecient, should use a circular buffer....

					Buffer.BlockCopy(_buffer, (int)totalPacketSize, _buffer, 0, (int)remainingBytes);
					/*for (int i = 0; i < remainingBytes; i++)
					{
						_buffer[i] = _buffer[i + totalPacketSize];
					}*/
					_bufferedBytes = remainingBytes;
				}

				return true;
			}
			else
				return false;
		}

		protected abstract void DecodePacket(UInt64 packetID, BinaryReader reader, UInt32 packetSize);

		private void RaiseClientDisconnected()
		{
			var handler = ClientDisconnected;
			if (handler != null)
				handler(this);
		}

		private void RaisePacketReceived(UInt64 packetID)
		{
			var handler = PacketReceived;
			if (handler != null)
				handler(packetID);
		}

		protected void SendPacket(UInt64 packetID, byte[] data)
		{
			if (!IsConnected)
			{
				throw new Exception("Attempted to send command on disconnected connection");
			}

			lock (_sendQueueLock)
			{
				PacketData pdata = new PacketData();
				pdata.PacketID = packetID;
				pdata.Data = data;
				_sendQueue.Enqueue(pdata);
				_sendQueueHasData.Set();
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					StopWorkerThread();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~TCPConnectionHandler() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion

	}
}

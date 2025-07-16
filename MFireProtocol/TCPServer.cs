using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MFireProtocol
{
	public abstract class TCPServer : IDisposable
	{
		public event Action<TCPConnectionHandler> ClientConnected;

		public TCPConnectionHandler LastConnectedClient;

		private TcpListener _listener;
		private Thread _serverThread;

		private ManualResetEvent _terminateThread;
		private int _port;

		private object _connectionsListLock;
		private List<TCPConnectionHandler> _connections;

		public TCPServer()
		{
			_terminateThread = new ManualResetEvent(false);
			_connections = new List<TCPConnectionHandler>();
			_connectionsListLock = new object();
		}

		public void StartServer(int port = 3444)
		{
			if (_serverThread != null)
			{
				StopServer();
			}

			_port = port;

			_terminateThread.Reset();
			_serverThread = new Thread(ServerThreadEntry);
			_serverThread.Start();
		}

		public void StopServer()
		{
			if (_serverThread == null)
				return;

			_terminateThread.Set();
			if (!_serverThread.Join(3000))
			{
				_serverThread.Abort();
			}

			_serverThread = null;
		}

		protected abstract TCPConnectionHandler CreateConnectionHandler(TcpClient client);

		private void ServerThreadEntry()
		{
			_listener = new TcpListener(IPAddress.Any, _port);
			_listener.Start();

			WaitHandle[] waitHandles = new WaitHandle[2];
			waitHandles[0] = _terminateThread;

			while (true)
			{
				var asyncResult = _listener.BeginAcceptTcpClient(null, null);
				waitHandles[1] = asyncResult.AsyncWaitHandle;

				int waitResult = WaitHandle.WaitAny(waitHandles);
				
				if (waitResult != 1)
				{
					break;
				}

				TcpClient newClient = _listener.EndAcceptTcpClient(asyncResult);

				//create a handler for the new connection
				var handler = CreateConnectionHandler(newClient);
				handler.ClientDisconnected += OnClientDisconnected;

				handler.StartWorkerThread();

				lock (_connectionsListLock)
				{
					_connections.Add(handler);
				}

				LastConnectedClient = handler;

				RaiseClientConnected(handler);
			}

			_listener.Stop();
		}

		private void OnClientDisconnected(TCPConnectionHandler handler)
		{
			handler.ClientDisconnected -= OnClientDisconnected;
			/*lock (_connectionsListLock)
			{
				_connections.Remove(handler);
			} */
		}

		public int GetNumConnections()          
		{
			int numConnections = 0;

			lock (_connectionsListLock)
			{
				if (_connections != null)
					numConnections = _connections.Count;
			}

			return numConnections;
		}

		public TCPConnectionHandler GetConnectedClient(int index)
		{
			TCPConnectionHandler handler = null;

			lock (_connectionsListLock)
			{
				if (index < _connections.Count && index >= 0)
					handler = _connections[index];
			}

			return handler;
		}

		public void RaiseClientConnected(TCPConnectionHandler newClient)
		{
			var handler = ClientConnected;
			if (handler != null)
			{
				handler(newClient);
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
					// dispose managed state (managed objects).
					StopServer();

					
					for (int i = _connections.Count - 1; i >= 0; i--)
					{
						lock (_connectionsListLock)
						{
							if (i <= _connections.Count)
								_connections[i].Dispose();
						}
					}
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~TCPServer() {
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

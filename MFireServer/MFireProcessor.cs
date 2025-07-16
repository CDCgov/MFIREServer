using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFireProtocol;
using MFireDLL;
using System.IO;

namespace MFireServer
{
	class MFireProcessor : IDisposable
	{

		public event Action<string> LogMessage;
		public event Action<string> OutputMessage;
		public event Action SimulationUpdated;

		private MFireTCPServer _tcpServer;
		private MFireEngine _engine;

		private MFireConfig _config;
		private MFireConfig _configB;

		//private MFCUpdateMineState _mineState = new MFCUpdateMineState();

		private Dictionary<int, MFAirway> _airways;
		private Dictionary<int, MFJunction> _junctions;
		private Dictionary<int, MFFan> _fans;
		private Dictionary<int, MFFire> _fires;

        private CommandProcessor _commandProcessor;

		private bool _configBuilt = false;

		private MFCConfigureMFire _lastConfigCommand = null;

		public void Startup()
		{
			if (_tcpServer != null)
			{
				_tcpServer.StopServer();
				_tcpServer = null;
			}

			ClearMFireSimulation();

			_tcpServer = new MFireTCPServer();
			_tcpServer.ClientConnected += OnClientConnected;

			int port = 3444;
			_tcpServer.StartServer(port);			

			RaiseLogMessage("TCP Server Started on port {0}", port);
		}

		public MFireConfig GetCurrentConfig()
		{
			return _config;
		}

		private void OnClientConnected(TCPConnectionHandler obj)
		{
			MFireConnection conn = (MFireConnection)obj;
			conn.MFireCmdReceived += OnMFireCmdReceived;
		}

		private void OnMFireCmdReceived(MFireConnection obj)
		{
			ProcessMFireCmds(obj);
		}

		private void ProcessMFireCmds(MFireConnection conn)
		{
			MFireCmd cmd;

            MFireDLL.EngineState engineState;
			if (_engine != null)
				engineState = _engine.GetEngineState();
			else
				engineState = MFireDLL.EngineState.STOPPED;

			while ((cmd = conn.DequeueReceivedCmd()) != null)
			{
                //execute newly added commands
                _commandProcessor?.Execute(cmd, conn);

                //execute old commands from VR mine
                Type cmdType = cmd.GetType();

				if (cmdType == typeof(MFireCmdTerminateServer))
				{
					Application.Current.Shutdown();
				}
				else if (cmdType == typeof(MFireCmdTest))
				{
					var cmdTest = (MFireCmdTest)cmd;
					RaiseLogMessage("Received Test Packet {0} {1} {2}", cmdTest.Int1,
						cmdTest.Int2, cmdTest.TestString);
                    conn.SendMFireCmd(cmd);
				}
				else if (cmdType == typeof(MFCResetSimulation))
				{
					RaiseLogMessage("MFC: Reset Simulation");
					ResetSimulation();
				}
				else if (cmdType == typeof(MFCUpdateJunction))
				{
					var c = (MFCUpdateJunction)cmd;
					_junctions[c.JunctionIndex] = c.Junction;
				}
				else if (cmdType == typeof(MFCUpdateAirway))
				{
					var c = (MFCUpdateAirway)cmd;
					if (engineState == MFireDLL.EngineState.STOPPED)
					{
						_airways[c.AirwayIndex] = c.Airway;
					}
					else
					{
						_engine.AddAirwayNormalEvent(0, c.Airway.Number, c.Airway.Resistance);
					}
				}
				else if (cmdType == typeof(MFCUpdateFan))
				{
					var c = (MFCUpdateFan)cmd;
					if (engineState == MFireDLL.EngineState.STOPPED)
					{
						_fans[c.FanIndex] = c.Fan;
					}
					else
					{
						_engine.AddFanEvent(0, c.Fan.AirwayNo, c.Fan.AirflowData, c.Fan.PressureData);
						//_config.MeshReformFlag = 1;
						//_configB.MeshReformFlag = 1;
					}
				}
				else if (cmdType == typeof(MFCUpdateFire))
				{
					var c = (MFCUpdateFire)cmd;

					if (engineState == MFireDLL.EngineState.STOPPED)
					{
						_fires[c.FireIndex] = c.Fire;
					}
					else
                    {
						//_engine.AddChangeFireEvent(0, c.Fire.AirwayNo, c.Fire.HeatInput);
						_fires[c.FireIndex] = c.Fire;
						MFHelper.UpdateConfigFire(_config, c.Fire);
						MFHelper.UpdateConfigFire(_configB, c.Fire);
					}
					
				}
				else if (cmdType == typeof(MFCRunSimulation))
				{
					RunSimulation();
				}
				else if (cmdType == typeof(MFCUpdateMineState))
				{
					RaiseLogMessage("MFC: Update Mine State");
					//BuildMFireNetwork((MFCUpdateMineState)cmd);
				}
				else if (cmdType == typeof(MFCConfigureMFire))
				{
					_lastConfigCommand = (MFCConfigureMFire)cmd;
				}
			}
			
		}

        private void RunCommand(MFireCmd cmd)
        {

        }

		public void RunSimulation()
		{
			if (!_configBuilt)
			{
				if (!BuildMFireNetwork())
					return;

				var date = DateTime.Now.ToString("yyyy-dd-M_HH-mm-ss");
				var filename = $"MFIRE-Config-{date}";

				var mydocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				var folder = Path.Combine(mydocs, "MFireConfigs");
				Directory.CreateDirectory(folder);

				filename = Path.Combine(folder, filename);

				//_engine.SaveConfig(filename);

				_engine.SyncBeginSimulation();
			}

			/*
			_engine.AddPauseEvent(_engine.GetTime() + 0.5);

			if (_engine.GetEngineState() == EngineState.PAUSED)
			{
				_engine.AddContinueEvent(0);
			}
			else
			{
				_engine.RunSimulation();
			}
			*/

			for (int i = 0; i < _config.Junctions.Count; i++)
			{
				Junction j = _config.Junctions[i];
				if (j.IsInAtmosphere)
				{
					j.ContamConcentration = 0;
				}
			}

			_engine.SyncRunSimulation(10000);
		}

		public void ResetSimulation()
		{
			ClearMFireSimulation();
			InitializeMFire();
		}

		private void InitializeMFire()
		{
			_engine = new MFireEngine();

			_engine.RegisterLoggerCallback(OnLogReceived);
			_engine.RegisterOutputCallback(OnOutputReceived);
			_engine.RegisterDataReportCallback(OnDataReportReceived);
			_engine.RegisterEngineTickCallback(OnEngineTickReceived);
			_engine.RegisterEngineModelPublishedCallback(OnEngineModelPublished);
			_engine.RegisterEngineStateChangedCallback(OnEngineStateChanged);
			_engine.RegisterEngineMonitorPublishedCallback(OnEngineMonitorPublished);

            _commandProcessor = new CommandProcessor(_engine);

			RaiseLogMessage("MFire Initialized");
		}

		private bool BuildMFireNetwork()
		{
			_config = new MFireConfig();
			_configB = new MFireConfig();

			if (_lastConfigCommand != null)
			{
				MFHelper.SetConfigParameters(_config, _lastConfigCommand);
				MFHelper.SetConfigParameters(_configB, _lastConfigCommand);
			}
			else
			{
				SetConfigParameters(_config);
				SetConfigParameters(_configB);
			}

			MFHelper.AddConfigAirways(_config, _airways);
			MFHelper.AddConfigAirways(_configB, _airways);

			MFHelper.AddConfigJunctions(_config, _junctions);
			MFHelper.AddConfigJunctions(_configB, _junctions);

			MFHelper.AddConfigFans(_config, _fans);
			MFHelper.AddConfigFans(_configB, _fans);

			MFHelper.AddConfigFires(_config, _fires);
			MFHelper.AddConfigFires(_configB, _fires);

			RaiseLogMessage("Loading MFire Config");

			_config.SetLegacyLoadSuccess(true);
			_configB.SetLegacyLoadSuccess(true);

			_config.ApplyEngineeringUnits();
			_config.CombineEvents();
			_config.PerformLegacyInit();
			_config.Validate();

			_configB.ApplyEngineeringUnits();
			_configB.CombineEvents();
			_configB.PerformLegacyInit();
			_configB.Validate();

			if (!_engine.LoadConfig(_config, _configB))
				return false;

			//_engine.RunSimulation();

			_configBuilt = true;
			return true;

		}

		private void SetConfigParameters(MFireConfig config)
		{
			config.StartJunction = 1;
			config.StartJunctionTemperature = 50;
			config.TimeEquillibrium = 0.1f;
			config.FumeCriteria = 0.5f;
			config.CH4Criteria = 0.5f;
			config.TemperatureCriteria = 0.5f;
			config.PressureDropWarningLimit = 0.25f;
			config.FumeWarningLimit = 0.5f;
			config.CH4WarningLimit = 9.0f;
			config.TemperatureLimit = 900;
			config.AvgValueDataPresent = 0;
			config.MaxDynamicIterations = 10;
			config.MaxTemperatureIterations = 10;
			config.TimeSpan = 25;
		}

		private void ClearMFireSimulation()
		{
			if (_engine != null)
			{
				_engine.EndSimulation();
				_engine.UnregisterLoggerCallback(OnLogReceived);
				_engine.UnregisterOutputCallback(OnOutputReceived);
				_engine.UnregisterDataReportCallback(OnDataReportReceived);
				_engine.UnregisterEngineTickCallback(OnEngineTickReceived);
				_engine.UnregisterEngineModelPublishedCallback(OnEngineModelPublished);
				_engine.UnregisterEngineStateChangedCallback(OnEngineStateChanged);
				_engine.UnregisterEngineMonitorPublishedCallback(OnEngineMonitorPublished);

				_engine.Dispose();
				_engine = null;
			}

			_configBuilt = false;
			_airways = new Dictionary<int, MFAirway>();
			_junctions = new Dictionary<int, MFJunction>();
			_fans = new Dictionary<int, MFFan>();
			_fires = new Dictionary<int, MFFire>();
		}

		public void BuildMFireConfig()
		{
			MFireConfig config = new MFireConfig();

			
		}

		private void OnLogReceived(LogSeverityLevel severity, string message)
		{
			RaiseLogMessage("LogMessage: " + message);
		}

		private void OnOutputReceived(string message)
		{
			RaiseOutputMessage("Output: " + message);
		}

		private void OnDataReportReceived(string message)
		{
			RaiseOutputMessage("DataReport: " + message);
		}

		private void OnEngineTickReceived(double currentTime)
		{

		}
		
		private void OnEngineModelPublished(Dictionary<string, object> model)
		{

		}

		private void OnEngineStateChanged(MFireDLL.EngineState state)
		{

		}

		private void OnEngineMonitorPublished(Dictionary<string, object> model)
		{
			//send current sim state back to the last client

			try
			{
				MFireConnection conn = (MFireConnection)_tcpServer.LastConnectedClient;

				if (conn != null && conn.IsConnected)
				{
					//copy data directly from the config class
					//MFHelper.UpdateMineStateData(_mineState, _config);
					//conn.SendMFireCmd(_mineState);

					//send data from config class
					MFAirway mfa = new MFAirway();
					MFJunction mfj = new MFJunction();

					foreach (Airway airway in _config.Airways)
					{
						MFHelper.CopyAirwayData(airway, mfa);
						conn.SendUpdateAirway(mfa.Number, mfa);
					}

					foreach (Junction junction in _config.Junctions)
					{
						MFHelper.CopyJunctionData(junction, mfj);
						conn.SendUpdateJunction(mfj.Number, mfj);
					}


					double elapsedTime = _engine.GetTime() * 60.0 * 1000.0;

					conn.SendSimulationUpdated(elapsedTime);
					RaiseSimulationUpdated();
				}
			}
			catch (Exception)
			{

			}
		}

		public void Shutdown()
		{
			ClearMFireSimulation();

			if (_tcpServer != null)
			{
				_tcpServer.StopServer();
				_tcpServer.Dispose();
				_tcpServer = null;
			}

			RaiseLogMessage("TCP Server Shutdown");
		}

		private void RaiseLogMessage(string fmt, params object[] args)
		{
			string msg = string.Format(fmt, args);
			var handler = LogMessage;
			if (handler != null)
				handler(msg);
		}

		private void RaiseOutputMessage(string fmt, params object[] args)
		{
			string msg = string.Format(fmt, args);
			var handler = OutputMessage;
			if (handler != null)
				handler(msg);
		}

		private void RaiseSimulationUpdated()
		{
			var handler = SimulationUpdated;
			if (handler != null)
				handler();
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
					Shutdown();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~MFireProcessor() {
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

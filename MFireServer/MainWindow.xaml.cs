using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MFireProtocol;
using MFireDLL;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Threading;

namespace MFireServer
{
	public class MFIREStatusItem : INotifyPropertyChanged
	{
		public string StatusText { get; set; }
		public ImageSource Image { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

		private PropertyChangedEventArgs _eventArgs = new PropertyChangedEventArgs(null);
		public void Update()
		{
			PropertyChanged?.Invoke(this, _eventArgs);
		}
    }

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public const int MAX_LOG_DISPLAY = 100;
		LinkedList<string> _logEntries;
		LinkedList<string> _debugEntries;
		LinkedList<string> _errorEntries;

		LinkedList<string> _outputEntries;
		//LinkedList<string> _reportEntries;
		StringBuilder _txtLogSB;
		StringBuilder _txtDebugSB;
		StringBuilder _txtErrorSB;

		StringBuilder _txtOutputSB;
		StringBuilder _simStatus;

		//MFireServer _server;
		//MFireTCPServer _server;
		MFireProcessor _processor;

		private DispatcherTimer _timer;
		private DateTime _lastUpdate = DateTime.MinValue;
		                                                                                                            
		public MainWindow()
		{
			_logEntries = new LinkedList<string>();
			_debugEntries = new LinkedList<string>();
			_errorEntries = new LinkedList<string>();
			_outputEntries = new LinkedList<string>();
			//_reportEntries = new LinkedList<string>();
			_txtLogSB = new StringBuilder();
			_txtDebugSB = new StringBuilder();
			_txtErrorSB = new StringBuilder();
			_txtOutputSB = new StringBuilder();
			_simStatus = new StringBuilder();

			InitializeComponent();
            //Hide();
			

			_processor = new MFireProcessor();
			_processor.LogMessageWithLevel += OnLogMessage;
			_processor.OutputMessage += OnOutputMessage;
			_processor.DataReportReceived += OnDataReportReceived;
			_processor.SimulationUpdated += OnSimulationUpdated;
			_processor.SimulationReset += OnSimulationReset;
			_processor.Startup();

			this.DataContext = _processor;

			lstConnectedClients.DataContext = _processor.ConnectedClients;

			_timer = new DispatcherTimer();
			_timer.Tick += OnTimer;
			_timer.Interval = TimeSpan.FromSeconds(1);
			_timer.Start();
		}

		private Action _updateSimStatusDel;

		private void OnSimulationUpdated(double elapsedMs)
		{
			lock (_simStatus)
			{
				_simStatus.Clear();

				var config = _processor.GetCurrentConfig();

				int numAirways = 0;
				int numJunctions = 0;
				int numFans = 0;
				int numFires = 0;

				_simStatus.AppendFormat("Start Junction: {0}\n", config.StartJunction);

				
				if (config.Fans != null)
				{
					foreach (var fan in config.Fans)
					{
						_simStatus.AppendFormat("Fan in Airway {0}\n", fan.AirwayNo);
						numFans++;
					}
				}

				if (config.Fires != null)
				{
					//_simStatus.AppendFormat("Num Fires : {0}\n", config.Fires.Count);
					foreach (var fire in config.Fires)
					{
						_simStatus.AppendFormat("Fire in Airway {0}\n", fire.AirwayNo);
						numFires++;
					}
				}

				if (config.Airways != null)
				{
					_simStatus.AppendFormat("Num Airways : {0}\n", config.Airways.Count);
					numAirways = config.Airways.Count;
				}

				if (config.Junctions != null)
				{
					_simStatus.AppendFormat("Num Junctions : {0}\n", config.Junctions.Count);
					numJunctions = config.Junctions.Count;
					
					foreach (Junction j in config.Junctions)
					{
						_simStatus.AppendFormat("J{0}\tContamConc: {1:F5}\tTotalContam: {2:F5}\tCH4Conc: {3:F5}\n",
							j.Number, j.ContamConcentration, j.TotalContaminant, j.CH4Concentration);
					}
				}

				//TimeSpan ts = new TimeSpan(0, 0, (int)Math.Round((elapsedMs / 1000.0)));
				//statusSimTime.StatusText = string.Format("Sim Time: {0}", ts.ToString());
				//statusSimTime.Update();

				statusNumAirways.StatusText = string.Format("Num Airways: {0}", numAirways);
				statusNumAirways.Update();

				statusNumJunctions.StatusText = string.Format("Num Junctions: {0}", numJunctions);
				statusNumJunctions.Update();

				statusNumFires.StatusText = string.Format("Num Fires: {0}", numFires);
				statusNumFires.Update();

				statusNumFans.StatusText = string.Format("Num Fans: {0}", numFans);
				statusNumFans.Update();

				statusComputeTime.StatusText = string.Format("Compute Time: {0}ms", _processor.LastComputeTime);
				statusComputeTime.Update();
			}
						

			if (_updateSimStatusDel == null)
				_updateSimStatusDel = UpdateSimStatusText;

			App.Current.Dispatcher.BeginInvoke(_updateSimStatusDel);
		}

		private void UpdateErrorDisplay()
		{
			try
			{
				if (_errorEntries.Count > 0)
				{
					taskBarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Error;
					taskBarItemInfo.Overlay = this.Resources["ErrorImage"] as ImageSource;

                    WindowFlasher.Flash(this, 0);
                }
				else
				{
					taskBarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
					TaskbarItemInfo.Overlay = null;

					WindowFlasher.StopFlashing(this);
				}
			}
			catch (Exception ex)
			{
				AppendLog($"Error updating error display: {ex.Message}", LogSeverityLevel.ERROR);
			}
        }

		private void OnSimulationReset()
		{
            if (App.Current == null || App.Current.Dispatcher == null)
                return;

			//invoke on main thread
			if (App.Current.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
			{
				App.Current.Dispatcher.BeginInvoke((Action)OnSimulationReset);
				return;
			}

			ClearErrors();
		}

		private void ClearErrors()
		{
			if (txtErrors != null)
				txtErrors.Text = "";

			if (_errorEntries != null)
				_errorEntries.Clear();

			if (_txtErrorSB != null)
				_txtErrorSB.Clear();

			UpdateErrorDisplay();
		}

		private void UpdateSimStatusText()
		{
			string status;
			lock (_simStatus)
			{
				status = _simStatus.ToString();
			}
			txtSimStatus.Text = status;


			_lastUpdate = DateTime.Now;

            UpdateErrorDisplay();
        }

		private void OnLogMessage(string msg, LogSeverityLevel severity)
		{
			AppendLog(msg, severity);
		}

		private void OnDataReportReceived(string dataReport)
		{
			if (dataReport == null || dataReport.Length <= 0)
				return;

            //invoke on main thread
            if (App.Current.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                App.Current.Dispatcher.BeginInvoke((Action<string>)OnDataReportReceived, dataReport);
                return;
            }

			txtDataReport.Text = dataReport;
        }

		private void OnOutputMessage(string msg)
		{
			AppendOutputLog(msg);
		}

		//public void AppendLog(string fmt, params object[] args)
		//{
		//	string line = string.Format(fmt, args);

		//	AppendLog(line);
		//}

		public void AppendOutputLog(string fmt, params object[] args)
		{
			string line = string.Format(fmt, args);

			AppendOutputLog(line);
		}

		public void AppendOutputLog(string message)
		{
			AppendText(message, _outputEntries, _txtOutputSB, txtOutput);
		}

		private Action<string, LogSeverityLevel> _appendLogDel;

		public void AppendLog(string msg, LogSeverityLevel severity)
		{
			if (App.Current == null || App.Current.Dispatcher == null)
				return;

			//invoke on main thread
			if (App.Current.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
			{
				if (_appendLogDel == null)
					_appendLogDel = AppendLog;

				App.Current.Dispatcher.BeginInvoke(_appendLogDel, msg, severity);
				return;
			}

			var log = String.Format("{0,-8}: {1}", severity.ToString(), msg);

			if (severity != LogSeverityLevel.DEBUG)
			{
				AppendLog(log, _logEntries, _txtLogSB, txtLog);
			}
			else
			{
                AppendLog(log, _debugEntries, _txtDebugSB, txtDebugLog);
            }

			if (severity == LogSeverityLevel.ERROR)
			{
				AppendLog(log, _errorEntries, _txtErrorSB, txtErrors);

				UpdateErrorDisplay();
			}

			//_logEntries.AddFirst(line);
			//while (_logEntries.Count > MAX_LOG_DISPLAY)
			//	_logEntries.RemoveLast();

			//_txtLogSB.Clear();
			//foreach (string log in _logEntries)
			//{
			//	_txtLogSB.AppendLine(log);
			//}

			//txtLog.Text = _txtLogSB.ToString();
		}

		private void AppendLog(string line, LinkedList<string> list, StringBuilder sb, TextBlock txtBlock)
		{
			if (list == null || sb == null || txtBlock == null)
				return;

            list.AddFirst(line);
            while (list.Count > MAX_LOG_DISPLAY)
                list.RemoveLast();

            sb.Clear();
            foreach (string log in list)
            {
                sb.AppendLine(log);
            }

            txtBlock.Text = sb.ToString();
        }

		private delegate void AppendTextDel(string line, LinkedList<string> entries, StringBuilder sb, TextBlock txt);
		private AppendTextDel _appendTextDel;

		public void AppendText(string line, LinkedList<string> entries, StringBuilder sb, TextBlock txt)
		{
			if (App.Current == null || App.Current.Dispatcher == null)
				return;

			//invoke on main thread
			if (App.Current.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
			{
				if (_appendTextDel == null)
					_appendTextDel = AppendText;

				App.Current.Dispatcher.BeginInvoke(_appendTextDel, line, entries, sb, txt);
				return;
			}

			entries.AddFirst(line);
			while (entries.Count > MAX_LOG_DISPLAY)
				entries.RemoveLast();

			sb.Clear();
			foreach (string log in entries)
			{
				sb.AppendLine(log);
			}

			txt.Text = sb.ToString();
		}

		protected override void OnClosed(EventArgs e)
		{
			Cleanup();

			base.OnClosed(e);
		}

		private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Cleanup();
		}

		private void Cleanup()
		{
            if (_processor != null)
            {
                _processor.Shutdown();
                _processor = null;
            }

			if (_timer != null)
			{
				_timer.Stop();
				_timer.Tick -= OnTimer;
				_timer = null;
			}
        }

		private void OnTimer(object sender, EventArgs e)
		{
			var elapsed = DateTime.Now - _lastUpdate;
			var totalSeconds = elapsed.TotalSeconds;
			
			if (_lastUpdate == DateTime.MinValue)
			{
                statusLastUpdate.StatusText = "Last Update: Never";
            }
			else if (totalSeconds > 600)
			{
                statusLastUpdate.StatusText = "Last Update: >10m ago";
            }
			else
			{
				statusLastUpdate.StatusText = string.Format("Last Update: {0}s ago", (int)totalSeconds);
			}

			if (totalSeconds > 3)
			{
				txtSimTime.Foreground = Brushes.Red;
			}
			else
			{
				txtSimTime.Foreground = Brushes.Green;
			}

			statusLastUpdate.Update();
		}

		private void OnClickedExit(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

        protected override void OnStateChanged(EventArgs e)
        {
            //if (WindowState == System.Windows.WindowState.Minimized)
            //{
            //    WindowState = WindowState.Normal;
            //    Hide();
            //}
            base.OnStateChanged(e);
        }

        private void OnTrayDoubleClick(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }

        private void OnMenuShowWindow(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }

        private void ShowWindow()
        {
            WindowState = WindowState.Normal;
            Show();
        }

        private void OnMenuExit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

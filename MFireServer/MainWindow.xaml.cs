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

namespace MFireServer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public const int MAX_LOG_DISPLAY = 100;
		LinkedList<string> _logEntries;
		LinkedList<string> _outputEntries;
		StringBuilder _txtLogSB;
		StringBuilder _txtOutputSB;
		StringBuilder _simStatus;

		//MFireServer _server;
		//MFireTCPServer _server;
		MFireProcessor _processor;
		                                                                                                            
		public MainWindow()
		{
			_logEntries = new LinkedList<string>();
			_outputEntries = new LinkedList<string>();
			_txtLogSB = new StringBuilder();
			_txtOutputSB = new StringBuilder();
			_simStatus = new StringBuilder();

			InitializeComponent();
            Hide();

			_processor = new MFireProcessor();
			_processor.LogMessage += OnLogMessage;
			_processor.OutputMessage += OnOutputMessage;
			_processor.SimulationUpdated += OnSimulationUpdated;
			_processor.Startup();

		}

		private Action _updateSimStatusDel;

		private void OnSimulationUpdated()
		{
			lock (_simStatus)
			{
				_simStatus.Clear();

				var config = _processor.GetCurrentConfig();

				_simStatus.AppendFormat("Start Junction: {0}\n", config.StartJunction);
				
				if (config.Fans != null)
				{
					foreach (var fan in config.Fans)
					{
						_simStatus.AppendFormat("Fan in Airway {0}\n", fan.AirwayNo);
					}
				}

				if (config.Fires != null)
				{
					//_simStatus.AppendFormat("Num Fires : {0}\n", config.Fires.Count);
					foreach (var fire in config.Fires)
					{
						_simStatus.AppendFormat("Fire in Airway {0}\n", fire.AirwayNo);
					}
				}
				if (config.Airways != null)
					_simStatus.AppendFormat("Num Airways : {0}\n", config.Airways.Count);
				if (config.Junctions != null)
				{
					_simStatus.AppendFormat("Num Junctions : {0}\n", config.Junctions.Count);

					foreach (Junction j in config.Junctions)
					{
						_simStatus.AppendFormat("J{0}\tContamConc: {1:F5}\tTotalContam: {2:F5}\tCH4Conc: {3:F5}\n",
							j.Number, j.ContamConcentration, j.TotalContaminant, j.CH4Concentration);
					}
				}
			}

			if (_updateSimStatusDel == null)
				_updateSimStatusDel = UpdateSimStatusText;

			App.Current.Dispatcher.BeginInvoke(_updateSimStatusDel);
		}

		private void UpdateSimStatusText()
		{
			string status;
			lock (_simStatus)
			{
				status = _simStatus.ToString();
			}
			txtSimStatus.Text = status;
		}

		private void OnLogMessage(string msg)
		{
			AppendLog(msg);
		}

		private void OnOutputMessage(string msg)
		{
			AppendOutputLog(msg);
		}

		public void AppendLog(string fmt, params object[] args)
		{
			string line = string.Format(fmt, args);

			AppendLog(line);
		}

		public void AppendOutputLog(string fmt, params object[] args)
		{
			string line = string.Format(fmt, args);

			AppendOutputLog(line);
		}

		public void AppendOutputLog(string message)
		{
			AppendText(message, _outputEntries, _txtOutputSB, txtOutput);
		}

		private Action<string> _appendLogDel;

		public void AppendLog(string line)
		{
			if (App.Current == null || App.Current.Dispatcher == null)
				return;

			//invoke on main thread
			if (App.Current.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
			{
				if (_appendLogDel == null)
					_appendLogDel = AppendLog;

				App.Current.Dispatcher.BeginInvoke(_appendLogDel, line);
				return;
			}

			_logEntries.AddFirst(line);
			while (_logEntries.Count > MAX_LOG_DISPLAY)
				_logEntries.RemoveLast();

			_txtLogSB.Clear();
			foreach (string log in _logEntries)
			{
				_txtLogSB.AppendLine(log);
			}

			txtLog.Text = _txtLogSB.ToString();
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
			if (_processor != null)
			{
				_processor.Shutdown();
				_processor = null;
			}

			base.OnClosed(e);
		}

		private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (_processor != null)
			{
				_processor.Shutdown();
				_processor = null;
			}			
		}

		private void OnClickedExit(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
                Hide();
            }
            base.OnStateChanged(e);
        }

        private void myNotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }

        private void ShowWindow()
        {
            WindowState = WindowState.Normal;
            Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

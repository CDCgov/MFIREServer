using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MFireDLL;
using MFireProtocol;

namespace MFireServer
{
    public class CommandProcessor
    {
        //TODO: Commands that use MFireConfig, GetLogger?, GetOutput?

        private MFireEngine _engine;
        private Dictionary<Type, Action<MFireCmd, MFireConnection>> _commandProcessors;

        public CommandProcessor(MFireEngine engine)
        {
            _engine = engine;
            _commandProcessors = new Dictionary<Type, Action<MFireCmd, MFireConnection>>();
            _commandProcessors.Add(typeof(MFCAddAirwayNormalEvent), addAirwayNormalEvent);
            _commandProcessors.Add(typeof(MFCConfigChanged), configChanged);
            _commandProcessors.Add(typeof(MFCAddChangeFireEvent), addChangeFireEvent);
            _commandProcessors.Add(typeof(MFCAddChangeOutputTimeIntervalEvent), addChangeOutputTimeIntervalEvent);
            _commandProcessors.Add(typeof(MFCLoadConfigFile), loadConfigFile);
            _commandProcessors.Add(typeof(MFCAddChangeParameterEvent), addChangeParameterEvent);
            _commandProcessors.Add(typeof(MFCAddChangeTimeIncrementEvent), addChangeTimeIncrementEvent);
            _commandProcessors.Add(typeof(MFCAddClearFireEvent), addClearFireEvent);
            _commandProcessors.Add(typeof(MFCAddContinueEvent), addContinueEvent);
            _commandProcessors.Add(typeof(MFCAddEvent), addEvent);
            _commandProcessors.Add(typeof(MFCAddFanEvent), addFanEvent);
            _commandProcessors.Add(typeof(MFCAddFireSourceEvent), addFireSourceEvent);
            _commandProcessors.Add(typeof(MFCAddModelQueryEvent), addModelQueryEvent);
            _commandProcessors.Add(typeof(MFCAddPauseEvent), addPauseEvent);
            _commandProcessors.Add(typeof(MFCAddShowDetailedJunctionOutputEvent), addShowDetailedJunctionOutputEvent);
            _commandProcessors.Add(typeof(MFCAddSimulationResumeEvent), addSimulationResumeEvent);
            _commandProcessors.Add(typeof(MFCClearEvents), clearEvents);
            _commandProcessors.Add(typeof(MFCDispose), dispose);
            _commandProcessors.Add(typeof(MFCEndSimulation), endSimulation);
            _commandProcessors.Add(typeof(MFCGetAirwayNumbers), getAirwayNumbers);
            _commandProcessors.Add(typeof(MFCGetEngineState), getEngineState);
            _commandProcessors.Add(typeof(MFCGetEventQueue), getEventQueue);
            _commandProcessors.Add(typeof(MFCGetFanAirFlowData), getFanAirflowData);
            _commandProcessors.Add(typeof(MFCGetFanPressureData), getFanPressureData);
            _commandProcessors.Add(typeof(MFCGetInputUnits), getInputUnits);
            _commandProcessors.Add(typeof(MFCGetJunctionNumbers), getJunctionNumbers);
            _commandProcessors.Add(typeof(MFCGetOutputUnits), getOutputUnits);
            _commandProcessors.Add(typeof(MFCGetResumeTime), getResumeTime);
            _commandProcessors.Add(typeof(MFCGetSimulationSnapshot), getSimulationSnapshot);
            _commandProcessors.Add(typeof(MFCGetTime), getTime);
            _commandProcessors.Add(typeof(MFCIsContinuousMode), isContinuousMode);
            _commandProcessors.Add(typeof(MFCIsResuming), isResuming);
            _commandProcessors.Add(typeof(MFCSaveConfig), saveConfig);
            _commandProcessors.Add(typeof(MFCSetContinuousMode), setContinuousMode);
            _commandProcessors.Add(typeof(MFCSetDelay), setDelay);
            _commandProcessors.Add(typeof(MFCSetInputUnits), setInputUnits);
            _commandProcessors.Add(typeof(MFCSetOutputUnits), setOutputUnits);
            _commandProcessors.Add(typeof(MFCSyncBeginSimulation), syncBeginSimulation);
            _commandProcessors.Add(typeof(MFCSyncRunSimulation), syncRunSimulation);
        }

        private void syncRunSimulation(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCSyncRunSimulation)arg1;
            _engine.SyncRunSimulation(cmd.Timestep);
        }

        private void syncBeginSimulation(MFireCmd arg1, MFireConnection arg2)
        {
            _engine.SyncBeginSimulation();
        }

        private void setOutputUnits(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCSetOutputUnits)arg1;
            _engine.SetOutputUnits((MFireDLL.EngineeringUnits)cmd.EngineeringUnits);
        }

        private void setInputUnits(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCSetInputUnits)arg1;
            _engine.SetInputUnits((MFireDLL.EngineeringUnits)cmd.EngineeringUnits);
        }

        private void setDelay(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCSetDelay)arg1;
            _engine.SetDelay(cmd.Seconds);
        }

        private void setContinuousMode(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCSetContinuousMode)arg1;
            _engine.SetContinuousMode(cmd.Continuous);
        }

        private void saveConfig(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCSaveConfig)arg1;
            var r = _engine.SaveConfig(cmd.FileName);
            con.SendMFireCmd(new MFRSaveConfig() { Success = r });
        }

        private void isResuming(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCIsResuming)arg1;
            var r = _engine.IsResuming();
            con.SendMFireCmd(new MFRIsResuming() { IsResuming = r });
        }

        private void isContinuousMode(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCIsContinuousMode)arg1;
            var r = _engine.IsContinuousMode();
            con.SendMFireCmd(new MFRIsContinuousMode() { IsContinuous = r });
        }

        private void getTime(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCGetTime)arg1;
            var r = _engine.GetTime();
            con.SendMFireCmd(new MFRGetTime() { Time = r });
        }

        private void getSimulationSnapshot(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCGetSimulationSnapshot)arg1;
            var r = _engine.GetSimulationSnapshot();
            con.SendMFireCmd(new MFRGetSimulationSnapshot() { Snapshot = r });
        }

        private void getResumeTime(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCGetResumeTime)arg1;
            var r = _engine.GetResumeTime();
            con.SendMFireCmd(new MFRGetResumeTime() { ResumeTime = r });
        }

        private void getOutputUnits(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCGetOutputUnits)arg1;
            var r = _engine.GetOutputUnits();
            con.SendMFireCmd(new MFRGetOutputUnits() { EngineeringUnits = (MFireProtocol.EngineeringUnits)r });
        }

        private void getJunctionNumbers(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCGetJunctionNumbers)arg1;
            var r = _engine.GetJunctionNumbers();
            con.SendMFireCmd(new MFRGetJunctionNumbers() { JunctionNumbers = r });
        }

        private void getInputUnits(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCGetInputUnits)arg1;
            var r = _engine.GetInputUnits();
            con.SendMFireCmd(new MFRGetInputUnits() { EngineeringUnits = (MFireProtocol.EngineeringUnits)r });
        }

        private void getFanPressureData(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCGetFanPressureData)arg1;
            var r = _engine.GetFanPressureData(cmd.A_0);
            con.SendMFireCmd(new MFRGetFanPressureData() { PressureData = r });
        }

        private void getFanAirflowData(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCGetFanAirFlowData)arg1;
            var r = _engine.GetFanAirFlowData(cmd.A_0);
            con.SendMFireCmd(new MFRGetFanAirFlowData() { AirflowData = r });
        }

        private void getEventQueue(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCGetEventQueue)arg1;
            var r = _engine.GetEventQueue();
            var se = new List<SimulationEvent>();
            foreach(var e in r)
            {
                se.Add(new SimulationEvent()
                {
                    EventType = e.EventType,
                    Periodic = e.Periodic,
                    RecurringInterval = e.RecurringInterval,
                    TS = e.TS
                });
            }
            con.SendMFireCmd(new MFRGetEventQueue() { SimulationEvents = se });
        }

        private void getEngineState(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCGetEngineState)arg1;
            var r = _engine.GetEngineState();
            con.SendMFireCmd(new MFRGetEngineState() { EngineState = (MFireProtocol.EngineState)r });
        }

        private void getAirwayNumbers(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCGetAirwayNumbers)arg1;
            var r = _engine.GetAirwayNumbers();
            con.SendMFireCmd(new MFRGetAirwayNumbers() { AirwayNumbers = r });
        }

        private void endSimulation(MFireCmd arg1, MFireConnection arg2)
        {
            _engine.EndSimulation();
        }

        private void dispose(MFireCmd arg1, MFireConnection arg2)
        {
            _engine.Dispose();
        }

        private void clearEvents(MFireCmd arg1, MFireConnection arg2)
        {
            _engine.ClearEvents();
        }

        private void addSimulationResumeEvent(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCAddSimulationResumeEvent)arg1;
            _engine.AddSimulationResumeEvent(cmd.TimeStamp);
        }

        private void addShowDetailedJunctionOutputEvent(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCAddShowDetailedJunctionOutputEvent)arg1;
            _engine.AddShowDetailedJunctionOutputEvent(cmd.TimeStamp, cmd.JunctionNumber);
        }

        private void addPauseEvent(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCAddPauseEvent)arg1;
            _engine.AddPauseEvent(cmd.TimeStamp);
        }

        private void addModelQueryEvent(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCAddModelQueryEvent)arg1;
            _engine.AddModelQueryEvent(cmd.TimeStamp, cmd.Interval);
        }

        private void addFireSourceEvent(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCAddFireSourceEvent)arg1;
            _engine.AddFireSourceEvent(cmd.TimeStamp, cmd.AirwayNumber, cmd.ContamFlowRate, cmd.ContamConcentration, cmd.HeatInput, cmd.O2ConcLeavingFire, cmd.ContamPerCuFtO2, cmd.HeatPerCuFtO2, cmd.StandardAirFlow, cmd.TransitionTime); 
        }

        private void addFanEvent(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCAddFanEvent)arg1;
            _engine.AddFanEvent(cmd.TimeStamp, cmd.AirwayNumber, cmd.AirflowData, cmd.PressureData);
        }

        private void addEvent(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCAddEvent)arg1;
            using (var e = new MFireDLL.Events.SimulationEvent()) //why does this implement IDisposable? Is it OK to dispose after adding it?
            {
                e.EventType = cmd.SimulationEvent.EventType;
                e.Periodic = cmd.SimulationEvent.Periodic;
                e.RecurringInterval = cmd.SimulationEvent.RecurringInterval;
                e.TS = cmd.SimulationEvent.TS;
                _engine.AddEvent(e);
            }
        }

        private void addContinueEvent(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCAddContinueEvent)arg1;
            _engine.AddContinueEvent(cmd.TimeStamp);
        }

        private void addClearFireEvent(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCAddClearFireEvent)arg1;
            _engine.AddClearFireEvent(cmd.TimeStamp, cmd.AirwayNumber);
        }

        private void addChangeTimeIncrementEvent(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCAddChangeTimeIncrementEvent)arg1;
            _engine.AddChangeTimeIncrementEvent(cmd.TimeStamp, cmd.TimeIncrement);
        }

        private void addChangeParameterEvent(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCAddChangeParameterEvent)arg1;
            _engine.AddChangeParameterEvent(cmd.TimeStamp, (MFireDLL.Events.SimulationParameter)cmd.SimulationParameter, cmd.Value);
        }

        private void loadConfigFile(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCLoadConfigFile)arg1;
            _engine.LoadConfig(cmd.FileName);
        }

        private void addChangeOutputTimeIntervalEvent(MFireCmd arg1, MFireConnection arg2)
        {
            var cmd = (MFCAddChangeOutputTimeIntervalEvent)arg1;
            _engine.AddChangeOutputTimeIntervalEvent(cmd.TimeStamp, cmd.OutputTimeInterval);
        }

        private void addChangeFireEvent(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCAddChangeFireEvent)arg1;
            _engine.AddChangeFireEvent(cmd.TimeStamp, cmd.AirwayNumber, cmd.HeatInputBtu);
        }

        private void configChanged(MFireCmd arg1, MFireConnection con)
        {
            var r = _engine.ConfigChanged();
            con.SendMFireCmd(new MFRConfigChanged() { ConfigChanged = r });
        }

        private void addAirwayNormalEvent(MFireCmd arg1, MFireConnection con)
        {
            var cmd = (MFCAddAirwayNormalEvent)arg1;
            _engine.AddAirwayNormalEvent(cmd.TimeStamp, cmd.AirwayNumber, cmd.StandardResistance);
        }

        public void Execute<CommandType>(CommandType cmd, MFireConnection con) where CommandType : MFireCmd
        {
            var cmdType = cmd.GetType();
            if (_commandProcessors.ContainsKey(cmdType))
                _commandProcessors[cmdType].Invoke(cmd, con);
        }
    }
}

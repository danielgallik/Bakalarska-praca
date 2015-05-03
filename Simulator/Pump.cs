using System;
using System.Collections.Generic;
using System.Timers;
using System.Configuration;

namespace Simulator
{
    class Pump
    {
        public string SerialNumber { get; private set; }
        public string Medicament { get; private set; }
        public string IpAddress { get; private set; }
        public string AlertMessage { get; private set; }
        public string CurrentState { get { return MyCurrentState.ToString(); } }
        public string TypePump { get { return MyType.ToString(); } }
        public int TotalTime { get { return Convert.ToInt32(MyTotalTime); } }
        public int RemainingTime { get { return Convert.ToInt32(MyRemainingTime); } }

        private double MyRate { get; set; }                   // ml/h

        private double MyTotalVolume { get; set; }           // ml
        private double MyTotalTime { get; set; }             // s

        private double MyInfusedVolume { get; set; }         // ml
        private double MyElapsedTime { get; set; }           // s

        private double MyRemainingVolume { get; set; }       // ml
        private double MyRemainingTime { get; set; }         // s

        private object MyPumpLock;
        private Timer MyTimer;
        private Dictionary<Transition, State> MyTransitions;
        private List<MyAlarm> MyAlarms;
        private State MyCurrentState { get; set; }
        private Type MyType { get; set;}

        private double MyPrealarmStartTime = Convert.ToDouble(ConfigurationManager.AppSettings["PreAlarm"]);

        public Pump(string serialNumber, string type = "")
        {
            if (type.ToLower() == Type.Injection.ToString().ToLower())
            {
                MyType = Type.Injection;
            }
            else
            {
                MyType = Type.Infusion;
            }
            MyPumpLock = new object();
            SerialNumber = serialNumber;
            AlertMessage = "";
            MyTransitions = new Dictionary<Transition, State>
            {
                { new Transition(State.Off, Command.TurnOn), State.Stop },
                { new Transition(State.Stop, Command.TurnOff), State.Off },
                { new Transition(State.Stop, Command.Start), State.Running },
                { new Transition(State.Running, Command.Stop), State.Stop },
                { new Transition(State.Running, Command.Warning), State.PreAlarm },
                { new Transition(State.Running, Command.Error), State.Alarm },
                { new Transition(State.PreAlarm, Command.Stop), State.Stop },
                { new Transition(State.PreAlarm, Command.Error), State.Alarm },
                { new Transition(State.Alarm, Command.Acknowledge), State.Stop }
            };
            MyAlarms = new List<MyAlarm>
            {
                {new MyAlarm(){Used = false, ProgressPosition = 0.2, Message = "The door is open"}},
                {new MyAlarm(){Used = false, ProgressPosition = 0.8, Message = "Pressure too high"}}
            };
            MyCurrentState = State.Off;
            MyRate = 0;
            MyTotalTime = 0;
            MyTotalVolume = 0;
            MyElapsedTime = 0;
            MyInfusedVolume = 0;
            MyRemainingTime = 0;
            MyRemainingVolume = 0;
            Medicament = "";
            MyTimer = new Timer();
            MyTimer.Elapsed += new ElapsedEventHandler(ElapsedTimer);
            MyTimer.Interval = 1000;
        }

        public bool TurnOn()
        {
            return MoveNext(Command.TurnOn);
        }

        public bool TurnOff()
        {
            return MoveNext(Command.TurnOff);
        }

        public bool SetInfusionParams(int rate, int volume, string medicament)
        {
            if (MyCurrentState == State.Stop)
            {
                MyRate = rate;
                MyTotalTime = Math.Round((double)volume / rate * 3600, 0);
                MyTotalVolume = volume;
                MyElapsedTime = 0;
                MyInfusedVolume = 0;
                MyRemainingTime = MyTotalTime;
                MyRemainingVolume = MyTotalVolume;
                Medicament = medicament;
                return true;
            }
            return false;
        }

        public bool StartInfusion()
        {
            if (MyCurrentState == State.Stop)
            {
                if (MyRate > 0 && MyRemainingVolume > 0)
                {
                    MoveNext(Command.Start);
                    MyTimer.Enabled = true;
                    MyTimer.Start();
                    return true;
                }
            }
            return false;
        }

        public bool StopInfusion()
        {
            if (!MoveNext(Command.Stop))
            {
                return false;
            }
            MyTimer.Stop();
            return true;
        }

        public bool AcknowledgeAlert()
        {
            if (!MoveNext(Command.Acknowledge))
            {
                return false;
            }
            AlertMessage = "";
            return true;
        }

        public void ConnectToIpAddress(string ipAddress)
        {
            IpAddress = ipAddress;
        }

        private void ElapsedTimer(object sender, ElapsedEventArgs e)
        {
            lock (MyPumpLock)
            {
                // check alarms
                if (MyCurrentState == State.Running && MyRemainingTime <= MyPrealarmStartTime)
                {
                    MoveNext(Command.Warning);
                    AlertMessage = "The infusion is near end";
                }
                if (MyRemainingTime <= 0 || MyRemainingVolume <= 0)
                {
                    MoveNext(Command.Error);
                    AlertMessage = "The infusion ended";
                }

                switch (MyCurrentState)
                {
                    case State.Running:
                    case State.PreAlarm:
                        // update pump
                        MyElapsedTime++;
                        MyRemainingTime = MyTotalTime - MyElapsedTime;
                        MyInfusedVolume = MyRate * MyElapsedTime / 3600;
                        MyRemainingVolume = MyTotalVolume - MyInfusedVolume;
                        // test MyAlarm events
                        foreach (MyAlarm myAlarm in MyAlarms)
                        {
                            if (!myAlarm.Used && MyRemainingTime / MyTotalTime <= myAlarm.ProgressPosition)
                            {
                                myAlarm.Used = true;
                                AlertMessage = myAlarm.Message;
                                MoveNext(Command.Error);
                            }
                        }
                        break;
                    default:
                        MyTimer.Stop();
                        break;
                }
            }
        }

        private bool MoveNext(Command command)
        {
            Transition transition = new Transition(MyCurrentState, command);
            State nextState;
            if (!MyTransitions.TryGetValue(transition, out nextState))
            {
                return false;
            }
            MyCurrentState = nextState;
            return true;
        }

        private enum Type
        {
            Infusion,
            Injection
        }

        private enum State
        {
            Off,
            Stop,
            Running,
            PreAlarm,
            Alarm
        }

        private enum Command
        {
            TurnOn,
            TurnOff,
            Set,
            Start,
            Stop,
            Warning,
            Error,
            Acknowledge
        }

        private class Transition
        {
            readonly State CurrentState;
            readonly Command Command;

            public Transition(State currentState, Command command)
            {
                CurrentState = currentState;
                Command = command;
            }

            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Transition other = obj as Transition;
                return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
            }
        }

        private class MyAlarm
        {
            public bool Used { get; set; }
            public double ProgressPosition { get; set; }
            public string Message { get; set; }
        }
    }
}
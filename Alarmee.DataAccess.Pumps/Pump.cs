﻿using System;
using System.Collections.Generic;
using System.Timers;
using System.Configuration;

namespace Alarmee.DataAccess.Pumps
{
    class Pump
    {
        public string GetSerialNumber { get { return MySerialNumber; } }
        public string GetMedicament { get { return MyMedicament; } }
        public string GetIpAddress { get { return MyIpAddress; } }
        public string GetAlertMessage { get { return MyAlertMessage; } }
        public string GetCurrentState { get { return MyCurrentState.ToString(); } }
        public string GetTypePump { get { return MyType.ToString(); } }
        public int GetTotalTime { get { return Convert.ToInt32(MyTotalTime); } }
        public int GetRemainingTime { get { return Convert.ToInt32(MyRemainingTime); } }

        private double MyRate { get; set; }                   // ml/h

        private double MyTotalVolume { get; set; }           // ml
        private double MyTotalTime { get; set; }             // s

        private double MyInfusedVolume { get; set; }         // ml
        private double MyElapsedTime { get; set; }           // s

        private double MyRemainingVolume { get; set; }       // ml
        private double MyRemainingTime { get; set; }         // s

        private string MySerialNumber { get; set; }
        private string MyMedicament { get; set; }
        private string MyIpAddress { get; set; }
        private string MyAlertMessage { get; set; }
        private object MyPumpLock;
        private Timer MyTimer;
        private Dictionary<Transition, State> MyTransitions;
        private List<MyAlarm> MyAlarms;
        private State MyCurrentState { get; set; }
        private Type MyType { get; set; }

        private double MyPrealarmStartTime = Convert.ToDouble(ConfigurationManager.AppSettings["PreAlarm"]);

        public Pump(string serialNumber, string type = "")
        {
            if (type.ToLower() == Type.Syringe.ToString().ToLower())
            {
                MyType = Type.Syringe;
            }
            else
            {
                MyType = Type.Volumetric;
            }
            MyPumpLock = new object();
            MySerialNumber = serialNumber;
            MyAlertMessage = "";
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
                {new MyAlarm(){Used = false, ProgressPosition = 0.8, Message = "Pressure too high"}},
                {new MyAlarm(){Used = false, ProgressPosition = 0.2, Message = "Door open"}}
            };
            MyCurrentState = State.Off;
            MyRate = 0;
            MyTotalTime = 0;
            MyTotalVolume = 0;
            MyElapsedTime = 0;
            MyInfusedVolume = 0;
            MyRemainingTime = 0;
            MyRemainingVolume = 0;
            MyMedicament = "";
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

        public bool SetInfusionParams(double rate, double volume, string medicament)
        {
            if (MyCurrentState == State.Stop)
            {
                if ((MyType == Type.Volumetric && rate >= 0.1 && rate <= 1200 && volume >= 0.1 && volume <= 1000) ||
                    (MyType == Type.Syringe && rate >= 0.1 && rate <= 1000 && volume >= 0.1 && volume <= 50))
                {
                    MyRate = rate;
                    MyTotalTime = Math.Round((double)volume / rate * 3600, 0);
                    MyTotalVolume = volume;
                    MyElapsedTime = 0;
                    MyInfusedVolume = 0;
                    MyRemainingTime = MyTotalTime;
                    MyRemainingVolume = MyTotalVolume;
                    MyMedicament = medicament;
                    return true;
                }
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
            MyAlertMessage = "";
            return true;
        }

        public void ConnectToIpAddress(string ipAddress)
        {
            MyIpAddress = ipAddress;
        }

        private void ElapsedTimer(object sender, ElapsedEventArgs e)
        {
            lock (MyPumpLock)
            {
                if (MyCurrentState == State.Running && MyRemainingTime <= MyPrealarmStartTime)
                {
                    MoveNext(Command.Warning);
                    MyAlertMessage = "Infusion Near End";
                }
                if (MyRemainingTime <= 0 || MyRemainingVolume <= 0)
                {
                    MoveNext(Command.Error);
                    MyAlertMessage = "Infusion complete";
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
                                MyAlertMessage = myAlarm.Message;
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
            Volumetric,
            Syringe
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
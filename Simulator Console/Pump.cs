using System;
using System.Collections.Generic;
using System.Timers;
using System.Configuration;

namespace Simulator_Console
{
    class Pump
    {
        public string serial_number { get; private set; }
        public string medicament { get; private set; }
        private object pumpLock;
        private Timer timer;
        private Dictionary<Transition, State> transitions;
        private State currentState { get; set; }

        private double rate { get; set; }                   // ml/h

        private double total_time { get; set; }             // s
        private double total_volume { get; set; }           // ml

        private double elapsed_time { get; set; }           // s
        private double infused_volume { get; set; }         // ml

        private double remaining_time { get; set; }         // s
        private double remaining_volume { get; set; }       // ml

        private double prealarm_start_time = Convert.ToDouble(ConfigurationManager.AppSettings["PreAlarm"]);
        private double nurse_max_waiting_time = Convert.ToDouble(ConfigurationManager.AppSettings["Nurse"]);
        private double nurse_waiting_time = 0; // value for nurse action

        public Pump(string serial_number)
        {
            pumpLock = new object();
            this.serial_number = serial_number;
            transitions = new Dictionary<Transition, State>
            {
                { new Transition(State.Off, Command.TurnOn), State.Stop },
                { new Transition(State.Stop, Command.TurnOff), State.Off },
                { new Transition(State.Stop, Command.Start), State.Running },
                { new Transition(State.Running, Command.Stop), State.Stop },
                { new Transition(State.Running, Command.Warning), State.PreAlarm },
                { new Transition(State.Running, Command.Error), State.Alarm },
                { new Transition(State.PreAlarm, Command.Acknowledge), State.Running },
                { new Transition(State.PreAlarm, Command.Error), State.Alarm },
                { new Transition(State.Alarm, Command.Acknowledge), State.Stop }
            };
            currentState = State.Off;
            rate = 0;
            total_time = 0;
            total_volume = 0;
            elapsed_time = 0;
            infused_volume = 0;
            remaining_time = 0;
            remaining_volume = 0;
            medicament = "";
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(ElapsedTimer);
            timer.Interval = 1000;
            timer.Enabled = true;
        }

        public void TurnOn()
        {
            MoveNext(Command.TurnOn);
        }

        public void TurnOff()
        {
            MoveNext(Command.TurnOff);
            timer.Stop();
        }

        public void SetInfusionParams(int nrate, int volume, string medicament)
        {
            if (currentState == State.Stop)
            {
                rate = nrate;
                total_time = Math.Round((double)volume / rate * 3600,0);
                total_volume = volume;
                elapsed_time = 0;
                infused_volume = 0;
                remaining_time = total_time;
                remaining_volume = total_volume;
                this.medicament = medicament;
            }
            else
            {
                Console.WriteLine("Cannot set unstopped pump.");
            }
        }

        public void StartInfusion()
        {
            if (currentState == State.Stop)
            {
                if (rate > 0 && total_volume > 0)
                {
                    MoveNext(Command.Start);
                    timer.Start();
                }
                else
                {
                    Console.WriteLine("Cannot start pump.");
                }
            }

        }

        public void StopInfusion()
        {
            MoveNext(Command.Stop);
        }

        public void AcknowledgeAlert()
        {
            MoveNext(Command.Acknowledge);
        }

        private void ElapsedTimer(object sender, ElapsedEventArgs e)
        {
            lock (pumpLock)
            {
                switch (currentState)
                {
                    case State.Running:
                    case State.PreAlarm:
                        // update pump
                        elapsed_time++;
                        remaining_time = total_time - elapsed_time;
                        infused_volume = rate * elapsed_time / 3600;
                        remaining_volume = total_volume - infused_volume;
                        // check alarms
                        if (remaining_time == prealarm_start_time)
                            MoveNext(Command.Warning);
                        if (remaining_time <= 0 || remaining_volume <= 0)
                            MoveNext(Command.Error);
                        // test alarm event
                        if (Math.Round(remaining_time / total_time, 2) == 0.80 || Math.Round(remaining_time / total_time, 2) == 0.20)
                            MoveNext(Command.Error);
                        break;
                    default:
                        break;
                }
                NurseAction();
                Console.WriteLine("Current state: " + currentState + " Remaining time: " + remaining_time + " Remaining volume: " + Math.Round(remaining_volume, 2));
            }
        }

        private void NurseAction()
        {
            switch (currentState)
            {
                case State.PreAlarm:
                case State.Alarm:
                    if (nurse_waiting_time >= nurse_max_waiting_time)
                    {
                        nurse_waiting_time = 0;
                        AcknowledgeAlert();
                    }
                    nurse_waiting_time++;
                    break;
                case State.Stop:
                    if (nurse_waiting_time >= nurse_max_waiting_time)
                    {
                        nurse_waiting_time = 0;
                        if (remaining_time <= 0 || remaining_volume <= 0)
                        {
                            TurnOff();
                        }
                        else
                        {
                            StartInfusion();
                        }
                    }
                    nurse_waiting_time++;
                    break;
                default:
                    break;
            }
        }

        private void MoveNext(Command command)
        {
            Transition transition = new Transition(currentState, command);
            State nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                Console.WriteLine("Invalid transition: " + currentState + " -> " + command);
            else
                currentState = nextState;
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
    }
}

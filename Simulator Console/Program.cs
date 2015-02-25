using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Pump pump = new Pump("51351032");
            pump.TurnOn();
            pump.SetInfusionParams(500,15,"Morfium");
            pump.StartInfusion();
            Console.ReadLine();
        }
    }
}

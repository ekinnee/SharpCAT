using SharpCATLib;
using System;

namespace SharpCATConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SharpCAT sharpCAT = new SharpCAT();

            Console.WriteLine("Ports found: ");
            foreach (var port in sharpCAT.PortNames)
            {
                Console.WriteLine(port);
            }

            Console.ReadKey();
        }

        private static string SharpCAT_PortsSelected(string[] portnames)
        {
            throw new NotImplementedException();
        }
    }
}
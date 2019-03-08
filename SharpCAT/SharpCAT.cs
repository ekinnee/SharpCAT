using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace SharpCATLib
{
    public class SharpCAT
    {
        private delegate string OnPortsSelected(string[] portnames);

        private static event OnPortsSelected PortsSelected;

        public SharpCAT() => SharpCAT.PortsSelected += new OnPortsSelected(ConnectPorts);

        public string[] PortsToUse { get; set; }

        public double[] CTCSSTones = { 67.0, 69.3, 71.9, 74.4, 77.0, 79.7, 82.5, 85.4, 88.5,
            91.5, 94.8, 97.4, 100.0, 103.5, 107.2, 110.9, 114.8, 118.8, 123, 127.3, 131.8,
            136.5, 141.3, 146.2, 151.4, 156.7, 162.2, 167.9, 173.8, 179.9, 186.2, 192.8,
            203.5, 210.7, 218.1, 225.7, 233.6, 241.8, 250.3 };

        public int[] DCSCodes = { 023, 025, 026, 031, 032, 036, 043, 047, 051, 053, 054, 065,
            071, 072, 073, 074, 114, 115, 116, 122, 125, 131, 132, 134, 143, 145, 152, 155,
            156, 162, 165, 172, 174, 205, 212, 223, 225, 226, 243, 244, 245, 246, 251, 252, 255,
            261, 263, 265, 266, 271, 274, 306, 311, 315, 325, 331, 332, 343, 346, 351, 356, 364,
            365, 371, 411, 412, 413, 423, 431, 432, 445, 446, 452, 454, 455, 462, 464, 465, 466,
            503, 506, 516, 523, 526, 532, 546, 565, 606, 612, 624, 627, 631, 632, 654, 662, 664,
            703, 712, 723, 731, 732, 734, 743, 754 };

        public string[] PortNames { get => SerialPort.GetPortNames(); }

        public enum BaudRates : int { TwelveHundred = 1200, TwentyFourHundred = 2400, FourtyEightHUndred = 4800, NinteySixHundred = 9600, NineteenTwo = 19200, ThirtyEightFour = 38400 };

        public static int[] DataBits { get; } = new int[] { 7, 8 };

        public static string[] RadioTypes { get; } = new string[] { "CAT", "CIV" };

        private string ConnectPorts(string[] portnames)
        {
            List<Serial> ports = new List<Serial>();

            foreach (string port in portnames)
            {
                //Testing
                ports.Add(new Serial("COM11", BaudRates.ThirtyEightFour, Parity.None, StopBits.Two, Handshake.None));
            }
            return "";
        }
    }
}
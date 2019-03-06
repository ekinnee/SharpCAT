using System;
using System.IO.Ports;
using System.Threading;

namespace SharpCAT
{
    public class Serial
    {
        private SerialPort _serialPort;

        public string[] PortNames { get => SerialPort.GetPortNames(); }
        public static int[] BaudRates { get; } = new int[] { 1200, 4800, 9600, 19200, 38400 };

        public static int[] DataBits { get; } = new int[] { 7, 8 };

        public enum Parity
        {
            Even = System.IO.Ports.Parity.Even,
            Mark = System.IO.Ports.Parity.Mark,
            None = System.IO.Ports.Parity.None,
            Odd = System.IO.Ports.Parity.Odd,
            Space = System.IO.Ports.Parity.Space
        }

        public enum StopBits
        {
            None = System.IO.Ports.StopBits.None,
            One = System.IO.Ports.StopBits.One,
            OnePointFive = System.IO.Ports.StopBits.OnePointFive,
            Two = System.IO.Ports.StopBits.Two
        }

        public enum Handshake
        {
            None = System.IO.Ports.Handshake.None,
            RequestToSend = System.IO.Ports.Handshake.RequestToSend,
            RequestToSendXOnXOff = System.IO.Ports.Handshake.RequestToSendXOnXOff,
            XOnXOff = System.IO.Ports.Handshake.XOnXOff
        }

        public Serial(string portname, int baudrate, Parity parity, StopBits bits, Handshake handshake)
        {
            _serialPort = new SerialPort
            {
                ReadTimeout = 500,
                WriteTimeout = 500,
                PortName = portname,
                BaudRate = baudrate,
                Parity = (System.IO.Ports.Parity)parity,
                StopBits = (System.IO.Ports.StopBits)bits,
                Handshake = (System.IO.Ports.Handshake)handshake
            };

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceived);
            _serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialErrorReceived);

        }

        private void SerialErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {

        }

        private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {

        }

        public void Read()
        {

            try
            {
                string message = _serialPort.ReadLine();
                //Console.WriteLine(message);
            }
            catch (TimeoutException) { }
        }

        public void GetRXStatus()
        {

        }

        public void GetFreqAndMode()
        {

        }
    }
}
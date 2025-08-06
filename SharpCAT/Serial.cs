using System;
using System.IO.Ports;

namespace SharpCAT
{
    public class Serial
    {
        private SerialPort _serialPort;

        //Init
        public Serial(string portname, SharpCAT.BaudRates baudrate, Parity parity, StopBits bits, Handshake handshake)
        {
            _serialPort = new SerialPort
            {
                ReadTimeout = 500,
                WriteTimeout = 500,
                PortName = portname,
                BaudRate = (int)baudrate,
                Parity = parity,
                StopBits = bits,
                Handshake = handshake
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

        public void ProbeSerialPort(SerialPort port)
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
    }
}
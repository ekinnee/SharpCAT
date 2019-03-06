namespace SharpCAT.Models.Radios.Yaesu
{
    internal class FT818 : Radio
    {     
        public struct Lock
        {
            public static readonly string ON = "00";
            public static readonly string OFF = "80";
        }

        public struct Ptt
        {
            public static readonly string ON = "08";
            public static readonly string OFF = "88";
        }

        public struct Clar
        {
            public static readonly string ON = "05";
            public static readonly string OFF = "85";
        }

        public struct Split
        {
            public static readonly string ON = "02";
            public static readonly string OFF = "82";
        }

        public struct Power
        {
            public static readonly string ON = "0F";
            public static readonly string OFF = "8F";
        }

        public struct OpModes
        {
            public static readonly string LSB = "00";
            public static readonly string USB = "01";
            public static readonly string CW = "02";
            public static readonly string CWR = "03";
            public static readonly string AM = "04";
            public static readonly string FM = "08";
            public static readonly string DIG = "0A";
            public static readonly string PKT = "0C";
        }

        public string LockOn()
        {
            string _cmd = CmdPad + Lock.ON;
            return _cmd;
        }

        public string LockOff()
        {
            string _cmd = CmdPad + Lock.OFF;
            return _cmd;
        }

        public string PttOn()
        {
            string _cmd = CmdPad + Ptt.ON;
            return _cmd;
        }

        public string PttOff()
        {
            string _cmd = CmdPad + Ptt.OFF;
            return _cmd;
        }

        public string ClarOn()
        {
            string _cmd = CmdPad + Clar.ON;
            return _cmd;
        }

        public string ClarOff()
        {
            string _cmd = CmdPad + Clar.OFF;
            return _cmd;
        }

        public string SplitOn()
        {
            string _cmd = CmdPad + Split.ON;
            return _cmd;
        }

        public string SplitOff()
        {
            string _cmd = CmdPad + Split.OFF;
            return _cmd;
        }

        public string PowerOn()
        {
            string _cmd = CmdPad + Power.ON;
            return _cmd;
        }

        public string PowerOff()
        {
            string _cmd = CmdPad + Power.OFF;
            return _cmd;
        }

        public void SetFreq(double freq)
        {
        }

        public void SetOpMode(string opmode)
        {
        }

        public void SetVFO()
        {
        }

        public void GetRXStatus()
        {

        }

        public void GetTXStatus()
        {

        }

        public void GetFreqAndModeStatus()
        {

        }
    }
}
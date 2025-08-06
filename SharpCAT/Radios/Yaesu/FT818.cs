using SharpCAT.Models;

namespace SharpCAT.Radios.Yaesu
{
    public class FT818 : CATRadio
    {
        public string RadioMfg => "Yaesu";
        public string RadioModel => "FT-818";

        public string CmdPad => "00000000";

        public class Lock
        {
            public static readonly string ON = "00";
            public static readonly string OFF = "80";
        }

        public class Ptt
        {
            public static readonly string ON = "08";
            public static readonly string OFF = "88";
        }

        public class Clar
        {
            public static readonly string ON = "05";
            public static readonly string OFF = "85";
        }

        public class Split
        {
            public static readonly string ON = "02";
            public static readonly string OFF = "82";
        }

        public class Power
        {
            public static readonly string ON = "0F";
            public static readonly string OFF = "8F";
        }

        public string VFOToggle => "81";

        public class ToneMode
        {
            public static readonly string DCS = "0A";
            public static readonly string CTCSS = "2A";
            public static readonly string ENCODER = "4A";
            public static readonly string OFF = "8A";
        }

        public class OpModes
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

        private string LockOn()
        {
            return "";
        }

        private string LockOff()
        {
            return "";
        }

        private string PttOn()
        {
            return "";
        }

        private string PttOff()
        {
            return "";
        }

        private string ClarOn()
        {
            return "";
        }

        private string ClarOff()
        {
            return "";
        }

        private string SplitOn()
        {
            return "";
        }

        private string SplitOff()
        {
            return "";
        }

        private string PowerOn()
        {
            return "";
        }

        private string PowerOff()
        {
            return "";
        }

        private string SetFreq(double freq)
        {
            return "";
        }

        private string SetOpMode(OpModes opmode)
        {
            return "";
        }

        private string SwitchVFO()
        {
            return "";
        }

        private string SetToneMode(ToneMode mode)
        {
            return "";
        }

        private string GetRXStatus()
        {
            return "";
        }

        private string GetTXStatus()
        {
            return "";
        }

        private string GetFreqAndModeStatus()
        {
            return "";
        }
    }
}